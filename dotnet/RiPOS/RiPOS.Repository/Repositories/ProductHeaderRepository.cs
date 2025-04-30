using RiPOS.Database;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;

namespace RiPOS.Repository.Repositories;

public class ProductHeaderRepository(RiPosDbContext dbContext) : GenericRepository<ProductHeader>(dbContext), IProductHeaderRepository;