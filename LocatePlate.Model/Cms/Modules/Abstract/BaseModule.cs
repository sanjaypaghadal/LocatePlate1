using System;

namespace LocatePlate.Model.Cms.Modules.Abstract
{
    public abstract class BaseModule
    {
        public string Name { get; set; }
        public string Markup { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public virtual Guid SectionId { get; set; }
        public virtual Guid PageId { get; set; }

    }
}
