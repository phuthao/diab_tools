using System;
using System.ComponentModel;
using CpTech.Core.Middle.Dtos;

namespace DiaB.Middle.Dtos.DistrictDtos
{
    public partial class DistrictDtos
    {
        public partial class AppFilter : PagingFilterDto
        {
            [DisplayName("chuỗi tra cứu")]
            public string SearchTerm { get; set; }

            [DisplayName("mã định danh tỉnh/thành phố")]
            public Guid? ProvinceId { get; set; }
        }
    }
}
