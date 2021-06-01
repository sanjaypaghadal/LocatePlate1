using System;

namespace LocatePlate.Infrastructure.Extentions
{
    public static class doubleHelpers
    {
        public static double ComputeDiscountedPrice(this double originalPrice, double percentDiscount, out double discountedPrice)
        {
            // enforce preconditions
            if (originalPrice < 0) throw new ArgumentOutOfRangeException("originalPrice", "a price can't be negative!");
            if (percentDiscount < 0) throw new ArgumentOutOfRangeException("percentDiscount", "a discount can't be negative!");
            if (percentDiscount > 100) throw new ArgumentOutOfRangeException("percentDiscount", "a discount can't exceed 100%");

            discountedPrice = Math.Round(originalPrice * (percentDiscount / 100), 2, MidpointRounding.ToEven);
            double grandTotalAfterDiscount = originalPrice - discountedPrice;

            return grandTotalAfterDiscount;
        }
    }
}
