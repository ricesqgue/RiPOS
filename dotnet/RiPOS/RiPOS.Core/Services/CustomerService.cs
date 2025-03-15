using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RiPOS.Core.Interfaces;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;
using RiPOS.Shared.Models;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<CustomerResponse>> GetAllAsync(int companyId, bool includeInactives = false)
        {
            var customers = await _customerRepository.GetAllAsync(c => c.CompanyId == companyId && (c.IsActive || includeInactives),
                includeProps: c => c.Include(x => x.CountryState));

            var customersReponse = _mapper.Map<ICollection<CustomerResponse>>(customers);
            return customersReponse;
        }

        public async Task<CustomerResponse> GetByIdAsync(int id, int companyId)
        {
            var customer = await _customerRepository.FindAsync(c => c.Id == id && c.CompanyId == companyId,
                includeProps: c => c.Include(x => x.CountryState));

            var customerResponse = _mapper.Map<CustomerResponse>(customer);
            return customerResponse;
        }

        public async Task<bool> ExistsByIdAsync(int id, int companyId)
        {
            return await _customerRepository.ExistsAsync(c => c.Id == id && c.CompanyId == companyId && c.IsActive);
        }

        public async Task<MessageResponse<CustomerResponse>> AddAsync(CustomerRequest request, UserSession userSession)
        {
            var messageResponse = new MessageResponse<CustomerResponse>();

            var customer = _mapper.Map<Customer>(request);

            customer.CreationByUserId = userSession.UserId;
            customer.LastModificationByUserId = userSession.UserId;
            customer.CompanyId = userSession.CompanyId;
            customer.IsActive = true;

            var exists = await _customerRepository
                .FindAsync(c => (customer.Email != null && c.Email.ToUpper() == customer.Email.ToUpper() || customer.Rfc != null && c.Rfc == customer.Rfc)
                    && c.IsActive && c.CompanyId == userSession.CompanyId);

            if (exists != null)
            {
                messageResponse.Success = false;
                if (exists.Email.ToUpper() == request.Email.Trim().ToUpper())
                {
                    messageResponse.Message = $"Ya existe un cliente con el email \"{customer.Email}\"";
                }
                else
                {
                    messageResponse.Message = $"Ya existe un cliente con el RFC \"{customer.Rfc.ToUpper()}\"";
                }
                return messageResponse;
            }

            messageResponse.Success = await _customerRepository.AddAsync(customer);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Cliente agregado correctamente";
                messageResponse.Data = _mapper.Map<CustomerResponse>(customer);
            }
            else
            {
                messageResponse.Message = "No se realizó ningún cambio";
            }

            return messageResponse;
        }

        public async Task<MessageResponse<CustomerResponse>> UpdateAsync(int id, CustomerRequest request, UserSession userSession)
        {
            var messageResponse = new MessageResponse<CustomerResponse>();

            var customer = await _customerRepository.GetByIdAsync(id);

            var exists = await _customerRepository.FindAsync(c => c.Id != customer.Id && (customer.Email != null && c.Email.ToUpper() == customer.Email.ToUpper() || customer.Rfc != null && c.Rfc == customer.Rfc)
                && c.CompanyId == userSession.CompanyId && c.IsActive);

            if (exists != null)
            {
                messageResponse.Success = false;
                if (exists.Name.ToUpper() == request.Name.Trim().ToUpper())
                {
                    messageResponse.Message = $"Ya existe un cliente con el email \"{customer.Email}\"";
                }
                else
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

            customer.LastModificationByUserId = userSession.UserId;

            messageResponse.Success = await _customerRepository.UpdateAsync(customer);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Cliente modificado correctamente";
                messageResponse.Data = _mapper.Map<CustomerResponse>(customer);
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

            var customer = await _customerRepository.GetByIdAsync(id);

            customer.IsActive = false;
            customer.LastModificationByUserId = userSession.UserId;

            messageResponse.Success = await _customerRepository.UpdateAsync(customer);

            if (messageResponse.Success)
            {
                messageResponse.Success = true;
                messageResponse.Message = $"Cliente eliminado correctamente";
            }
            else
            {
                messageResponse.Message = "No se realizó ningún cambio";
            }

            return messageResponse;
        }
    }
}
