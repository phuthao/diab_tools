using System;

namespace DiaB.Middle.Dtos.DistrictDtos
{
    public partial class DistrictDtos
    {
        public partial class Filter
        {
            public string SearchTerm { get; set; }

            public Guid? ProvinceId { get; set; }
        }
    }
}
