using _29Lifts.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace _29Lifts.Api.Public.models
{
    [DataContract]
    public class CostEstimates
    {
        [DataMember(Name = "cost_estimates")]
        public List<CostEstimate> Estimates { get; set; }
    }

    [DataContract]
    public class CostEstimate
    {
        [JsonProperty(PropertyName = "ride_type")]
        //[JsonConverter(typeof(JsonLyftRideTypeConverter))]
        public LyftRideTypeEnum RideType { get; set; }

        [DataMember(Name = "estimated_duration_seconds")]
        public int EstimatedDurationSeconds { get; set; }

        [DataMember(Name = "estimated_distance_miles")]
        public double EstimatedDistanceMiles { get; set; }

        [DataMember(Name = "estimated_cost_cents_max")]
        public int EstimatedCostCentsMax { get; set; }

        [DataMember(Name = "primetime_percentage")]
        public string PrimetimePercentage { get; set; }

        [DataMember(Name = "currency")]
        public string Currency { get; set; }

        [DataMember(Name = "estimated_cost_cents_min")]
        public int EstimatedCostCentsMin { get; set; }

        [DataMember(Name = "display_name")]
        public string DisplayName { get; set; }

        [DataMember(Name = "primetime_confirmation_token")]
        public object PrimetimeConfirmationToken { get; set; }

        [DataMember(Name = "is_valid_estimate")]
        public bool IsValidEstimate { get; set; }
    }


    
}
