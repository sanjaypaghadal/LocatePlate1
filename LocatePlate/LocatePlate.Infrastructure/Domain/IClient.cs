using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LocatePlate.Infrastructure.Domain
{
    public interface IClientSide
    {
        DateTime ClientTime { get; set; }
        TimeZoneInfo ClientInfo { get; set; }
        ICollection<string> Cookies { get; set; }
    }
    public class ClientSide : IClientSide
    {
        public DateTime ClientTime { get; set; }
        [JsonIgnore]
        public TimeZoneInfo ClientInfo { get; set; }
        [JsonIgnore]
        public ICollection<string> Cookies { get; set; }
    }
}
