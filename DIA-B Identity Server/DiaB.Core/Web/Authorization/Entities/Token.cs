// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     Token.cs
// Created Date:  2018/11/15 6:39 PM
// ------------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;
using DiaB.Core.Data.Entities;
using DiaB.Core.Data.Attributes;

namespace DiaB.Core.Web.Authorization.Entities
{
    public class Token : BaseEntity<Guid>
    {
        [Required] public string UserIdentifier { get; set; }

        [Required] public string AccessToken { get; set; }

        public string ClientToken { get; set; }

        public DateTime? ValidFrom { get; set; }

        public DateTime? ValidTo { get; set; }

        public string IpAddress { get; set; }

        public string Location { get; set; }

        public string BrowserFingerprint { get; set; }

        [Log] public string Browser { get; set; }
    }
}
