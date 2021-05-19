using LocatePlate.Model.Identity;

namespace LocatePlate.Model.Api
{
    public class AccountResponseModel
    {
        public User User { get; set; }
        public string Message{ get; set; }
        public bool IsStatus { get; set; }
    }
}
