// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     BaseEntity.cs
// Created Date:  2019/05/17 11:27 AM
// ------------------------------------------------------------------------

using System;

namespace DiaB.Core.Data.Entities
{
    public class BaseEntity<T>
    {
        public virtual T Id { get; set; }

        public virtual bool IsDeleted { get; set; } = false;

        public virtual DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public virtual Guid? CreatedBy { get; set; }

        public virtual DateTime? ModifiedOn { get; set; }

        public virtual Guid? ModifiedBy { get; set; }

        public virtual Guid? DeletedBy { get; set; }

        public virtual DateTime? DeletedOn { get; set; }
    }
}
