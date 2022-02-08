// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     EntityLogEvent.cs
// Created Date:  2019/05/17 11:27 AM
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace DiaB.Core.Data.Entities
{
    public class EntityLogEvent : BaseEntity<Guid>
    {
        public string EntityName { get; set; }

        public string EntityKey { get; set; }

        public virtual ICollection<EntityLog> EntityLogs { get; set; } = new List<EntityLog>();
    }
}
