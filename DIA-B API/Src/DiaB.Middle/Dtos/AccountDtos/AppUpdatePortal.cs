﻿using System;
using System.ComponentModel;
using DiaB.Common.Enums;
using Microsoft.AspNetCore.Http;

namespace DiaB.Middle.Dtos.AccountDtos
{
    public partial class AccountDtos
    {
        public partial class AppUpdatePortal
        {
            public Guid Id { get; set; }

            [DisplayName("họ và tên")]
            public string FullName { get; set; }

            [DisplayName("số điện thoại")]
            public string PhoneNumber { get; set; }

            public string SecondPhoneNumber { get; set; }

            [DisplayName("giới tính")]
            public GenderTypes? Gender { get; set; }

            [DisplayName("quốc gia")]
            public Guid? NationId { get; set; }

            [DisplayName("tỉnh/thành phố")]
            public Guid? ProvinceId { get; set; }

            [DisplayName("quận/huyện")]
            public Guid? DistrictId { get; set; }

            [DisplayName("phường/xã/thị trấn")]
            public Guid? WardId { get; set; }

            [DisplayName("avatar")]
            public IFormFile Image { get; set; }

            public string Username { get; set; }

            public string Email { get; set; }

            public DateTime? DateOfBirth { get; set; }

            public string Address { get; set; }

            public bool? Active { get; set; }

            public string Code { get; set; }

            public bool? ChangePassword { get; set; }
        }
    }
}
