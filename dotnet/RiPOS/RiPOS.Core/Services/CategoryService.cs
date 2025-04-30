using AutoMapper;
using RiPOS.Core.Interfaces;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;
using RiPOS.Repository.Session;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.Services;

public class CategoryService(IRepositorySessionFactory repositorySessionFactory, ICategoryRepository categoryRepository, IMapper mapper) : ICategoryService
{
    public async Task<ICollection<CategoryResponse>> GetAllAsync(bool includeInactives = false)
    {
        var categories = await categoryRepository.GetAllAsync(c => (c.IsActive || includeInactives));

        var categoriesResponse = mapper.Map<ICollection<CategoryResponse>>(categories);
        return categoriesResponse;
    }

    public async Task<CategoryResponse?> GetByIdAsync(int id)
    {
        var category = await categoryRepository.FindAsync(c => c.Id == id);

        var categoryResponse = mapper.Map<CategoryResponse>(category);
        return categoryResponse;
    }

    public async Task<bool> ExistsByIdAsync(int id)
    {
        return await categoryRepository.ExistsAsync(c => c.Id == id && c.IsActive);
    }

    public async Task<MessageResponse<CategoryResponse>> AddAsync(CategoryRequest request, int userId)
    {
        var messageResponse = new MessageResponse<CategoryResponse>();
        var session = await repositorySessionFactory.CreateAsync();

        try
        {
            var exists = await categoryRepository
                .ExistsAsync(c => c.Name.ToUpper() == request.Name.Trim().ToUpper() && c.IsActive);

            if (exists)
            {
                messageResponse.Success = false;
                messageResponse.Message = $"Ya existe una categoría con el nombre \"{request.Name.Trim()}\"";

                return messageResponse;
            }
            
            var category = mapper.Map<Category>(request);

            category.CreationByUserId = userId;
            category.LastModificationByUserId = userId;
            category.IsActive = true;

            await categoryRepository.AddAsync(category);
            await session.CommitAsync();
            
            messageResponse.Success = true;
            messageResponse.Message = $"Categoría agregada correctamente";
            messageResponse.Data = mapper.Map<CategoryResponse>(category);
        }
        catch
        {
            await session.RollbackAsync();
            messageResponse.Success = false;
            messageResponse.Message = "No se realizó ningún cambio";
        }
        return messageResponse;
    }

    public async Task<MessageResponse<CategoryResponse>> UpdateAsync(int id, CategoryRequest request, int userId)
    {
        var messageResponse = new MessageResponse<CategoryResponse>();
        var session = await repositorySessionFactory.CreateAsync();

        try
        {
            var category = await categoryRepository.GetByIdAsync(id);

            if (category != null)
            {
                var exists = await categoryRepository
                    .ExistsAsync(c => c.Id != category.Id && c.Name.ToUpper() == request.Name.ToUpper() && c.IsActive);

                if (exists)
                {
                    messageResponse.Success = false;
                    messageResponse.Message = $"Ya existe una categoría con el nombre \"{category.Name}\"";
                    return messageResponse;
                }

                category.Name = request.Name.Trim();
                category.LastModificationByUserId = userId;

                categoryRepository.Update(category);
                await session.CommitAsync();
                
                messageResponse.Success = true;
                messageResponse.Message = $"Categoría modificada correctamente";
                messageResponse.Data = mapper.Map<CategoryResponse>(category);

            }
            else
            {
                messageResponse.Success = false;
                messageResponse.Message = "Categoría no encontrada";
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
            var category = await categoryRepository.GetByIdAsync(id);

            if (category != null)
            {
                category.IsActive = false;
                category.LastModificationByUserId = userId;

                categoryRepository.Update(category);
                await session.CommitAsync();
                
                messageResponse.Success = true;
                messageResponse.Data = $"Categoría eliminada correctamente";
            }
            else
            {
                messageResponse.Success = false;
                messageResponse.Message = "Categoría no encontrada";
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