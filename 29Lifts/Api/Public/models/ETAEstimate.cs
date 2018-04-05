using _29Lifts.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace _29Lifts.Api.Public.models
{
    [DataContract]
    public class ETAEstimates
    {
        [DataMember(Name = "eta_estimates")]
        public List<ETAEstimate> Estimates { get; set; }
    }

    [DataContract]
    public class ETAEstimate
    {
        [DataMember(Name = "display_name")]
        public string DisplayName { get; set; }

        [JsonProperty(PropertyName = "ride_type")]
        //[JsonConverter(typeof(JsonLyftRideTypeConverter))]
        public LyftRideTypeEnum RideType { get; set; }

        [DataMember(Name = "eta_seconds")]
        public int? EtaSeconds { get; set; }

        [DataMember(Name = "is_valid_estimate")]
        public bool IsValidEstimate { get; set; }
    }
}
