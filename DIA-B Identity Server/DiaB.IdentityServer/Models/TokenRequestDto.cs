﻿using Newtonsoft.Json;

namespace DiaB.IdentityServer.Models
{
    public class TokenRequestDto
    {
        [JsonProperty("grant_type")]
        public string GrantType { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
