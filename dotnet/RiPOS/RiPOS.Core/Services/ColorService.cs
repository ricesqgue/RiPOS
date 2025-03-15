using AutoMapper;
using RiPOS.Core.Interfaces;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Models;

namespace RiPOS.Core.Services
{
    public class ColorService : IColorService
    {
        private readonly IMapper _mapper;
        private readonly IColorRepository _colorRepository;

        public ColorService(IColorRepository colorRepository, IMapper mapper)
        {
            _colorRepository = colorRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<ColorResponse>> GetAllAsync(int companyId, bool includeInactives = false)
        {
            var colors = await _colorRepository.GetAllAsync(c => c.CompanyId == companyId && (c.IsActive || includeInactives));

            var colorsReponse = _mapper.Map<ICollection<ColorResponse>>(colors);
            return colorsReponse;
        }

        public async Task<ColorResponse> GetByIdAsync(int id, int companyId)
        {
            var color = await _colorRepository.FindAsync(c => c.Id == id && c.CompanyId == companyId);

            var colorResponse = _mapper.Map<ColorResponse>(color);
            return colorResponse;
        }

        public async Task<bool> ExistsByIdAsync(int id, int companyId)
        {
            return await _colorRepository.ExistsAsync(c => c.Id == id && c.CompanyId == companyId && c.IsActive);
        }

        public async Task<MessageResponse<ColorResponse>> AddAsync(ColorRequest request, UserSession userSession)
        {
            var messageResponse = new MessageResponse<ColorResponse>();

            var color = _mapper.Map<Color>(request);

            color.CreationByUserId = userSession.UserId;
            color.LastModificationByUserId = userSession.UserId;
            color.CompanyId = userSession.CompanyId;
            color.IsActive = true;

            var exists = await _colorRepository.FindAsync(c => (c.Name.ToUpper() == request.Name.Trim().ToUpper() || c.RgbHex.ToUpper() == request.RgbHex.Trim().ToUpper()) && c.IsActive && c.CompanyId == userSession.CompanyId);

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

            messageResponse.Success = await _colorRepository.AddAsync(color);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Color agregado correctamente";
                messageResponse.Data = _mapper.Map<ColorResponse>(color);
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

            var color = await _colorRepository.GetByIdAsync(id);

            var exists = await _colorRepository.FindAsync(c => c.Id != color.Id && (c.Name.ToUpper() == color.Name.ToUpper() || c.RgbHex.ToUpper() == color.RgbHex.ToUpper())
                && c.CompanyId == userSession.CompanyId && c.IsActive);

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


            messageResponse.Success = await _colorRepository.UpdateAsync(color);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Color modificado correctamente";
                messageResponse.Data = _mapper.Map<ColorResponse>(color);
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

            var color = await _colorRepository.GetByIdAsync(id);

            color.IsActive = false;
            color.LastModificationByUserId = userSession.UserId;

            messageResponse.Success = await _colorRepository.UpdateAsync(color);

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
