using AutoMapper;
using RiPOS.Core.Interfaces;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Models;

namespace RiPOS.Core.Services
{
    public class CategoryService(ICategoryRepository categoryRepository, IMapper mapper) : ICategoryService
    {
        public async Task<ICollection<CategoryResponse>> GetAllAsync(int companyId, bool includeInactives = false)
        {
            var categories = await categoryRepository.GetAllAsync(c => (c.IsActive || includeInactives));

            var categoriesResponse = mapper.Map<ICollection<CategoryResponse>>(categories);
            return categoriesResponse;
        }

        public async Task<CategoryResponse> GetByIdAsync(int id, int companyId)
        {
            var category = await categoryRepository.FindAsync(c => c.Id == id);

            var categoryResponse = mapper.Map<CategoryResponse>(category);
            return categoryResponse;
        }

        public async Task<bool> ExistsByIdAsync(int id, int companyId)
        {
            return await categoryRepository.ExistsAsync(c => c.Id == id && c.IsActive);
        }

        public async Task<MessageResponse<CategoryResponse>> AddAsync(CategoryRequest request, UserSession userSession)
        {
            var messageResponse = new MessageResponse<CategoryResponse>();

            var category = mapper.Map<Category>(request);

            category.CreationByUserId = userSession.UserId;
            category.LastModificationByUserId = userSession.UserId;
            category.IsActive = true;

            var exists = await categoryRepository
                .ExistsAsync(c => c.Name.ToUpper() == request.Name.Trim().ToUpper() && c.IsActive);

            if (exists)
            {
                messageResponse.Success = false;
                messageResponse.Message = $"Ya existe una categoría con el nombre \"{category.Name}\"";

                return messageResponse;
            }

            messageResponse.Success = await categoryRepository.AddAsync(category);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Categoría agregada correctamente";
                messageResponse.Data = mapper.Map<CategoryResponse>(category);
            }
            else
            {
                messageResponse.Message = "No se realizó ningún cambio";
            }

            return messageResponse;
        }

        public async Task<MessageResponse<CategoryResponse>> UpdateAsync(int id, CategoryRequest request, UserSession userSession)
        {
            var messageResponse = new MessageResponse<CategoryResponse>();
            var category = await categoryRepository.GetByIdAsync(id);

            var exists = await categoryRepository
                .ExistsAsync(c => c.Id != category.Id && c.Name.ToUpper() == category.Name.ToUpper() && c.IsActive);

            if (exists)
            {
                messageResponse.Success = false;
                messageResponse.Message = $"Ya existe una categoría con el nombre \"{category.Name}\"";
                return messageResponse;
            }

            category.Name = request.Name.Trim();
            category.LastModificationByUserId = userSession.UserId;

            messageResponse.Success = await categoryRepository.UpdateAsync(category);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Categoría modificada correctamente";
                messageResponse.Data = mapper.Map<CategoryResponse>(category);
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

            var category = await categoryRepository.GetByIdAsync(id);

            category.IsActive = false;
            category.LastModificationByUserId = userSession.UserId;

            messageResponse.Success = await categoryRepository.UpdateAsync(category);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Categoría eliminada correctamente";
            }
            else
            {
                messageResponse.Message = "No se realizó ningún cambio";
            }

            return messageResponse;
        }
    }
}
