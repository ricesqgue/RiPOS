using AutoMapper;
using RiPOS.Core.Interfaces;
using RiPOS.Repository.Interfaces;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Domain.Entities;

namespace RiPOS.Core.Services
{
    public class BrandService(IBrandRepository brandRepository, IMapper mapper) : IBrandService
    {
        public async Task<ICollection<BrandResponse>> GetAllAsync(bool includeInactives = false)
        {
            var brands = await brandRepository.GetAllAsync(b => (b.IsActive || includeInactives));

            var brandsResponse = mapper.Map<ICollection<BrandResponse>>(brands);
            return brandsResponse;
        }

        public async Task<BrandResponse?> GetByIdAsync(int id)
        {
            var brand = await brandRepository.FindAsync(b => b.Id == id);

            var brandResponse = mapper.Map<BrandResponse>(brand);
            return brandResponse;
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await brandRepository.ExistsAsync(b => b.Id == id && b.IsActive);
        }

        public async Task<MessageResponse<BrandResponse>> AddAsync(BrandRequest request, int userId)
        {
            var messageResponse = new MessageResponse<BrandResponse>();

            var brand = mapper.Map<Brand>(request);

            brand.CreationByUserId = userId;
            brand.LastModificationByUserId = userId;
            brand.IsActive = true;

            var exists = await brandRepository.ExistsAsync(b => b.Name.ToUpper() == request.Name.Trim().ToUpper() && b.IsActive);

            if (exists)
            {
                messageResponse.Success = false;
                messageResponse.Message = $"Ya existe una marca con el nombre \"{brand.Name}\"";
                return messageResponse;
            }

            messageResponse.Success = await brandRepository.AddAsync(brand);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Marca agregada correctamente";
                messageResponse.Data = mapper.Map<BrandResponse>(brand);
            }
            else
            {
                messageResponse.Success = false;
                messageResponse.Message = "No se realizó ningún cambio";
            }

            return messageResponse;
        }

        public async Task<MessageResponse<BrandResponse>> UpdateAsync(int id, BrandRequest request, int userId)
        {
            var messageResponse = new MessageResponse<BrandResponse>();

            var brand = await brandRepository.GetByIdAsync(id);

            if (brand != null)
            {
                var exists = await brandRepository.ExistsAsync(b => b.Id != brand.Id && b.Name.ToUpper() == request.Name.ToUpper()
                    && b.IsActive);

                if (exists)
                {
                    messageResponse.Success = false;
                    messageResponse.Message = $"Ya existe una marca con el nombre \"{brand.Name}\"";
                    return messageResponse;
                }

                brand.Name = request.Name.Trim();
                brand.LastModificationByUserId = userId;

                messageResponse.Success = await brandRepository.UpdateAsync(brand);

                if (messageResponse.Success)
                {
                    messageResponse.Success = true;
                    messageResponse.Message = $"Marca modificada correctamente";
                    messageResponse.Data = mapper.Map<BrandResponse>(brand);
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
                messageResponse.Message = "No se encontró la marca";
            }
            return messageResponse;
        }

        public async Task<MessageResponse<string>> DeactivateAsync(int id, int userId)
        {
            var messageResponse = new MessageResponse<string>();

            var brand = await brandRepository.GetByIdAsync(id);

            if (brand != null)
            {
                brand.IsActive = false;
                brand.LastModificationByUserId = userId;

                messageResponse.Success = await brandRepository.UpdateAsync(brand);

                if (messageResponse.Success)
                {
                    messageResponse.Success = true;
                    messageResponse.Data = $"Marca eliminada correctamente";
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
                messageResponse.Message = "No se encontró la marca";
            }

            return messageResponse;
        }
    }
}
