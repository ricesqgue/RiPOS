using AutoMapper;
using RiPOS.Core.Interfaces;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Models;

namespace RiPOS.Core.Services
{
    public class GenderService : IGenderService
    {
        private readonly IMapper _mapper;
        private readonly IGenderRepository _genderRepository;

        public GenderService(IGenderRepository genderRepository, IMapper mapper)
        {
            _genderRepository = genderRepository;
            _mapper = mapper;
        }


        public async Task<ICollection<GenderResponse>> GetAllAsync(int companyId, bool includeInactives = false)
        {
            var genders = await _genderRepository.GetAllAsync(c => c.CompanyId == companyId && (c.IsActive || includeInactives));

            var gendersReponse = _mapper.Map<ICollection<GenderResponse>>(genders);
            return gendersReponse;
        }

        public async Task<GenderResponse> GetByIdAsync(int id, int companyId)
        {
            var gender = await _genderRepository.FindAsync(g => g.Id == id && g.CompanyId == companyId);

            var genderResponse = _mapper.Map<GenderResponse>(gender);
            return genderResponse;
        }

        public async Task<bool> ExistsByIdAsync(int id, int companyId)
        {
            return await _genderRepository.ExistsAsync(g => g.Id == id && g.CompanyId == companyId && g.IsActive);
        }

        public async Task<MessageResponse<GenderResponse>> AddAsync(GenderRequest request, UserSession userSession)
        {
            var messageResponse = new MessageResponse<GenderResponse>();

            var gender = _mapper.Map<Gender>(request);

            gender.CreationByUserId = userSession.UserId;
            gender.LastModificationByUserId = userSession.UserId;
            gender.CompanyId = userSession.CompanyId;
            gender.IsActive = true;

            var exists = await _genderRepository.ExistsAsync(g => g.Name.ToUpper() == request.Name.Trim().ToUpper() && g.IsActive && g.CompanyId == userSession.CompanyId);

            if (exists)
            {
                messageResponse.Success = false;
                messageResponse.Message = $"Ya existe un género con el nombre \"{gender.Name}\"";
                return messageResponse;
            }

            messageResponse.Success = await _genderRepository.AddAsync(gender);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Género agregado correctamente";
                messageResponse.Data = _mapper.Map<GenderResponse>(gender);
            }
            else
            {
                messageResponse.Message = "No se realizó ningún cambio";
            }

            return messageResponse;
        }

        public async Task<MessageResponse<GenderResponse>> UpdateAsync(int id, GenderRequest request, UserSession userSession)
        {
            var messageResponse = new MessageResponse<GenderResponse>();

            var gender = await _genderRepository.GetByIdAsync(id);

            var exists = await _genderRepository.ExistsAsync(g => g.Id != gender.Id && g.Name.ToUpper() == gender.Name.ToUpper()
                && g.CompanyId == userSession.CompanyId && g.IsActive);

            if (exists)
            {
                messageResponse.Success = false;
                messageResponse.Message = $"Ya existe un género con el nombre \"{gender.Name}\"";
                return messageResponse;
            }

            gender.Name = request.Name.Trim();
            gender.LastModificationByUserId = userSession.UserId;

            messageResponse.Success = await _genderRepository.UpdateAsync(gender);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Género modificado correctamente";
                messageResponse.Data = _mapper.Map<GenderResponse>(gender);
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

            var gender = await _genderRepository.GetByIdAsync(id);

            gender.IsActive = false;
            gender.LastModificationByUserId = userSession.UserId;

            messageResponse.Success = await _genderRepository.UpdateAsync(gender);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Género eliminado correctamente";
            }
            else
            {
                messageResponse.Message = "No se realizó ningún cambio";
            }

            return messageResponse;
        }
    }
}
