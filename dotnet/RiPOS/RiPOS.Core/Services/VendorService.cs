using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RiPOS.Core.Interfaces;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.Services
{
    public class VendorService(IVendorRepository vendorRepository, IMapper mapper) : IVendorService
    {
        public async Task<ICollection<VendorResponse>> GetAllAsync(bool includeInactives = false)
        {
            var vendors = await vendorRepository.GetAllAsync(v => v.IsActive || includeInactives,
                includeProps: v => v.Include(x => x.CountryState!));

            var vendorsResponse = mapper.Map<ICollection<VendorResponse>>(vendors);
            return vendorsResponse;
        }

        public async Task<VendorResponse?> GetByIdAsync(int id)
        {
            var vendor = await vendorRepository.FindAsync(v => v.Id == id,
                includeProps: v => v.Include(x => x.CountryState!));

            var vendorResponse = mapper.Map<VendorResponse>(vendor);
            return vendorResponse;
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await vendorRepository.ExistsAsync(v => v.Id == id && v.IsActive);
        }

        public async Task<MessageResponse<VendorResponse>> AddAsync(VendorRequest request, int userId)
        {
            var messageResponse = new MessageResponse<VendorResponse>();

            var vendor = mapper.Map<Vendor>(request);

            vendor.CreationByUserId = userId;
            vendor.LastModificationByUserId = userId;
            vendor.IsActive = true;

            var exists = await vendorRepository
                .ExistsAsync(v => vendor.Email != null && v.Email != null && vendor.Email != null && v.Email.ToUpper() == vendor.Email.ToUpper()
                    && v.IsActive);

            if (exists)
            {
                messageResponse.Success = false;
                messageResponse.Message = $"Ya existe un proveedor con el email \"{vendor.Email}\"";
                return messageResponse;
            }

            messageResponse.Success = await vendorRepository.AddAsync(vendor);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Proveedor agregado correctamente";
                messageResponse.Data = mapper.Map<VendorResponse>(vendor);
            }
            else
            {
                messageResponse.Success = false;
                messageResponse.Message = "No se realizó ningún cambio";
            }

            return messageResponse;
        }

        public async Task<MessageResponse<VendorResponse>> UpdateAsync(int id, VendorRequest request, int userId)
        {
            var messageResponse = new MessageResponse<VendorResponse>();

            var vendor = await vendorRepository.GetByIdAsync(id);

            if (vendor != null)
            {
                var exists = await vendorRepository.
                    ExistsAsync(v => v.Id != vendor.Id && vendor.Email != null && v.Email != null && v.Email.ToUpper() == vendor.Email.ToUpper() 
                                     && v.IsActive);

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

                vendor.LastModificationByUserId = userId;

                messageResponse.Success = await vendorRepository.UpdateAsync(vendor);

                if (messageResponse.Success)
                {
                    messageResponse.Success = true;
                    messageResponse.Message = $"Proveedor modificado correctamente";
                    messageResponse.Data = mapper.Map<VendorResponse>(vendor);
                }
                else
                {
                    messageResponse.Success = false;
                    messageResponse.Message = "No se realizó ningún cambio";
                }
            }
            else
            {
                messageResponse.Success = false;
                messageResponse.Message = "Proveedor no encontrado";
            }


            return messageResponse;
        }

        public async Task<MessageResponse<string>> DeactivateAsync(int id, int userId)
        {
            var messageResponse = new MessageResponse<string>();

            var vendor = await vendorRepository.GetByIdAsync(id);

            if (vendor != null)
            {
                vendor.IsActive = false;
                vendor.LastModificationByUserId = userId;

                messageResponse.Success = await vendorRepository.UpdateAsync(vendor);

                if (messageResponse.Success)
                {
                    messageResponse.Success = true;
                    messageResponse.Data = $"Proveedor eliminado correctamente";
                }
                else
                {
                    messageResponse.Success = false;
                    messageResponse.Message = "No se realizó ningún cambio";
                }
            }
            else
            {
                messageResponse.Success = false;
                messageResponse.Message = "Proveedor no encontrado";  
            }
            
            return messageResponse;
        }
    }
}
