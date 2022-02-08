using System.ComponentModel;

namespace DiaB.Middle.Dtos.BaseDtos
{
    public class PagingFilterDto : CpTech.Core.Middle.Dtos.PagingFilterDto
    {
        [DisplayName("lấy tất cả")]
        public bool TakeAll { get; set; }
    }
}
