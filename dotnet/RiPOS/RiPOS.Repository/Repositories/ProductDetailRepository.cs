using RiPOS.Database;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;

namespace RiPOS.Repository.Repositories;

public class ProductDetailRepository(RiPosDbContext dbContext) : GenericRepository<ProductDetail>(dbContext), IProductDetailRepository;