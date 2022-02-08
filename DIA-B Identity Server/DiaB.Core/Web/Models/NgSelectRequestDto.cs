// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     NgSelectRequestDto.cs
// Created Date:  2019/06/24 7:06 PM
// ------------------------------------------------------------------------

using DiaB.Core.Web.Constants;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DiaB.Core.Web.Models
{
    public class NgSelectRequestDto
    {
        [FromQuery(Name = "offset")]
        [JsonProperty("offset")]
        public int Offset { get; set; } = NgSelect.DefaultPage - 1;

        [FromQuery(Name = "limit")]
        [JsonProperty("limit")]
        public int Limit { get; set; } = NgSelect.DefaultPageSize;

        [FromQuery(Name = "term")]
        [JsonProperty("term")]
        public string Term { get; set; }
    }
}
