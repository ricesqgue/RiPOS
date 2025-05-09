﻿using AutoMapper;
using RiPOS.Core.Interfaces;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;
using RiPOS.Repository.Session;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.Services;

public class ColorService(IRepositorySessionFactory repositorySessionFactory, IColorRepository colorRepository, IMapper mapper) : IColorService
{
    public async Task<ICollection<ColorResponse>> GetAllAsync(bool includeInactives = false)
    {
        var colors = await colorRepository.GetAllAsync(c => c.IsActive || includeInactives);

        var colorsResponse = mapper.Map<ICollection<ColorResponse>>(colors);
        return colorsResponse;
    }

    public async Task<ColorResponse?> GetByIdAsync(int id)
    {
        var color = await colorRepository.FindAsync(c => c.Id == id);

        var colorResponse = mapper.Map<ColorResponse>(color);
        return colorResponse;
    }

    public async Task<bool> ExistsByIdAsync(int id)
    {
        return await colorRepository.ExistsAsync(c => c.Id == id && c.IsActive);
    }

    public async Task<MessageResponse<ColorResponse>> AddAsync(ColorRequest request, int userId)
    {
        var messageResponse = new MessageResponse<ColorResponse>();
        var session = await repositorySessionFactory.CreateAsync();

        try
        {
            var color = mapper.Map<Color>(request);

            color.CreationByUserId = userId;
            color.LastModificationByUserId = userId;
            color.IsActive = true;
        
            var exists = await colorRepository
                .FindAsync(c => (c.Name.ToUpper() == request.Name.Trim().ToUpper() ||  c.RgbHex.ToUpper() == request.RgbHex.Trim().ToUpper()) 
                                && c.IsActive);

            if (exists != null)
            {
                messageResponse.Success = false;
                if (string.Equals(exists.Name, request.Name.Trim(), StringComparison.CurrentCultureIgnoreCase))
                {
                    messageResponse.Message = $"Ya existe un color con el nombre \"{color.Name}\"";
                }
                else
                {
                    messageResponse.Message = $"Ya existe un color con el código \"{color.RgbHex}\"";
                }
                return messageResponse;
            }

            await colorRepository.AddAsync(color);
            await session.CommitAsync();
            
            messageResponse.Success = true;
            messageResponse.Message = $"Color agregado correctamente";
            messageResponse.Data = mapper.Map<ColorResponse>(color);
        }
        catch
        {
            await session.RollbackAsync();
            messageResponse.Success = false;
            messageResponse.Message = "No se realizó ningún cambio";
        }
        
        return messageResponse;
    }

    public async Task<MessageResponse<ColorResponse>> UpdateAsync(int id, ColorRequest request, int userId)
    {
        var messageResponse = new MessageResponse<ColorResponse>();
        var session = await repositorySessionFactory.CreateAsync();

        try
        {
            var color = await colorRepository.GetByIdAsync(id);

            if (color != null)
            {
                var exists = await colorRepository
                    .FindAsync(c => c.Id != color.Id && (c.Name.ToUpper() == request.Name.ToUpper() || c.RgbHex.ToUpper() == request.RgbHex.ToUpper())
                                                     && c.IsActive);

                if (exists != null)
                {
                    messageResponse.Success = false;
                    if (string.Equals(exists.Name, request.Name.Trim(), StringComparison.CurrentCultureIgnoreCase))
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
                color.LastModificationByUserId = userId;
                
                colorRepository.Update(color);
                await session.CommitAsync();
          
                messageResponse.Success = true;
                messageResponse.Message = $"Color modificado correctamente";
                messageResponse.Data = mapper.Map<ColorResponse>(color);
                
            }
            else
            {
                messageResponse.Success = false;
                messageResponse.Message = "Color no encontrado";
            }
        }
        catch
        {
            await session.RollbackAsync();
            messageResponse.Success = false;
            messageResponse.Message = "No se realizó ningún cambio";
        }
        
        return messageResponse;
    }

    public async Task<MessageResponse<string>> DeactivateAsync(int id, int userId)
    {
        var messageResponse = new MessageResponse<string>();
        var session = await repositorySessionFactory.CreateAsync();

        try
        {
            var color = await colorRepository.GetByIdAsync(id);

            if (color != null)
            {
                color.IsActive = false;
                color.LastModificationByUserId = userId;

                colorRepository.Update(color);
                await session.CommitAsync();
                
                messageResponse.Success = true;
                messageResponse.Data = $"Color eliminado correctamente";
            }
            else
            {
                messageResponse.Success = false;
                messageResponse.Message = "Color no encontrado";
            }
        }
        catch
        {
            await session.CommitAsync();
            messageResponse.Success = false;
            messageResponse.Message = "No se realizó ningún cambio";
        }
        
        return messageResponse;
    }
}