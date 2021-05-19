using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Repository.Context;
using System;
using System.Linq;

namespace LocatePlate.Repository.Sql.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly LocatePlateContext _locatePlateContext;
        public UserRepository(LocatePlateContext locatePlateContext)
        {
            this._locatePlateContext = locatePlateContext;
        }

        public UserIdentity GetUserById(Guid Id) => this._locatePlateContext.UserIdentities.FirstOrDefault(c => c.Id == Id.ToString());
    }
}
