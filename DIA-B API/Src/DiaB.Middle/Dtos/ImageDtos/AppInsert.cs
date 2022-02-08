using System;
using System.Collections.Generic;
using System.ComponentModel;
using DiaB.Common.Enums;
using Microsoft.AspNetCore.Http;

namespace DiaB.Middle.Dtos.ImageDtos
{
    public partial class ImageDtos
    {
        public partial class AppInsert
        {
            [DisplayName("các tệp hình ảnh")]
            public IList<IFormFile> images { get; set; }

            [DisplayName("loại")]
            public ImageTypes Type { get; set; }

            [DisplayName("avatar")]
            public Guid? AccountId { get; set; }
        }
    }
}
