using System.ComponentModel;
using DiaB.Common.Enums;

namespace DiaB.Middle.Dtos.DeviceDtos
{
    public partial class DeviceDtos
    {
        public partial class AppInsert
        {
            [DisplayName("loại thiết bị")]
            public DeviceTypes DeviceType { get; set; }

            [DisplayName("thông tin thiết bị")]
            public string DeviceInformation { get; set; }

            [DisplayName("mã thông báo firebase")]
            public string FirebaseToken { get; set; }
        }
    }
}
