using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Repository.Sql.Repositories.Users;
using System;

namespace LocatePlate.Service.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserIdentity GetUserById(Guid Id) => this._userRepository.GetUserById(Id);
    }
}
