﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace _29Lifts.Api.Public.models
{
    [DataContract]
    public class AccessTokenPublic
    {
        [DataMember(Name = "token_type")]
        public string TokenType { get; set; }

        [DataMember(Name = "access_token")]
        public string Token { get; set; }

        [DataMember(Name = "expires_in")]
        public int ExpiresIn { get; set; }

        [DataMember(Name = "scope")]
        public string Scope { get; set; }
    }
}
