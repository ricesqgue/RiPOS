using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RiPOS.Core.Interfaces;
using RiPOS.Repository.Interfaces;
using RiPOS.Shared.Enums;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.Services
{
    public class UserService(IUserRepository userRepository, IMapper mapper) : IUserService
    {
        public async Task<ICollection<UserResponse>> GetAllAsync(bool includeInactives = false)
        {
            var users = await userRepository.GetAllAsync(u => u.IsActive || includeInactives);
            var usersResponse = mapper.Map<ICollection<UserResponse>>(users);

            return usersResponse;
        }
        
        public async Task<ICollection<UserResponse>> GetAllByStoreAsync(int storeId, bool includeInactives = false)
        {
            var users = await userRepository
                .GetAllAsync(u => u.UserStoreRoles.Any(usr => usr.StoreId == storeId) && (u.IsActive || includeInactives),
                    includeProps: u => u.Include(ur => ur.UserStoreRoles));

            var usersResponse = mapper.Map<ICollection<UserResponse>>(users);

            return usersResponse;
        }   
        
        public async Task<UserResponse> GetByIdInStoreAsync(int id, int storeId)
        {
            var user = await userRepository.FindAsync(u => u.UserStoreRoles.Any(usr => usr.StoreId == storeId),
                includeProps: u => u.Include(ur => ur.UserStoreRoles));

            var userResponse = mapper.Map<UserResponse>(user);

            return userResponse;
        }
        
        public async Task<bool> ExistsByIdInStoreAsync(int id, int storeId)
        {
            return await userRepository.ExistsAsync(u => u.UserStoreRoles.Any(usr => usr.StoreId == storeId));
        }

        public async Task<ICollection<RoleEnum>> GetUserRolesByStoreIdAsync(int userId, int storeId)
        {
            var roles = await userRepository.GetStoreRolesAsync(userId, storeId);
            var rolesEnum = roles.Select(r => (RoleEnum)r.Id).ToList();

            return rolesEnum;
        }
    }
}
