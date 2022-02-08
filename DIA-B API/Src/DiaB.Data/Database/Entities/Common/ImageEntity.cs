using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CpTech.Core.Data.Entities;
using DiaB.Common.Enums;
using DiaB.Data.Database.Entities.Account;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiaB.Data.Database.Entities.Common
{
    public class ImageEntity : ModelEntity<ImageEntity>
    {
        [Required]
        public string PhysicalPath { get; set; }

        [Required]
        public long Size { get; set; }

        [Required]
        public string Checksum { get; set; }

        [Required]
        [MaxLength(250)]
        public string Title { get; set; }

        public ImageTypes Type { get; set; }
    }
}
