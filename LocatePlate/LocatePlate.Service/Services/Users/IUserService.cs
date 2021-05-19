using LocatePlate.Model.RestaurantDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocatePlate.Service.Services.Users
{
   public interface IUserService
    {
        UserIdentity GetUserById(Guid Id);
    }
}
