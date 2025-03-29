﻿using RiPOS.Database;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;

namespace RiPOS.Repository.Repositories
{
    public class BrandRepository(RiPosDbContext dbContext) : GenericRepository<Brand>(dbContext), IBrandRepository;
}
