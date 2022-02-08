using System;
using System.ComponentModel;
using DiaB.Common.Enums;

namespace DiaB.Middle.Dtos.DeviceDtos
{
    public partial class DeviceDtos
    {
        public partial class AppUpdate
        {
            [DisplayName("mã định danh thiết bị")]
            public Guid Id { get; set; }

            [DisplayName("loại thiết bị")]
            public DeviceTypes DeviceType { get; set; }

            [DisplayName("thông tin thiết bị")]
            public string DeviceInformation { get; set; }

            [DisplayName("mã thông báo firebase")]
            public string FirebaseToken { get; set; }
        }
    }
}
