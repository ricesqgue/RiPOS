using AutoMapper;
using RiPOS.Core.Interfaces;
using RiPOS.Repository.Interfaces;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Models;
using RiPOS.Domain.Entities;

namespace RiPOS.Core.Services
{
    public class CashRegisterService : ICashRegisterService
    {
        private readonly IMapper _mapper;
        private readonly ICashRegisterRepository _cashRegisterRepository;

        public CashRegisterService(ICashRegisterRepository cashRegisterRepository, IMapper mapper)
        {
            _cashRegisterRepository = cashRegisterRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<CashRegisterResponse>> GetAllAsync(int storeId, bool includeInactives = false)
        {
            var cashRegisters = await _cashRegisterRepository.GetAllAsync(c => c.StoreId == storeId && (c.IsActive || includeInactives));

            var cashRegistersReponse = _mapper.Map<ICollection<CashRegisterResponse>>(cashRegisters);
            return cashRegistersReponse;
        }

        public async Task<CashRegisterResponse> GetByIdAsync(int id, int storeId)
        {
            var cashRegister = await _cashRegisterRepository.FindAsync(c => c.Id == id && c.StoreId == storeId);

            var cashRegisterResponse = _mapper.Map<CashRegisterResponse>(cashRegister);
            return cashRegisterResponse;
        }

        public async Task<bool> ExistsByIdAsync(int id, int storeId)
        {
            return await _cashRegisterRepository.ExistsAsync(c => c.Id == id && c.StoreId == storeId && c.IsActive);
        }

        public async Task<MessageResponse<CashRegisterResponse>> AddAsync(CashRegisterRequest request, UserSession userSession)
        {
            var messageResponse = new MessageResponse<CashRegisterResponse>();

            var cashRegister = _mapper.Map<CashRegister>(request);

            cashRegister.CreationByUserId = userSession.UserId;
            cashRegister.LastModificationByUserId = userSession.UserId;
            cashRegister.StoreId = userSession.StoreId;
            cashRegister.IsActive = true;

            var exists = await _cashRegisterRepository.ExistsAsync(c => c.Name.ToUpper() == request.Name.Trim().ToUpper() && c.IsActive && c.StoreId == userSession.StoreId);

            if (exists)
            {
                messageResponse.Success = false;
                messageResponse.Message = $"Ya existe una caja con el nombre \"{cashRegister.Name}\"";

                return messageResponse;
            }

            messageResponse.Success = await _cashRegisterRepository.AddAsync(cashRegister);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Caja agregada correctamente";
                messageResponse.Data = _mapper.Map<CashRegisterResponse>(cashRegister);
            }
            else
            {
                messageResponse.Message = "No se realizó ningún cambio";
            }

            return messageResponse;
        }

        public async Task<MessageResponse<CashRegisterResponse>> UpdateAsync(int id, CashRegisterRequest request, UserSession userSession)
        {
            var messageResponse = new MessageResponse<CashRegisterResponse>();
            var cashRegister = await _cashRegisterRepository.GetByIdAsync(id);

            var exists = await _cashRegisterRepository.ExistsAsync(c => c.Id != cashRegister.Id && c.Name.ToUpper() == cashRegister.Name.ToUpper()
                && c.StoreId == userSession.StoreId && c.IsActive);

            if (exists)
            {
                messageResponse.Success = false;
                messageResponse.Message = $"Ya existe una caja con el nombre \"{cashRegister.Name}\"";
                return messageResponse;
            }

            cashRegister.Name = request.Name.Trim();
            cashRegister.LastModificationByUserId = userSession.UserId;

            messageResponse.Success = await _cashRegisterRepository.UpdateAsync(cashRegister);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Caja modificada correctamente";
                messageResponse.Data = _mapper.Map<CashRegisterResponse>(cashRegister);
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

            var cashRegister = await _cashRegisterRepository.GetByIdAsync(id);

            cashRegister.IsActive = false;
            cashRegister.LastModificationByUserId = userSession.UserId;

            messageResponse.Success = await _cashRegisterRepository.UpdateAsync(cashRegister);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Caja eliminada correctamente";
            }
            else
            {
                messageResponse.Message = "No se realizó ningún cambio";
            }

            return messageResponse;
        }
    }
}
