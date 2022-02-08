using System.ComponentModel;
using CpTech.Core.Common.Attributes;

namespace DiaB.Middle.Dtos.BaseDtos
{
    public class CoreFilterDto : CpTech.Core.Middle.Dtos.CoreFilterDto
    {
        public override bool? IsDeleted { get; set; } = false;

        [DisplayName("kích thước trang")]
        [RangeValidate(1, double.MaxValue)]
        public override int Size { get; set; } = 10;
    }
}
