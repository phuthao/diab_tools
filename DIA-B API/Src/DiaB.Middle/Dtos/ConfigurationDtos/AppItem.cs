using System;
using System.ComponentModel;

namespace DiaB.Middle.Dtos.ConfigurationDtos
{
    public partial class ConfigurationDtos
    {
        public partial class AppItem
        {
            [Description("mã định danh")]
            public Guid Id { get; set; }

            [Description("Group")]
            public string Group { get; set; }

            [Description("Mã")]
            public string Key { get; set; }

            [Description("Giá trị")]
            public string Value { get; set; }

            [Description("Tên")]
            public string Name { get; set; }

            [Description("Ghi chú ngắn")]
            public string ShortDescription { get; set; }

            [Description("Ghi chú")]
            public string Description { get; set; }

            [Description("Cập nhật lần cuối")]
            public DateTime UpdateDateTime { get; set; }

            [Description("Người cập nhật lần cuối")]
            public string Updater { get; set; }
        }
    }
}
