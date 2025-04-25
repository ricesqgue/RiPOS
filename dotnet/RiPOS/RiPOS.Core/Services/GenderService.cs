using AutoMapper;
using RiPOS.Core.Interfaces;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.Services;

public class GenderService(IGenderRepository genderRepository, IMapper mapper) : IGenderService
{
    public async Task<ICollection<GenderResponse>> GetAllAsync(bool includeInactives = false)
    {
        var genders = await genderRepository.GetAllAsync(g => g.IsActive || includeInactives);

        var gendersResponse = mapper.Map<ICollection<GenderResponse>>(genders);
        return gendersResponse;
    }

    public async Task<GenderResponse?> GetByIdAsync(int id)
    {
        var gender = await genderRepository.FindAsync(g => g.Id == id);

        var genderResponse = mapper.Map<GenderResponse>(gender);
        return genderResponse;
    }

    public async Task<bool> ExistsByIdAsync(int id)
    {
        return await genderRepository.ExistsAsync(g => g.Id == id && g.IsActive);
    }

    public async Task<MessageResponse<GenderResponse>> AddAsync(GenderRequest request, int userId)
    {
        var messageResponse = new MessageResponse<GenderResponse>();

        var gender = mapper.Map<Gender>(request);

        gender.CreationByUserId = userId;
        gender.LastModificationByUserId = userId;
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
            messageResponse.Success = false;
            messageResponse.Message = "No se realizó ningún cambio";
        }

        return messageResponse;
    }

    public async Task<MessageResponse<GenderResponse>> UpdateAsync(int id, GenderRequest request, int userId)
    {
        var messageResponse = new MessageResponse<GenderResponse>();

        var gender = await genderRepository.GetByIdAsync(id);

        if (gender != null)
        {
            var exists = await genderRepository
                .ExistsAsync(g => g.Id != gender.Id && g.Name.ToUpper() == request.Name.ToUpper() && g.IsActive);

            if (exists)
            {
                messageResponse.Success = false;
                messageResponse.Message = $"Ya existe un género con el nombre \"{gender.Name}\"";
                return messageResponse;
            }

            gender.Name = request.Name.Trim();
            gender.LastModificationByUserId = userId;

            messageResponse.Success = await genderRepository.UpdateAsync(gender);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Género modificado correctamente";
                messageResponse.Data = mapper.Map<GenderResponse>(gender);
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
            messageResponse.Message = "Género no encontrado";
        }
        return messageResponse;
    }

    public async Task<MessageResponse<string>> DeactivateAsync(int id, int userId)
    {
        var messageResponse = new MessageResponse<string>();

        var gender = await genderRepository.GetByIdAsync(id);

        if (gender != null)
        {
            gender.IsActive = false;
            gender.LastModificationByUserId = userId;

            messageResponse.Success = await genderRepository.UpdateAsync(gender);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Data = $"Género eliminado correctamente";
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
            messageResponse.Message = "Género no encontrado";
        }
        return messageResponse;
    }
}