using AutoMapper;
using RiPOS.Core.Interfaces;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Models;

namespace RiPOS.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<CategoryResponse>> GetAllAsync(int companyId, bool includeInactives = false)
        {
            var categories = await _categoryRepository.GetAllAsync(c => c.CompanyId == companyId && (c.IsActive || includeInactives));

            var categoriesReponse = _mapper.Map<ICollection<CategoryResponse>>(categories);
            return categoriesReponse;
        }

        public async Task<CategoryResponse> GetByIdAsync(int id, int companyId)
        {
            var category = await _categoryRepository.FindAsync(c => c.Id == id && c.CompanyId == companyId);

            var categoryResponse = _mapper.Map<CategoryResponse>(category);
            return categoryResponse;
        }

        public async Task<bool> ExistsByIdAsync(int id, int companyId)
        {
            return await _categoryRepository.ExistsAsync(c => c.Id == id && c.CompanyId == companyId && c.IsActive);
        }

        public async Task<MessageResponse<CategoryResponse>> AddAsync(CategoryRequest request, UserSession userSession)
        {
            var messageResponse = new MessageResponse<CategoryResponse>();

            var category = _mapper.Map<Category>(request);

            category.CreationByUserId = userSession.UserId;
            category.LastModificationByUserId = userSession.UserId;
            category.CompanyId = userSession.CompanyId;
            category.IsActive = true;

            var exists = await _categoryRepository.ExistsAsync(c => c.Name.ToUpper() == request.Name.Trim().ToUpper() && c.IsActive && c.CompanyId == userSession.CompanyId);

            if (exists)
            {
                messageResponse.Success = false;
                messageResponse.Message = $"Ya existe una categoría con el nombre \"{category.Name}\"";

                return messageResponse;
            }

            messageResponse.Success = await _categoryRepository.AddAsync(category);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Categoría agregada correctamente";
                messageResponse.Data = _mapper.Map<CategoryResponse>(category);
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
            var category = await _categoryRepository.GetByIdAsync(id);

            var exists = await _categoryRepository.ExistsAsync(c => c.Id != category.Id && c.Name.ToUpper() == category.Name.ToUpper()
                && c.CompanyId == userSession.CompanyId && c.IsActive);

            if (exists)
            {
                messageResponse.Success = false;
                messageResponse.Message = $"Ya existe una categoría con el nombre \"{category.Name}\"";
                return messageResponse;
            }

            category.Name = request.Name.Trim();
            category.LastModificationByUserId = userSession.UserId;

            messageResponse.Success = await _categoryRepository.UpdateAsync(category);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Categoría modificada correctamente";
                messageResponse.Data = _mapper.Map<CategoryResponse>(category);
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

            var category = await _categoryRepository.GetByIdAsync(id);

            category.IsActive = false;
            category.LastModificationByUserId = userSession.UserId;

            messageResponse.Success = await _categoryRepository.UpdateAsync(category);

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
