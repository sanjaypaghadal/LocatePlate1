using System;

namespace LocatePlate.Model.Cms.Modules.Abstract
{
    public interface IModuleEntity
    {
        string Name { get; set; }
        Guid Id { get; }
    }
}
