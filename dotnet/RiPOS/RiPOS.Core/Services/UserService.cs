using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RiPOS.Core.Interfaces;
using RiPOS.Repository.Interfaces;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<UserResponse>> GetAllByCompanyAsync(int companyId, bool includeInactives = false)
        {
            var users = await _userRepository.GetAllAsync(u => u.CompanyId == companyId && (u.IsActive || includeInactives));
            var usersResponse = _mapper.Map<ICollection<UserResponse>>(users);

            return usersResponse;
        }
        public async Task<ICollection<UserResponse>> GetAllByStoreAsync(int storeId, bool includeInactives = false)
        {
            var users = await _userRepository.GetAllAsync(u => u.UserStoreRoles.Any(usr => usr.StoreId == storeId) && (u.IsActive || includeInactives),
                includeProps: u => u.Include(u => u.UserStoreRoles));

            var usersResponse = _mapper.Map<ICollection<UserResponse>>(users);

            return usersResponse;
        }

        public async Task<UserResponse> GetByIdInCompanyAsync(int id, int companyId)
        {
            var user = await _userRepository.FindAsync(u => u.Id == id && u.CompanyId == companyId);

            var userResponse = _mapper.Map<UserResponse>(user);
            return userResponse;
        }
        public async Task<UserResponse> GetByIdInStoreAsync(int id, int storeId)
        {
            var user = await _userRepository.FindAsync(u => u.UserStoreRoles.Any(usr => usr.StoreId == storeId),
                includeProps: u => u.Include(u => u.UserStoreRoles));

            var userResponse = _mapper.Map<UserResponse>(user);

            return userResponse;
        }

        public async Task<bool> ExistsByIdInCompanyAsync(int id, int companyId)
        {
            return await _userRepository.ExistsAsync(u => u.Id == id && u.CompanyId == companyId);
        }

        public async Task<bool> ExistsByIdInStoreAsync(int id, int storeId)
        {
            return await _userRepository.ExistsAsync(u => u.UserStoreRoles.Any(usr => usr.StoreId == storeId));
        }
    }
}
