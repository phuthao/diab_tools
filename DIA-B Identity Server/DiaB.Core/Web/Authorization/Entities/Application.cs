using DiaB.Core.Data.Entities;
using System;
using DiaB.Core.Common.Helpers;

namespace DiaB.Core.Web.Authorization.Entities
{
    public class Application : BaseEntity<Guid>
    {
        public string Name { get; set; }

        public string PrivateKey { get; set; } = SecurityHelper.GenerateCriptoKey();

        public string Description { get; set; }
    }
}
