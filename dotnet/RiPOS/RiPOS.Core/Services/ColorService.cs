using AutoMapper;
using RiPOS.Core.Interfaces;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Models;

namespace RiPOS.Core.Services
{
    public class ColorService(IColorRepository colorRepository, IMapper mapper) : IColorService
    {
        public async Task<ICollection<ColorResponse>> GetAllAsync(int companyId, bool includeInactives = false)
        {
            var colors = await colorRepository.GetAllAsync(c => c.IsActive || includeInactives);

            var colorsResponse = mapper.Map<ICollection<ColorResponse>>(colors);
            return colorsResponse;
        }

        public async Task<ColorResponse> GetByIdAsync(int id, int companyId)
        {
            var color = await colorRepository.FindAsync(c => c.Id == id);

            var colorResponse = mapper.Map<ColorResponse>(color);
            return colorResponse;
        }

        public async Task<bool> ExistsByIdAsync(int id, int companyId)
        {
            return await colorRepository.ExistsAsync(c => c.Id == id && c.IsActive);
        }

        public async Task<MessageResponse<ColorResponse>> AddAsync(ColorRequest request, UserSession userSession)
        {
            var messageResponse = new MessageResponse<ColorResponse>();

            var color = mapper.Map<Color>(request);

            color.CreationByUserId = userSession.UserId;
            color.LastModificationByUserId = userSession.UserId;
            color.IsActive = true;

            var exists = await colorRepository
                .FindAsync(c => (c.Name.ToUpper() == request.Name.Trim().ToUpper() || c.RgbHex.ToUpper() == request.RgbHex.Trim().ToUpper()) 
                                && c.IsActive);

            if (exists != null)
            {
                messageResponse.Success = false;
                if (exists.Name.ToUpper() == request.Name.Trim().ToUpper())
                {
                    messageResponse.Message = $"Ya existe un color con el nombre \"{color.Name}\"";
                }
                else
                {
                    messageResponse.Message = $"Ya existe un color con el código \"{color.RgbHex}\"";
                }
                return messageResponse;
            }

            messageResponse.Success = await colorRepository.AddAsync(color);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Color agregado correctamente";
                messageResponse.Data = mapper.Map<ColorResponse>(color);
            }
            else
            {
                messageResponse.Message = "No se realizó ningún cambio";
            }

            return messageResponse;
        }

        public async Task<MessageResponse<ColorResponse>> UpdateAsync(int id, ColorRequest request, UserSession userSession)
        {
            var messageResponse = new MessageResponse<ColorResponse>();

            var color = await colorRepository.GetByIdAsync(id);

            var exists = await colorRepository
                .FindAsync(c => c.Id != color.Id && (c.Name.ToUpper() == color.Name.ToUpper() || c.RgbHex.ToUpper() == color.RgbHex.ToUpper())
                && c.IsActive);

            if (exists != null)
            {
                messageResponse.Success = false;
                if (exists.Name.ToUpper() == request.Name.Trim().ToUpper())
                {
                    messageResponse.Message = $"Ya existe un color con el nombre \"{color.Name}\"";
                }
                else
                {
                    messageResponse.Message = $"Ya existe un color con el código \"{color.RgbHex}\"";
                }
                return messageResponse;
            }

            color.Name = request.Name.Trim();
            color.RgbHex = request.RgbHex.Trim();

            color.LastModificationByUserId = userSession.UserId;


            messageResponse.Success = await colorRepository.UpdateAsync(color);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Color modificado correctamente";
                messageResponse.Data = mapper.Map<ColorResponse>(color);
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

            var color = await colorRepository.GetByIdAsync(id);

            color.IsActive = false;
            color.LastModificationByUserId = userSession.UserId;

            messageResponse.Success = await colorRepository.UpdateAsync(color);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Color eliminado correctamente";
            }
            else
            {
                messageResponse.Message = "No se realizó ningún cambio";
            }

            return messageResponse;
        }
    }
}
