using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RiPOS.Core.Interfaces;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;
using RiPOS.Shared.Models;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.Services
{
    public class VendorService : IVendorService
    {
        private readonly IMapper _mapper;
        private readonly IVendorRepository _vendorRepository;

        public VendorService(IVendorRepository vendorRepository, IMapper mapper)
        {
            _vendorRepository = vendorRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<VendorResponse>> GetAllAsync(int companyId, bool includeInactives = false)
        {
            var vendors = await _vendorRepository.GetAllAsync(v => v.CompanyId == companyId && (v.IsActive || includeInactives),
                includeProps: v => v.Include(x => x.CountryState));

            var vendorsReponse = _mapper.Map<ICollection<VendorResponse>>(vendors);
            return vendorsReponse;
        }

        public async Task<VendorResponse> GetByIdAsync(int id, int companyId)
        {
            var vendor = await _vendorRepository.FindAsync(v => v.Id == id && v.CompanyId == companyId,
                includeProps: c => c.Include(x => x.CountryState));

            var vendorResponse = _mapper.Map<VendorResponse>(vendor);
            return vendorResponse;
        }

        public async Task<bool> ExistsByIdAsync(int id, int companyId)
        {
            return await _vendorRepository.ExistsAsync(v => v.Id == id && v.CompanyId == companyId && v.IsActive);
        }

        public async Task<MessageResponse<VendorResponse>> AddAsync(VendorRequest request, UserSession userSession)
        {
            var messageResponse = new MessageResponse<VendorResponse>();

            var vendor = _mapper.Map<Vendor>(request);

            vendor.CreationByUserId = userSession.UserId;
            vendor.LastModificationByUserId = userSession.UserId;
            vendor.CompanyId = userSession.CompanyId;
            vendor.IsActive = true;

            var exists = await _vendorRepository
                .ExistsAsync(v => vendor.Email != null && v.Email.ToUpper() == vendor.Email.ToUpper()
                    && v.IsActive && v.CompanyId == userSession.CompanyId);

            if (exists)
            {
                messageResponse.Success = false;
                messageResponse.Message = $"Ya existe un proveedor con el email \"{vendor.Email}\"";
                return messageResponse;
            }

            messageResponse.Success = await _vendorRepository.AddAsync(vendor);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Proveedor agregado correctamente";
                messageResponse.Data = _mapper.Map<VendorResponse>(vendor);
            }
            else
            {
                messageResponse.Message = "No se realizó ningún cambio";
            }

            return messageResponse;
        }

        public async Task<MessageResponse<VendorResponse>> UpdateAsync(int id, VendorRequest request, UserSession userSession)
        {
            var messageResponse = new MessageResponse<VendorResponse>();

            var vendor = await _vendorRepository.GetByIdAsync(id);

            var exists = await _vendorRepository.ExistsAsync(v => v.Id != vendor.Id && vendor.Email != null && v.Email.ToUpper() == vendor.Email.ToUpper()
                && v.CompanyId == userSession.CompanyId && v.IsActive);

            if (exists)
            {
                messageResponse.Success = false;
                messageResponse.Message = $"Ya existe un proveedor con el email \"{vendor.Email}\"";
                return messageResponse;
            }

            vendor.Name = request.Name.Trim();
            vendor.Surname = request.Surname.Trim();
            vendor.SecondSurname = request.SecondSurname?.Trim();
            vendor.Email = request.Email?.Trim();
            vendor.PhoneNumber = request.PhoneNumber?.Trim();
            vendor.MobilePhone = request.MobilePhone?.Trim();
            vendor.Address = request.Address?.Trim();
            vendor.City = request.City?.Trim();
            vendor.ZipCode = request.ZipCode?.Trim();
            vendor.CountryStateId = request.CountryStateId;

            vendor.LastModificationByUserId = userSession.UserId;

            messageResponse.Success = await _vendorRepository.UpdateAsync(vendor);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Proveedor modificado correctamente";
                messageResponse.Data = _mapper.Map<VendorResponse>(vendor);
            }
            else
            {
                messageResponse.Message = "No se realizó ningún cambio";
            }

            return messageResponse;
        }

        public async Task<MessageResponse<string>> DeactivateAsync(int id, UserSession userSession)
        {
            var messageResponse = new MessageResponse<string>();

            var vendor = await _vendorRepository.GetByIdAsync(id);

            vendor.IsActive = false;
            vendor.LastModificationByUserId = userSession.UserId;

            messageResponse.Success = await _vendorRepository.UpdateAsync(vendor);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Proveedor eliminado correctamente";
            }
            else
            {
                messageResponse.Message = "No se realizó ningún cambio";
            }

            return messageResponse;
        }
    }
}
