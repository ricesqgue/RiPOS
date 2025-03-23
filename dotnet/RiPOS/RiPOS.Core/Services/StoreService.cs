using AutoMapper;
using RiPOS.Core.Interfaces;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;
using RiPOS.Repository.Session;
using RiPOS.Shared.Models;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.Services
{
    public class StoreService(
        IStoreRepository storeRepository,
        IRepositorySessionFactory repositorySessionFactory,
        IMapper mapper)
        : IStoreService
    {
        private readonly IRepositorySessionFactory _repositorySessionFactory = repositorySessionFactory;

        public async Task<ICollection<StoreResponse>> GetAllAsync(int companyId)
        {
            var stores = await storeRepository.GetAllAsync();

            var storesResponse = mapper.Map<ICollection<StoreResponse>>(stores);

            return storesResponse;
        }

        public async Task<StoreResponse> GetByIdAsync(int id, int companyId)
        {
            var store = await storeRepository.FindAsync(s => s.Id == id);

            var storeResponse = mapper.Map<StoreResponse>(store);

            return storeResponse;
        }

        public async Task<bool> ExistsByIdAsync(int id, int companyId)
        {
            return await storeRepository.ExistsAsync(s => s.Id == id && s.IsActive);
        }

        public async Task<MessageResponse<StoreResponse>> AddAsync(StoreRequest request, UserSession userSession)
        {
            var messageResponse = new MessageResponse<StoreResponse>();

            var store = mapper.Map<Store>(request);

            store.CreationByUserId = userSession.UserId;
            store.LastModificationByUserId = userSession.UserId;
            store.IsActive = true;

            var exists = await storeRepository.ExistsAsync(s => s.Name.ToUpper() == request.Name.Trim().ToUpper() && s.IsActive);

            if (exists)
            {
                messageResponse.Success = false;
                messageResponse.Message = $"Ya existe una tienda con el nombre \"{store.Name}\"";
                return messageResponse;
            }

            messageResponse.Success = await storeRepository.AddAsync(store);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Tienda agregada correctamente";
                messageResponse.Data = mapper.Map<StoreResponse>(store);
            }
            else
            {
                messageResponse.Message = "No se realizó ningún cambio";
            }

            return messageResponse;
        }

        public async Task<MessageResponse<StoreResponse>> UpdateAsync(int id, StoreRequest request, UserSession userSession)
        {
            var messageResponse = new MessageResponse<StoreResponse>();

            var store = await storeRepository.GetByIdAsync(id);

            var exists = await storeRepository
                .ExistsAsync(s => s.Id != store.Id && s.Name.ToUpper() == store.Name.ToUpper() && s.IsActive);

            if (exists)
            {
                messageResponse.Success = false;
                messageResponse.Message = $"Ya existe una tienda con el nombre \"{store.Name}\"";
                return messageResponse;
            }

            store.Name = request.Name.Trim();
            store.Address = request.Address?.Trim();
            store.PhoneNumber = request.PhoneNumber?.Trim();
            store.MobilePhone = request.MobilePhone?.Trim();
            store.LastModificationByUserId = userSession.UserId;

            messageResponse.Success = await storeRepository.UpdateAsync(store);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Tienda modificada correctamente";
                messageResponse.Data = mapper.Map<StoreResponse>(store);
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

            var store = await storeRepository.GetByIdAsync(id);

            store.IsActive = false;
            store.LastModificationByUserId = userSession.UserId;

            messageResponse.Success = await storeRepository.UpdateAsync(store);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Tienda eliminada correctamente";
            }
            else
            {
                messageResponse.Message = "No se realizó ningún cambio";
            }

            return messageResponse;
        }
    }
}
