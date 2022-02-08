// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     BaseEnumEntity.cs
// Created Date:  2019/05/17 11:27 AM
// ------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiaB.Core.Data.Entities
{
    public class BaseEnumEntity<T> where T : struct
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual T Id { get; set; }

        [Required] public virtual string Name { get; set; }

        public virtual string Description { get; set; }
    }
}
