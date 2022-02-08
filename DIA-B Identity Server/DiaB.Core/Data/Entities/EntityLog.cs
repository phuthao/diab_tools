// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     EntityLog.cs
// Created Date:  2019/05/17 11:27 AM
// ------------------------------------------------------------------------

using System;

namespace DiaB.Core.Data.Entities
{
    public class EntityLog : BaseEntity<Guid>
    {
        public Guid EntityLogEventId { get; set; }

        public string PropertyName { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public virtual EntityLogEvent EntityLogEvent { get; set; }
    }
}
