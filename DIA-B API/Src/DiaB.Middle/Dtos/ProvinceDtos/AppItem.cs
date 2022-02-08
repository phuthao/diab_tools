using System;
using System.ComponentModel;
using CpTech.Core.Middle.Dtos;

namespace DiaB.Middle.Dtos.ProvinceDtos
{
    public partial class ProvinceDtos
    {
        public partial class AppItem : BaseItemDto
        {
            [DisplayName("mã định danh tỉnh/thành phố")]
            public Guid Id { get; set; }

            [DisplayName("tên tỉnh/thành phố")]
            public string Name { get; set; }
        }
    }
}
