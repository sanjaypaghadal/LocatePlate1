using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocatePlate.Model.RestaurantDomain
{
    public class UserIdentity : IdentityUser
    {
        //
        // Summary:
        //     Gets or sets a telephone number for the user.
        [ProtectedPersonalData]
        public override string PhoneNumber { get; set; }
        //
        // Summary:
        //     A random value that must change whenever a user is persisted to the store
        [MaxLength(300)]
        [Column(TypeName = "VARCHAR")]
        public override string ConcurrencyStamp { get; set; }
        //
        // Summary:
        //     A random value that must change whenever a users credentials change (password
        //     changed, login removed)
        [MaxLength(300)]
        [Column(TypeName = "VARCHAR")]
        public override string SecurityStamp { get; set; }
        //
        // Summary:
        //     Gets or sets a salted and hashed representation of the password for this user.
        [MaxLength(300)]
        [Column(TypeName = "VARCHAR")]
        public override string PasswordHash { get; set; }
        //
        // Summary:
        //     Gets or sets a flag indicating if a user has confirmed their email address.
        //
        // Value:
        //     True if the email address has been confirmed, otherwise false.
        [PersonalData]
        public override bool EmailConfirmed { get; set; }
        //
        // Summary:
        //     Gets or sets the normalized email address for this user.
        [MaxLength(150)]
        [Column(TypeName = "VARCHAR")]
        public override string NormalizedEmail { get; set; }
        //
        // Summary:
        //     Gets or sets the email address for this user.
        [ProtectedPersonalData]
        [MaxLength(100)]
        [Column(TypeName = "VARCHAR")]
        public override string Email { get; set; }
        //
        // Summary:
        //     Gets or sets the normalized user name for this user.
        [MaxLength(100)]
        [Column(TypeName = "VARCHAR")]
        public override string NormalizedUserName { get; set; }
        //
        // Summary:
        //     Gets or sets the user name for this user.
        [ProtectedPersonalData]
        public override string UserName { get; set; }
        [MaxLength(100)]
        [Column(TypeName = "VARCHAR")]
        public string BusinessAddress { get; set; }
        [MaxLength(200)]
        [Column(TypeName = "VARCHAR")]
        public string Signature { get; set; }
        [MaxLength(20)]
        [Column(TypeName = "VARCHAR")]
        public string Role { get; set; }
        public bool? IsAndroid { get; set; }
        [MaxLength(200)]
        [Column(TypeName = "VARCHAR")]
        public string DeviceId { get; set; }
        [MaxLength(200)]
        [Column(TypeName = "VARCHAR")]
        public string Locality { get; set; }
        [MaxLength(30)]
        [Column(TypeName = "VARCHAR")]
        public string PinCode { get; set; }
        [Required]
        [Display(Name = "Business Name")]
        [MaxLength(100)]
        [Column(TypeName = "VARCHAR")]
        public string BusinessName { get; set; }
    }
}
