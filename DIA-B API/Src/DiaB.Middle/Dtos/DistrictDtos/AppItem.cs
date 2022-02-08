using System;
using System.ComponentModel;
using CpTech.Core.Middle.Dtos;

namespace DiaB.Middle.Dtos.DistrictDtos
{
    public partial class DistrictDtos
    {
        public partial class AppItem : BaseItemDto
        {
            [DisplayName("mã định danh quận/huyện")]
            public Guid Id { get; set; }

            [DisplayName("tên quận/huyện")]
            public string Name { get; set; }
        }
    }
}
