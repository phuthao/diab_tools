using System;
using System.ComponentModel;

namespace DiaB.Middle.Dtos.ImageDtos
{
    public partial class ImageDtos
    {
        public class AppUrl
        {
            [DisplayName("mã định danh")]
            public Guid Id { get; set; }

            [DisplayName("đường dẫn")]
            public string Url { get; set; }
        }
    }
}
