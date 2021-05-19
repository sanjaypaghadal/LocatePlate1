using LocatePlate.Model.Cms.Modules.Abstract;
using System;

namespace LocatePlate.Model.Cms.Modules.Advertisement
{
    public class AdvertisementModule : BaseModule, IModuleEntity
    {
        public static Guid _id = Guid.Parse("47df4dbc-06b1-4066-afb4-8633115f6e28");
        public Guid Id { get => _id; }
        public string Advert { get; set; }
    }
}
