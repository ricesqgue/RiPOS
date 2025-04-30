using AutoMapper;
using RiPOS.Core.Interfaces;
using RiPOS.Repository.Interfaces;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Session;
using RiPOS.Shared.Models.Session;

namespace RiPOS.Core.Services;

public class CashRegisterService(IRepositorySessionFactory repositorySessionFactory, ICashRegisterRepository cashRegisterRepository, IMapper mapper) : ICashRegisterService
{
        
    public async Task<ICollection<CashRegisterResponse>> GetAllAsync(int storeId, bool includeInactives = false)
    {
        var cashRegisters = await cashRegisterRepository
            .GetAllAsync(c => c.StoreId == storeId && (c.IsActive || includeInactives));

        var cashRegistersResponse = mapper.Map<ICollection<CashRegisterResponse>>(cashRegisters);
        return cashRegistersResponse;
    }

    public async Task<CashRegisterResponse?> GetByIdAsync(int id, int storeId)
    {
        var cashRegister = await cashRegisterRepository
            .FindAsync(c => c.Id == id && c.StoreId == storeId);

        var cashRegisterResponse = mapper.Map<CashRegisterResponse>(cashRegister);
        return cashRegisterResponse;
    }

    public async Task<bool> ExistsByIdAsync(int id, int storeId)
    {
        return await cashRegisterRepository.ExistsAsync(c => c.Id == id && c.StoreId == storeId && c.IsActive);
    }

    public async Task<MessageResponse<CashRegisterResponse>> AddAsync(CashRegisterRequest request, UserSession userSession)
    {
        var messageResponse = new MessageResponse<CashRegisterResponse>();
        var session = await repositorySessionFactory.CreateAsync();

        try
        {
            var exists = await cashRegisterRepository
                .ExistsAsync(c => c.Name.ToUpper() == request.Name.Trim().ToUpper() 
                                  && c.IsActive && c.StoreId == userSession.StoreId);

            if (exists)
            {
                messageResponse.Success = false;
                messageResponse.Message = $"Ya existe una caja con el nombre \"{request.Name.Trim()}\"";

                return messageResponse;
            }
            
            var cashRegister = mapper.Map<CashRegister>(request);

            cashRegister.CreationByUserId = userSession.UserId;
            cashRegister.LastModificationByUserId = userSession.UserId;
            cashRegister.StoreId = userSession.StoreId;
            cashRegister.IsActive = true;

            await cashRegisterRepository.AddAsync(cashRegister);
            await session.CommitAsync();
            
            messageResponse.Success = true;
            messageResponse.Message = $"Caja agregada correctamente";
            messageResponse.Data = mapper.Map<CashRegisterResponse>(cashRegister);
        }
        catch
        {
            await session.RollbackAsync();
            messageResponse.Success = false;
            messageResponse.Message = "No se realizó ningún cambio";
        }
        
        return messageResponse;
    }

    public async Task<MessageResponse<CashRegisterResponse>> UpdateAsync(int id, CashRegisterRequest request, UserSession userSession)
    {
        var messageResponse = new MessageResponse<CashRegisterResponse>();
        var session = await repositorySessionFactory.CreateAsync();

        try
        {
            var cashRegister = await cashRegisterRepository.GetByIdAsync(id);
            if (cashRegister != null)
            {
                var exists = await cashRegisterRepository
                    .ExistsAsync(c => c.Id != cashRegister.Id 
                                      && c.Name.ToUpper() == request.Name.ToUpper() 
                                      && c.StoreId == userSession.StoreId && c.IsActive);

                if (exists)
                {
                    messageResponse.Success = false;
                    messageResponse.Message = $"Ya existe una caja con el nombre \"{cashRegister.Name}\"";
                    return messageResponse;
                }

                cashRegister.Name = request.Name.Trim();
                cashRegister.LastModificationByUserId = userSession.UserId;

                cashRegisterRepository.Update(cashRegister);
                await session.CommitAsync();
                
                messageResponse.Success = true;
                messageResponse.Message = $"Caja modificada correctamente";
                messageResponse.Data = mapper.Map<CashRegisterResponse>(cashRegister);
            }
            else
            {
                messageResponse.Success = false;
                messageResponse.Message = "Caja no encontrada";
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
            var cashRegister = await cashRegisterRepository.GetByIdAsync(id);

            if (cashRegister != null)
            {
                cashRegister.IsActive = false;
                cashRegister.LastModificationByUserId = userId;

                cashRegisterRepository.Update(cashRegister);
                await session.CommitAsync();

                messageResponse.Success = true;
                messageResponse.Data = $"Caja eliminada correctamente";
            }
            else
            {
                messageResponse.Success = false;
                messageResponse.Message = "Caja no encontrada";
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