﻿using AutoMapper;
using RiPOS.Core.Interfaces;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;
using RiPOS.Repository.Session;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.Services;

public class StoreService(IRepositorySessionFactory repositorySessionFactory, IStoreRepository storeRepository, IMapper mapper) : IStoreService
{
    public async Task<ICollection<StoreResponse>> GetAllAsync()
    {
        var stores = await storeRepository.GetAllAsync();

        var storesResponse = mapper.Map<ICollection<StoreResponse>>(stores);

        return storesResponse;
    }

    public async Task<StoreResponse?> GetByIdAsync(int id)
    {
        var store = await storeRepository.FindAsync(s => s.Id == id);

        var storeResponse = mapper.Map<StoreResponse>(store);

        return storeResponse;
    }

    public async Task<bool> ExistsByIdAsync(int id)
    {
        return await storeRepository.ExistsAsync(s => s.Id == id && s.IsActive);
    }

    public async Task<MessageResponse<StoreResponse>> AddAsync(StoreRequest request, int userId)
    {
        var messageResponse = new MessageResponse<StoreResponse>();
        var session = await repositorySessionFactory.CreateAsync();

        try
        {
            var store = mapper.Map<Store>(request);

            store.CreationByUserId = userId;
            store.LastModificationByUserId = userId;
            store.IsActive = true;

            var exists = await storeRepository.ExistsAsync(s => s.Name.ToUpper() == request.Name.Trim().ToUpper() && s.IsActive);

            if (exists)
            {
                messageResponse.Success = false;
                messageResponse.Message = $"Ya existe una tienda con el nombre \"{store.Name}\"";
                return messageResponse;
            }

            await storeRepository.AddAsync(store);
            await session.CommitAsync();
            
            messageResponse.Success = true;
            messageResponse.Message = $"Tienda agregada correctamente";
            messageResponse.Data = mapper.Map<StoreResponse>(store);
        }
        catch
        {
            await session.RollbackAsync();
            messageResponse.Success = false;
            messageResponse.Message = "No se realizó ningún cambio";
        }
        
        return messageResponse;
    }

    public async Task<MessageResponse<StoreResponse>> UpdateAsync(int id, StoreRequest request, int userId)
    {
        var messageResponse = new MessageResponse<StoreResponse>();
        var session = await repositorySessionFactory.CreateAsync();

        try
        {
            var store = await storeRepository.GetByIdAsync(id);

            if (store != null)
            {
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
                store.LastModificationByUserId = userId;

                storeRepository.Update(store);
                await session.CommitAsync();
                
                messageResponse.Success = true;
                messageResponse.Message = $"Tienda modificada correctamente";
                messageResponse.Data = mapper.Map<StoreResponse>(store);
            }
            else
            {
                messageResponse.Success = false;
                messageResponse.Message = "Tienda no encontrada";
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
            var store = await storeRepository.GetByIdAsync(id);

            if (store != null)
            {
                store.IsActive = false;
                store.LastModificationByUserId = userId;

                storeRepository.Update(store);
                await session.CommitAsync();
                
                messageResponse.Success = true;
                messageResponse.Data = $"Tienda eliminada correctamente";
            }
            else
            {
                messageResponse.Success = false;
                messageResponse.Message = "Tienda no encontrada";
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