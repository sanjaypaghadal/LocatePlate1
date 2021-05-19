using LocatePlate.Model.Cms.Modules.Abstract;
using System;
using System.Collections.Generic;

namespace LocatePlate.Model.Cms.Modules.WALF
{
    public class WALFModule : BaseModule, IModuleEntity
    {
        public static readonly Guid _id = Guid.Parse("9c7584c0-2f19-4030-b71a-95f010e7d800");
        public Guid Id { get => _id; }
        public List<WALF> Walf { get; set; }
        public string Url { get; set; }
    }
}
