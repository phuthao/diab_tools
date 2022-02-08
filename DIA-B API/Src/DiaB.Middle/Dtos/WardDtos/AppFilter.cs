using System;
using System.ComponentModel;
using CpTech.Core.Middle.Dtos;

namespace DiaB.Middle.Dtos.WardDtos
{
    public partial class WardDtos
    {
        public partial class AppFilter : PagingFilterDto
        {
            [DisplayName("chuỗi tra cứu")]
            public string SearchTerm { get; set; }

            [DisplayName("mã định danh quận/huyện")]
            public Guid? DistrictId { get; set; }
        }
    }
}
