using System.ComponentModel;
using CpTech.Core.Middle.Dtos;

namespace DiaB.Middle.Dtos.NationDtos
{
    public partial class NationDtos
    {
        public partial class AppFilter : PagingFilterDto
        {
            [DisplayName("chuỗi tra cứu")]
            public string SearchTerm { get; set; }
        }
    }
}
