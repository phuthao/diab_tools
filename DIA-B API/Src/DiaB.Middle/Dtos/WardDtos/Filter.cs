using System;

namespace DiaB.Middle.Dtos.WardDtos
{
    public partial class WardDtos
    {
        public partial class Filter
        {
            public string SearchTerm { get; set; }

            public Guid? DistrictId { get; set; }
        }
    }
}
