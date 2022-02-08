using System;

namespace DiaB.Middle.Dtos.ProvinceDtos
{
    public partial class ProvinceDtos
    {
        public partial class Filter
        {
            public string SearchTerm { get; set; }

            public Guid? NationId { get; set; }
        }
    }
}
