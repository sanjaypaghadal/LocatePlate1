using LocatePlate.Model.Cms.Modules.Abstract;
using System;
using System.Collections.Generic;

namespace LocatePlate.Model.Cms.Modules.SearchHeader
{
    public class SearchButtonsModule : BaseModule, IModuleEntity
    {
        public static readonly Guid _id = Guid.Parse("f60ab95c-797a-4b9e-8e06-deda3a82ef45");
        public Guid Id { get => _id; }
        public IEnumerable<Button> Buttons { get; set; }
    }

    public class Button
    {
        public string Name { get; set; }
        public string Keyword { get; set; }
        public UInt16 Order { get; set; }
    }
}
