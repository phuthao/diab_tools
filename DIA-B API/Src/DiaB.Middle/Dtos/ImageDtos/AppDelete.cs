using System;
using System.Collections.Generic;
using System.ComponentModel;
using DiaB.Common.Enums;

namespace DiaB.Middle.Dtos.ImageDtos
{
    public partial class ImageDtos
    {
      public partial class AppDelete
      {
          [DisplayName("danh sách mã định danh hình bị xóa")]
          public IList<Guid> RemovalImageIds { get; set; }

          [DisplayName("mã định danh đầu vào")]
          public Guid InputId { get; set; }

          [DisplayName("loại hình ảnh")]
          public ImageTypes Type { get; set; }
      }
    }
}
