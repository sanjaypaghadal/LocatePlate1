using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LocatePlate.Infrastructure.Geography
{
    public enum StateProvince
    {
        [Display(Name = "Select Province")]
        Unspecified,

        [Category("Canada")]
        [Display(Name = "Alberta")]
        AB,

        [Category("Canada")]
        [Display(Name = "British Columbia")]
        BC,

        [Category("Canada")]
        [Display(Name = "Manitoba")]
        MB,

        [Category("Canada")]
        [Display(Name = "New Brunswick")]
        NB,

        [Category("Canada")]
        [Display(Name = "Newfoundland and Labrador")]
        NL,

        [Category("Canada")]
        [Display(Name = "Northwest Territories")]
        NT,

        [Category("Canada")]
        [Display(Name = "Nova Scotia")]
        NS,

        [Category("Canada")]
        [Display(Name = "Nunavut")]
        NU,

        [Category("Canada")]
        [Display(Name = "Ontario")]
        ON,

        [Category("Canada")]
        [Display(Name = "Prince Edward Island")]
        PE,

        [Category("Canada")]
        [Display(Name = "Quebec")]
        QC,

        [Category("Canada")]
        [Display(Name = "Saskatchewan")]
        SK,

        [Category("Canada")]
        [Display(Name = "Yukon")]
        YT
    }

}
