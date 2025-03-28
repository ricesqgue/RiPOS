﻿using AutoMapper;
using RiPOS.Core.Interfaces;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Models;

namespace RiPOS.Core.Services
{
    public class GenderService(IGenderRepository genderRepository, IMapper mapper) : IGenderService
    {
        public async Task<ICollection<GenderResponse>> GetAllAsync(int companyId, bool includeInactives = false)
        {
            var genders = await genderRepository.GetAllAsync(c => c.IsActive || includeInactives);

            var gendersResponse = mapper.Map<ICollection<GenderResponse>>(genders);
            return gendersResponse;
        }

        public async Task<GenderResponse> GetByIdAsync(int id, int companyId)
        {
            var gender = await genderRepository.FindAsync(g => g.Id == id);

            var genderResponse = mapper.Map<GenderResponse>(gender);
            return genderResponse;
        }

        public async Task<bool> ExistsByIdAsync(int id, int companyId)
        {
            return await genderRepository.ExistsAsync(g => g.Id == id && g.IsActive);
        }

        public async Task<MessageResponse<GenderResponse>> AddAsync(GenderRequest request, UserSession userSession)
        {
            var messageResponse = new MessageResponse<GenderResponse>();

            var gender = mapper.Map<Gender>(request);

            gender.CreationByUserId = userSession.UserId;
            gender.LastModificationByUserId = userSession.UserId;
            gender.IsActive = true;

            var exists = await genderRepository
                .ExistsAsync(g => g.Name.ToUpper() == request.Name.Trim().ToUpper() && g.IsActive);

            if (exists)
            {
                messageResponse.Success = false;
                messageResponse.Message = $"Ya existe un género con el nombre \"{gender.Name}\"";
                return messageResponse;
            }

            messageResponse.Success = await genderRepository.AddAsync(gender);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Género agregado correctamente";
                messageResponse.Data = mapper.Map<GenderResponse>(gender);
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

            var gender = await genderRepository.GetByIdAsync(id);

            var exists = await genderRepository
                .ExistsAsync(g => g.Id != gender.Id && g.Name.ToUpper() == gender.Name.ToUpper() && g.IsActive);

            if (exists)
            {
                messageResponse.Success = false;
                messageResponse.Message = $"Ya existe un género con el nombre \"{gender.Name}\"";
                return messageResponse;
            }

            gender.Name = request.Name.Trim();
            gender.LastModificationByUserId = userSession.UserId;

            messageResponse.Success = await genderRepository.UpdateAsync(gender);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Género modificado correctamente";
                messageResponse.Data = mapper.Map<GenderResponse>(gender);
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

            var gender = await genderRepository.GetByIdAsync(id);

            gender.IsActive = false;
            gender.LastModificationByUserId = userSession.UserId;

            messageResponse.Success = await genderRepository.UpdateAsync(gender);

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
