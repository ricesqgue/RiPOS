using RiPOS.Database;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;

namespace RiPOS.Repository.Repositories;

public class CustomerRepository(RiPosDbContext dbContext)
    : GenericRepository<Customer>(dbContext), ICustomerRepository;