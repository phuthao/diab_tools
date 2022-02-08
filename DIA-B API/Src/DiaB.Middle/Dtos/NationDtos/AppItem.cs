using System;
using System.ComponentModel;
using CpTech.Core.Middle.Dtos;

namespace DiaB.Middle.Dtos.NationDtos
{
    public partial class NationDtos
    {
        public partial class AppItem : BaseItemDto
        {
            [DisplayName("mã định danh quốc gia")]
            public Guid Id { get; set; }

            [DisplayName("tên quốc gia")]
            public string Name { get; set; }
        }
    }
}
