using Newtonsoft.Json;
using System;

namespace DiaB.Core.Web.Models
{
    public class RequestDetailsDto
    {
        [JsonProperty("requestName")]
        public string RequestName { get; set; }

        [JsonProperty("requestMethod")]
        public string RequestMethod { get; set; }

        [JsonProperty("requestUrl")]
        public string RequestUrl { get; set; }

        [JsonProperty("eventTime")]
        public DateTime EventTime { get; set; }

        [JsonProperty("executionTime")]
        public long ExecutionTime { get; set; }

        [JsonProperty("responseCode")]
        public int ResponseCode { get; set; }

        [JsonProperty("requestPayload")]
        public object RequestPayload { get; set; }
    }
}
