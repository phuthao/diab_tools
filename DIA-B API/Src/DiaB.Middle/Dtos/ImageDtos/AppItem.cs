using System;
using System.ComponentModel;
using System.IO;

namespace DiaB.Middle.Dtos.ImageDtos
{
    public partial class ImageDtos
    {
        public partial class AppItem
        {
            [DisplayName("mã định danh")]
            public Guid Id { get; set; }

            [DisplayName("tên hình ảnh")]
            public string Title { get; set; }

            [DisplayName("dung lượng")]
            public long Size { get; set; }

            [DisplayName("Checksum")]
            public string Checksum { get; set; }

            [DisplayName("đường dẫn")]
            public string Url { get; set; }

            [DisplayName("nội dung hình ảnh")]
            public Stream Image { get; set; }
        }
    }
}
