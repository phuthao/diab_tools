using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiaB.IdentityServer.Entities
{
    [Table("common_configures")]
    public class DiabSetting
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}