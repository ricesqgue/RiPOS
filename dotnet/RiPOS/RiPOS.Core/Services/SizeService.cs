using AutoMapper;
using RiPOS.Core.Interfaces;
using RiPOS.Repository.Interfaces;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Models;
using RiPOS.Domain.Entities;

namespace RiPOS.Core.Services
{
    public class SizeService : ISizeService
    {
        private readonly IMapper _mapper;
        private readonly ISizeRepository _sizeRepository;

        public SizeService(ISizeRepository sizeRepository, IMapper mapper)
        {
            _sizeRepository = sizeRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<SizeResponse>> GetAllAsync(int companyId, bool includeInactives = false)
        {
            var sizes = await _sizeRepository.GetAllAsync(s => s.CompanyId == companyId && (s.IsActive || includeInactives));

            var sizesReponse = _mapper.Map<ICollection<SizeResponse>>(sizes);
            return sizesReponse;
        }

        public async Task<SizeResponse> GetByIdAsync(int id, int companyId)
        {
            var size = await _sizeRepository.FindAsync(s => s.Id == id && s.CompanyId == companyId);

            var sizeResponse = _mapper.Map<SizeResponse>(size);
            return sizeResponse;
        }

        public async Task<bool> ExistsByIdAsync(int id, int companyId)
        {
            return await _sizeRepository.ExistsAsync(s => s.Id == id && s.CompanyId == companyId && s.IsActive);
        }

        public async Task<MessageResponse<SizeResponse>> AddAsync(SizeRequest request, UserSession userSession)
        {
            var messageResponse = new MessageResponse<SizeResponse>();

            var size = _mapper.Map<Size>(request);

            size.CreationByUserId = userSession.UserId;
            size.LastModificationByUserId = userSession.UserId;
            size.CompanyId = userSession.CompanyId;
            size.IsActive = true;

            var exists = await _sizeRepository.FindAsync(s => (s.ShortName == request.ShortName.Trim().ToUpper() || s.Name.ToUpper() == request.Name.Trim().ToUpper()) && s.IsActive && s.CompanyId == userSession.CompanyId);

            if (exists != null)
            {
                messageResponse.Success = false;
                if (exists.Name.ToUpper() == request.Name.Trim().ToUpper())
                {
                    messageResponse.Message = $"Ya existe una talla con el nombre \"{size.Name}\"";
                }
                else
                {
                    messageResponse.Message = $"Ya existe una talla con el nombre corto \"{size.ShortName.ToUpper()}\"";
                }
                return messageResponse;
            }

            messageResponse.Success = await _sizeRepository.AddAsync(size);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Talla agregada correctamente";
                messageResponse.Data = _mapper.Map<SizeResponse>(size);
            }
            else
            {
                messageResponse.Message = "No se realizó ningún cambio";
            }

            return messageResponse;
        }

        public async Task<MessageResponse<SizeResponse>> UpdateAsync(int id, SizeRequest request, UserSession userSession)
        {
            var messageResponse = new MessageResponse<SizeResponse>();

            var size = await _sizeRepository.GetByIdAsync(id);

            var exists = await _sizeRepository.FindAsync(s => s.Id != size.Id && (s.ShortName == size.ShortName.ToUpper() || s.Name.ToUpper() == size.Name.ToUpper())
                && s.CompanyId == userSession.CompanyId && s.IsActive);

            if (exists != null)
            {
                messageResponse.Success = false;
                if (exists.Name.ToUpper() == request.Name.Trim().ToUpper())
                {
                    messageResponse.Message = $"Ya existe una talla con el nombre \"{size.Name}\"";
                }
                else
                {
                    messageResponse.Message = $"Ya existe una talla con el nombre corto \"{size.ShortName.ToUpper()}\"";
                }
                return messageResponse;
            }

            size.Name = request.Name.Trim();
            size.ShortName = request.ShortName.ToUpper().Trim();
            size.LastModificationByUserId = userSession.UserId;

            messageResponse.Success = await _sizeRepository.UpdateAsync(size);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Talla modificada correctamente";
                messageResponse.Data = _mapper.Map<SizeResponse>(size);
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

            var size = await _sizeRepository.GetByIdAsync(id);

            size.IsActive = false;
            size.LastModificationByUserId = userSession.UserId;

            messageResponse.Success = await _sizeRepository.UpdateAsync(size);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Talla eliminada correctamente";
            }
            else
            {
                messageResponse.Message = "No se realizó ningún cambio";
            }

            return messageResponse;
        }
    }
}
