using System;
using System.ComponentModel;
using DiaB.Common.Enums;

namespace DiaB.Middle.Dtos.AccountDtos
{
    public partial class AccountDtos
    {
        public partial class AppFilter
        {
            [DisplayName("sắp xếp theo")]
            public string SortBy { get; set; }

            [DisplayName("lọc theo tên")]
            public string Name { get; set; }

            public string Code { get; set; }

            public GenderTypes? Gender { get; set; }

            public int? AgeFrom { get; set; }

            public int? AgeTo { get; set; }

            public Guid? NationId { get; set; }

            public Guid? ProvinceId { get; set; }

            public Guid? DistrictId { get; set; }

            public Guid? WardId { get; set; }

            public bool ExcludeInactive { get; set; } = true;

            public string PhoneNumber { get; set; }

            public int Page { get; set; } = 1;

            public int Size { get; set; } = 1000;
        }
    }
}
