using DiaB.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using static DiaB.Middle.Dtos.ImageDtos.ImageDtos;

namespace DiaB.Middle.Dtos.AccountDtos
{
    public partial class AccountDtos
    {
        public partial class AppItem
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

            [DisplayName("avatar")]
            public Guid? CoverId { get; set; }

            public string Code { get; set; }

            public string GoogleUserId { get; set; }

            public string Email { get; set; }

            public string DateOfBirth { get; set; }

            public string Address { get; set; }

            public IEnumerable<string> Roles { get; set; }

            public AppUrl Avatar { get; set; }
        }
    }
}
