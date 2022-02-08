using System.ComponentModel;

namespace DiaB.Middle.Dtos.ConfigurationDtos
{
    public partial class ConfigurationDtos
    {
        public partial class AppUpdate
        {
            [Description("Key")]
            public string Key { get; set; }

            [Description("Giá trị")]
            public string Value { get; set; }

            [Description("Tên")]
            public string Name { get; set; }

            [Description("Ghi chú ngắn")]
            public string ShortDescription { get; set; }

            [Description("Ghi chú")]
            public string Description { get; set; }
        }
    }
}
