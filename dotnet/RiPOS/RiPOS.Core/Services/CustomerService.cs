using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RiPOS.Core.Interfaces;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;
using RiPOS.Repository.Session;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.Services;

public class CustomerService(IRepositorySessionFactory repositorySessionFactory, ICustomerRepository customerRepository, IMapper mapper) : ICustomerService
{
    public async Task<ICollection<CustomerResponse>> GetAllAsync(bool includeInactives = false)
    {
        var customers = await customerRepository.GetAllAsync(c => c.IsActive || includeInactives,
            includeProps: c => c.Include(x => x.CountryState!));

        var customersResponse = mapper.Map<ICollection<CustomerResponse>>(customers);
        return customersResponse;
    }

    public async Task<CustomerResponse?> GetByIdAsync(int id)
    {
        var customer = await customerRepository.FindAsync(c => c.Id == id,
            includeProps: c => c.Include(x => x.CountryState!));

        var customerResponse = mapper.Map<CustomerResponse>(customer);
        return customerResponse;
    }

    public async Task<bool> ExistsByIdAsync(int id)
    {
        return await customerRepository.ExistsAsync(c => c.Id == id && c.IsActive);
    }

    public async Task<MessageResponse<CustomerResponse>> AddAsync(CustomerRequest request, int userId)
    {
        var messageResponse = new MessageResponse<CustomerResponse>();
        var session = await repositorySessionFactory.CreateAsync();

        try
        {
            var customer = mapper.Map<Customer>(request);

            customer.CreationByUserId = userId;
            customer.LastModificationByUserId = userId;
            customer.IsActive = true;

            var exists = await customerRepository
                .FindAsync(c => (customer.Email != null && c.Email != null && c.Email.ToUpper() == customer.Email.ToUpper() 
                                 || (customer.Rfc != null && c.Rfc != null && customer.Rfc != null && c.Rfc == customer.Rfc))
                                && c.IsActive);

            if (exists != null)
            {
                messageResponse.Success = false;
                if (exists.Email != null && exists.Email.ToUpper() == request.Email?.Trim().ToUpper())
                {
                    messageResponse.Message = $"Ya existe un cliente con el email \"{customer.Email}\"";
                }
                else if (customer.Rfc != null)
                {
                    messageResponse.Message = $"Ya existe un cliente con el RFC \"{customer.Rfc.ToUpper()}\"";
                }
                return messageResponse;
            }

            await customerRepository.AddAsync(customer);
            await session.CommitAsync();
            
            messageResponse.Success = true;
            messageResponse.Message = $"Cliente agregado correctamente";
            messageResponse.Data = mapper.Map<CustomerResponse>(customer);
        }
        catch
        {
            await session.RollbackAsync();
            messageResponse.Success = false;
            messageResponse.Message = "No se realizó ningún cambio";
        }
        
        return messageResponse;
    }

    public async Task<MessageResponse<CustomerResponse>> UpdateAsync(int id, CustomerRequest request, int userId)
    {
        var messageResponse = new MessageResponse<CustomerResponse>();
        var session = await repositorySessionFactory.CreateAsync();

        try
        {
            var customer = await customerRepository.GetByIdAsync(id);

            if (customer != null)
            {
                var exists = await customerRepository
                    .FindAsync(c => c.Id != customer.Id 
                                    && (customer.Email != null && c.Email != null && c.Email.ToUpper() == customer.Email.ToUpper() 
                                        || (customer.Rfc != null && c.Rfc != null && customer.Rfc != null && c.Rfc == customer.Rfc))
                                    && c.IsActive);

                if (exists != null)
                {
                    messageResponse.Success = false;
                    if (exists.Email != null && exists.Email.ToUpper() == request.Name.Trim().ToUpper())
                    {
                        messageResponse.Message = $"Ya existe un cliente con el email \"{customer.Email}\"";
                    }
                    else if (customer.Rfc != null)
                    {
                        messageResponse.Message = $"Ya existe un cliente con el RFC  \"{customer.Rfc.ToUpper()}\"";
                    }
                    return messageResponse;
                }

                customer.Name = request.Name.Trim();
                customer.Surname = request.Surname.Trim();
                customer.SecondSurname = request.SecondSurname?.Trim();
                customer.Email = request.Email?.Trim();
                customer.PhoneNumber = request.PhoneNumber?.Trim();
                customer.MobilePhone = request.MobilePhone?.Trim();
                customer.Address = request.Address?.Trim();
                customer.City = request.City?.Trim();
                customer.ZipCode = request.ZipCode?.Trim();
                customer.Rfc = request.Rfc?.Trim().ToUpper();
                customer.CountryStateId = request.CountryStateId;

                customer.LastModificationByUserId = userId;

                customerRepository.Update(customer);
                await session.CommitAsync();
                
                messageResponse.Success = true;
                messageResponse.Message = $"Cliente modificado correctamente";
                messageResponse.Data = mapper.Map<CustomerResponse>(customer);
            }
            else
            {
                messageResponse.Success = false;
                messageResponse.Message = "Cliente no encontrado";
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
            var customer = await customerRepository.GetByIdAsync(id);

            if (customer != null)
            {
                customer.IsActive = false;
                customer.LastModificationByUserId = userId;

                customerRepository.Update(customer);
                await session.CommitAsync();
                
                messageResponse.Success = true;
                messageResponse.Data = $"Cliente eliminado correctamente";
            }
            else
            {
                messageResponse.Success = false;
                messageResponse.Message = "Cliente no encontrado";
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