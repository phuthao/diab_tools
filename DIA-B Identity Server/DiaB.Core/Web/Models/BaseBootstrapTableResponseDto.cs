// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     BaseBootstrapTableResponseDto.cs
// Created Date:  2019/07/09 1:00 PM
// ------------------------------------------------------------------------

using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiaB.Core.Web.Models
{
    public class BaseBootstrapTableResponseDto<TRow, TData>
    {
        [JsonProperty("total")] public long Total { get; set; }

        [JsonProperty("rows")] public List<TRow> Rows { get; set; }

        [JsonProperty("data")] public TData Data { get; set; }

        [JsonConstructor]
        public BaseBootstrapTableResponseDto()
        {
        }
    }
}
