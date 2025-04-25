using RiPOS.Domain.Entities;

namespace RiPOS.Repository.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<IEnumerable<Role>> GetStoreRolesAsync(int userId, int storeId);
}