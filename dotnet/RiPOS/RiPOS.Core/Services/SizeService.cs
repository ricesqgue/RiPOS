using AutoMapper;
using RiPOS.Core.Interfaces;
using RiPOS.Repository.Interfaces;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Session;

namespace RiPOS.Core.Services;

public class SizeService(IRepositorySessionFactory repositorySessionFactory, ISizeRepository sizeRepository, IMapper mapper) : ISizeService
{
    public async Task<ICollection<SizeResponse>> GetAllAsync(bool includeInactives = false)
    {
        var sizes = await sizeRepository.GetAllAsync(s => s.IsActive || includeInactives);

        var sizesResponse = mapper.Map<ICollection<SizeResponse>>(sizes);
        return sizesResponse;
    }

    public async Task<SizeResponse?> GetByIdAsync(int id)
    {
        var size = await sizeRepository.FindAsync(s => s.Id == id);

        var sizeResponse = mapper.Map<SizeResponse>(size);
        return sizeResponse;
    }

    public async Task<bool> ExistsByIdAsync(int id)
    {
        return await sizeRepository.ExistsAsync(s => s.Id == id && s.IsActive);
    }

    public async Task<MessageResponse<SizeResponse>> AddAsync(SizeRequest request, int userId)
    {
        var messageResponse = new MessageResponse<SizeResponse>();
        var session = await repositorySessionFactory.CreateAsync();

        try
        {
            var size = mapper.Map<Size>(request);

            size.CreationByUserId = userId;
            size.LastModificationByUserId = userId;
            size.IsActive = true;

            var exists = await sizeRepository
                .FindAsync(s => (s.ShortName == request.ShortName.Trim().ToUpper() || s.Name.ToUpper() == request.Name.Trim().ToUpper()) 
                                && s.IsActive);

            if (exists != null)
            {
                messageResponse.Success = false;
                if (string.Equals(exists.Name, request.Name.Trim(), StringComparison.CurrentCultureIgnoreCase))
                {
                    messageResponse.Message = $"Ya existe una talla con el nombre \"{size.Name}\"";
                }
                else
                {
                    messageResponse.Message = $"Ya existe una talla con el nombre corto \"{size.ShortName.ToUpper()}\"";
                }
                return messageResponse;
            }

            await sizeRepository.AddAsync(size);
            await session.CommitAsync();
            
            messageResponse.Success = true;
            messageResponse.Message = $"Talla agregada correctamente";
            messageResponse.Data = mapper.Map<SizeResponse>(size);
        }
        catch
        {
            await session.RollbackAsync();
            messageResponse.Success = false;
            messageResponse.Message = "No se realizó ningún cambio";
        }
        
        return messageResponse;
    }

    public async Task<MessageResponse<SizeResponse>> UpdateAsync(int id, SizeRequest request, int userId)
    {
        var messageResponse = new MessageResponse<SizeResponse>();
        var session = await repositorySessionFactory.CreateAsync();

        try
        {
            var size = await sizeRepository.GetByIdAsync(id);
            if (size != null)
            {
                var exists = await sizeRepository
                    .FindAsync(s =>
                        s.Id != size.Id && (s.ShortName == request.ShortName.ToUpper() ||
                                            s.Name.ToUpper() == request.Name.ToUpper())
                                        && s.IsActive);

                if (exists != null)
                {
                    messageResponse.Success = false;
                    if (string.Equals(exists.Name, request.Name.Trim(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        messageResponse.Message = $"Ya existe una talla con el nombre \"{size.Name}\"";
                    }
                    else
                    {
                        messageResponse.Message =
                            $"Ya existe una talla con el nombre corto \"{size.ShortName.ToUpper()}\"";
                    }

                    return messageResponse;
                }

                size.Name = request.Name.Trim();
                size.ShortName = request.ShortName.ToUpper().Trim();
                size.LastModificationByUserId = userId;

                sizeRepository.Update(size);
                await session.CommitAsync();
                
                messageResponse.Success = true;
                messageResponse.Message = $"Talla modificada correctamente";
                messageResponse.Data = mapper.Map<SizeResponse>(size);
            }
            else
            {
                messageResponse.Success = false;
                messageResponse.Message = "Talla no encontrada";
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
            var size = await sizeRepository.GetByIdAsync(id);

            if (size != null)
            {
                size.IsActive = false;
                size.LastModificationByUserId = userId;

                sizeRepository.Update(size);
                await session.CommitAsync();
                
                messageResponse.Success = true;
                messageResponse.Data = $"Talla eliminada correctamente";
            }
            else
            {
                messageResponse.Success = false;
                messageResponse.Message = "Talla no encontrada";
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
}