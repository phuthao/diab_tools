using System;
using System.ComponentModel;
using CpTech.Core.Middle.Dtos;

namespace DiaB.Middle.Dtos.WardDtos
{
    public partial class WardDtos
    {
        public partial class AppItem : BaseItemDto
        {
            [DisplayName("mã định danh phường/xã")]
            public Guid Id { get; set; }

            [DisplayName("tên phường/xã")]
            public string Name { get; set; }
        }
    }
}
