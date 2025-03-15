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
    public class StoreService : IStoreService
    {
        private readonly IMapper _mapper;
        private readonly IStoreRepository _storeRepository;
        private readonly IRepositorySessionFactory _repositorySessionFactory;

        public StoreService(IStoreRepository storeRepository, IRepositorySessionFactory repositorySessionFactory, IMapper mapper)
        {
            _mapper = mapper;
            _storeRepository = storeRepository;
            _repositorySessionFactory = repositorySessionFactory;
        }

        public async Task<ICollection<StoreResponse>> GetAllAsync(int companyId)
        {
            var stores = await _storeRepository.GetAllAsync(s => s.CompanyId == companyId);

            var storesResponse = _mapper.Map<ICollection<StoreResponse>>(stores);

            return storesResponse;
        }

        public async Task<StoreResponse> GetByIdAsync(int id, int companyId)
        {
            var store = await _storeRepository.FindAsync(s => s.Id == id && s.CompanyId == companyId);

            var storeResponse = _mapper.Map<StoreResponse>(store);

            return storeResponse;
        }

        public async Task<bool> ExistsByIdAsync(int id, int companyId)
        {
            return await _storeRepository.ExistsAsync(s => s.Id == id && s.CompanyId == companyId && s.IsActive);
        }

        public async Task<MessageResponse<StoreResponse>> AddAsync(StoreRequest request, UserSession userSession)
        {
            var messageResponse = new MessageResponse<StoreResponse>();

            var store = _mapper.Map<Store>(request);

            store.CreationByUserId = userSession.UserId;
            store.LastModificationByUserId = userSession.UserId;
            store.CompanyId = userSession.CompanyId;
            store.IsActive = true;

            var exists = await _storeRepository.ExistsAsync(s => s.Name.ToUpper() == request.Name.Trim().ToUpper() && s.IsActive && s.CompanyId == userSession.CompanyId);

            if (exists)
            {
                messageResponse.Success = false;
                messageResponse.Message = $"Ya existe una tienda con el nombre \"{store.Name}\"";
                return messageResponse;
            }

            messageResponse.Success = await _storeRepository.AddAsync(store);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Tienda agregada correctamente";
                messageResponse.Data = _mapper.Map<StoreResponse>(store);
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

            var store = await _storeRepository.GetByIdAsync(id);

            var exists = await _storeRepository.ExistsAsync(s => s.Id != store.Id && s.Name.ToUpper() == store.Name.ToUpper()
                && s.CompanyId == userSession.CompanyId && s.IsActive);

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

            messageResponse.Success = await _storeRepository.UpdateAsync(store);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Tienda modificada correctamente";
                messageResponse.Data = _mapper.Map<StoreResponse>(store);
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

            var store = await _storeRepository.GetByIdAsync(id);

            store.IsActive = false;
            store.LastModificationByUserId = userSession.UserId;

            messageResponse.Success = await _storeRepository.UpdateAsync(store);

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
