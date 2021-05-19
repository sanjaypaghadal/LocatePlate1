using LocatePlate.Model.RestaurantDomain;
using System;

namespace LocatePlate.Repository.Sql.Repositories.Users
{
    public interface IUserRepository
    {
        UserIdentity GetUserById(Guid Id);
    }
}
