using DiaB.Common.Enums;
using System;
using System.ComponentModel;
using static DiaB.Middle.Dtos.ImageDtos.ImageDtos;

namespace DiaB.Middle.Dtos.AccountDtos
{
    public partial class AccountDtos
    {
        public partial class AppItemPortal
        {
            [DisplayName("mã định danh tài khoản")]
            public Guid Id { get; set; }

            [DisplayName("tên tài khoản")]
            public string Username { get; set; }

            [DisplayName("họ và tên")]
            public string FullName { get; set; }

            [DisplayName("độ tuổi")]
            public int? Age { get; set; }

            [DisplayName("số điện thoại")]
            public string PhoneNumber { get; set; }

            public string SecondPhoneNumber { get; set; }

            [DisplayName("giới tính")]
            public string Gender { get; set; }

            [DisplayName("thời gian tạo")]
            public DateTime CreateDatetime { get; set; }

            [DisplayName("trạng thái")]
            public bool IsActive { get; set; }

            [DisplayName("quốc gia")]
            public string Nation { get; set; }

            [DisplayName("tỉnh/thành phố")]
            public string Province { get; set; }

            [DisplayName("quận/huyện")]
            public string District { get; set; }

            [DisplayName("phường/xã/thị trấn")]
            public string Ward { get; set; }

            [DisplayName("quốc gia")]
            public Guid? NationId { get; set; }

            [DisplayName("tỉnh/thành phố")]
            public Guid? ProvinceId { get; set; }

            [DisplayName("quận/huyện")]
            public Guid? DistrictId { get; set; }

            [DisplayName("phường/xã/thị trấn")]
            public Guid? WardId { get; set; }

            [DisplayName("avatar")]
            public AppUrl Avatar { get; set; }

            public Guid? CoverId { get; set; }

            public DateTime UpdateDatetime { get; set; }

            public Guid? CreatorId { get; set; }

            public Guid? UpdaterId { get; set; }

            public string Email { get; set; }

            public DateTime? DateOfBirth { get; set; }

            public string Address { get; set; }
            public bool ChangePassword { get; set; }
            public Editor Creator { get; set; }
            public Editor Updater { get; set; }

            public AccountTypes? AccountType { get; set; }
        }

        public class Editor
        {
            public string Username { get; set; }
            public string FullName { get; set; }
            public AppUrl Avatar { get; set; }
        }
    }
}
