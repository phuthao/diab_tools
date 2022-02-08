using System;
using System.ComponentModel;
using CpTech.Core.Middle.Dtos;

namespace DiaB.Middle.Dtos.ProvinceDtos
{
    public partial class ProvinceDtos
    {
        public partial class AppFilter : PagingFilterDto
        {
            [DisplayName("chuỗi tra cứu")]
            public string SearchTerm { get; set; }

            [DisplayName("mã định danh quốc gia")]
            public Guid? NationId { get; set; }
        }
    }
}
