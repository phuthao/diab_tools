using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace DiaB.Core.Web.Models
{
    public class HmacRequestDto
    {
        [FromQuery(Name = "nonce")]
        [JsonProperty("nonce")]
        [JsonPropertyName("nonce")]
        public Guid Nonce { get; set; }

        [FromQuery(Name = "requestTime")]
        [JsonProperty("requestTime")]
        [JsonPropertyName("requestTime")]
        public long RequestTime { get; set; }

        [FromQuery(Name = "publicKey")]
        [JsonProperty("publicKey")]
        [JsonPropertyName("publicKey")]
        public string PublicKey { get; set; }

        [FromQuery(Name = "signature")]
        [JsonProperty("signature")]
        [JsonPropertyName("signature")]
        public string Signature { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public string Data { get; set; }
    }
}
