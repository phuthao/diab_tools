// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     BaseBootstrapTableRequestDto.cs
// Created Date:  2019/07/09 12:55 PM
// ------------------------------------------------------------------------

using DiaB.Core.Web.Constants;
using DiaB.Core.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using DiaB.Core.Common.Extensions;

namespace DiaB.Core.Web.Models
{
    public class BaseBootstrapTableRequestDto<TSearch>
    {
        [FromQuery(Name = "offset")]
        [JsonProperty("offset")]
        [JsonPropertyName("offset")]
        public int Offset { get; set; } = BootstrapTable.DefaultPage - 1;

        [FromQuery(Name = "limit")]
        [JsonProperty("limit")]
        [JsonPropertyName("limit")]
        public int Limit { get; set; } = BootstrapTable.DefaultPageSize;

        [FromQuery(Name = "sort")]
        [JsonProperty("sort")]
        [JsonPropertyName("sort")]
        public string SortKey { get; set; }

        [FromQuery(Name = "order")]
        [JsonProperty("order")]
        [JsonPropertyName("order")]
        public string SortDirection { get; set; } = BootstrapTable.DescSortDirection;

        [FromQuery(Name = "search")]
        [JsonProperty("search")]
        [JsonPropertyName("search")]
        public string Search { get; set; }

        private TSearch _searchData;

        public TSearch SearchData
        {
            get
            {
                if (_searchData != null)
                {
                    return _searchData;
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    _searchData = Search.ToObject<TSearch>();
                    return _searchData;
                }

                return default;
            }
        }

        [FromQuery(Name = "isDescending")]
        [JsonProperty("isDescending")]
        [JsonPropertyName("isDescending")]
        public bool IsDescending => SortDirection.EqualsIgnoreCase(BootstrapTable.DescSortDirection);

        //[System.Text.Json.Serialization.JsonConstructor]
        [Newtonsoft.Json.JsonConstructor]
        public BaseBootstrapTableRequestDto()
        {
        }
    }
}
