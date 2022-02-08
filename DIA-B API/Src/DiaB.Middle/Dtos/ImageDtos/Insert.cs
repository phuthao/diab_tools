using System;
using System.ComponentModel;
using DiaB.Common.Enums;

namespace DiaB.Middle.Dtos.ImageDtos
{
    public partial class ImageDtos
    {
        public partial class Insert
        {
            [DisplayName("đường dẫn")]
            public string PhysicalPath { get; set; }

            [DisplayName("tên")]
            public string Title { get; set; }

            [DisplayName("loại")]
            public ImageTypes Type { get; set; }

            [DisplayName("dung lượng")]
            public long Size { get; set; }

            [DisplayName("Checksum")]
            public string Checksum { get; set; }
        }
    }
}
