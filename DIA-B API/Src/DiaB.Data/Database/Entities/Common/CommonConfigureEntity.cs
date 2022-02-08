using System.ComponentModel.DataAnnotations;
using CpTech.Core.Data.Entities;
using DiaB.Common.Enums;

namespace DiaB.Data.Database.Entities.Common
{
    public class CommonConfigureEntity : ModelEntity<CommonConfigureEntity>
    {
        [Required]
        public virtual string Key { get; set; }

        [Required]
        public virtual string Value { get; set; }

        public virtual string Name { get; set; }

        public virtual string ShortDescription { get; set; }

        public virtual string Description { get; set; }

        public virtual ConfigureSetupTypes? ConfigureSetupType { get; set; }
    }
}
