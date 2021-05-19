using LocatePlate.Infrastructure.Domain;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocatePlate.Model.RestaurantDomain
{
    public class Timing : BaseEntity
    {
        [NotMapped]
        public bool IsOpen { get; set; }
        public DayOfWeek Day { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime CloseTime { get; set; }
        public override int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
        [NotMapped]
        public TimeZoneInfo? ClientInfo { get; set; }
        [NotMapped]
        public string StartTimeLocal
        {
            get
            {
                return ToLocalTime(StartTime);
            }
        }

        [NotMapped]
        public string CloseTimeLocal
        {
            get
            {
                return ToLocalTime(CloseTime);
            }
        }

        private string ToLocalTime(DateTime date)
        {
            if (ClientInfo != null)
                return TimeZoneInfo.ConvertTimeFromUtc(date, ClientInfo).ToString("hh:mm tt");
            else
                return string.Empty;
        }

    }

    public enum Days { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday }
}
