using System;
using System.ComponentModel;
using DiaB.Common.Enums;

namespace DiaB.Middle.Dtos.AccountDtos
{
    public partial class AccountDtos
    {
        public partial class AppUpdate
        {
            [DisplayName("Mã định danh tài khoản")]
            public Guid? Id { get; set; }

            [DisplayName("Mã số bệnh nhân")]
            public string Code { get; set; }

            [DisplayName("Họ và tên")]
            public string FullName { get; set; }

            [DisplayName("Ngày sinh")]
            public DateTime? DateOfBirth { get; set; }

            [DisplayName("Giới tính")]
            public GenderTypes? Gender { get; set; }

            [DisplayName("quốc gia")]
            public Guid? NationId { get; set; }

            [DisplayName("tỉnh/thành phố")]
            public Guid? ProvinceId { get; set; }

            [DisplayName("quận/huyện")]
            public Guid? DistrictId { get; set; }

            [DisplayName("phường/xã/thị trấn")]
            public Guid? WardId { get; set; }

            [DisplayName("số nhà, đường")]
            public string Address { get; set; }

            [DisplayName("email")]
            public string Email { get; set; }

            [DisplayName("Active/Inactive")]
            public bool? Active { get; set; }

            [DisplayName("mã định danh avatar")]
            public Guid? CoverId { get; set; }

            [DisplayName("số điện thoại")]
            public string PhoneNumber { get; set; }

            [DisplayName("số điện thoại 2")]
            public string SecondPhoneNumber { get; set; }
        }
    }
}
