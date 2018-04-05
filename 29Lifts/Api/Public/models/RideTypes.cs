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
    public class RideTypes
    {
        [DataMember(Name = "ride_types")]
        public List<RideType> Rides { get; set; }
    }

    [DataContract]
    public class RideType
    {
        [DataMember(Name = "display_name")]
        public string DisplayName { get; set; }

        //[JsonConverter(typeof(JsonLyftRideTypeConverter))]
        [JsonProperty(PropertyName = "ride_type")]
        public LyftRideTypeEnum Ride_Type { get; set; }

        [DataMember(Name = "image_url")]
        public string ImageUrl { get; set; }

        [DataMember(Name = "pricing_details")]
        public PricingDetail PricingDetails { get; set; }

        [DataMember(Name = "seats")]
        public int Seats{ get; set; }

    }

    [DataContract]
    public class PricingDetail
    {
        [DataMember(Name = "base_charge")]
        public int BaseCharge { get; set; }

        [DataMember(Name = "cost_per_mile")]
        public int CostPerMile { get; set; }

        [DataMember(Name = "cost_per_minute")]
        public int CostPerMinute { get; set; }

        [DataMember(Name = "cost_minimum")]
        public int CostMinimum { get; set; }

        [DataMember(Name = "trust_and_service")]
        public int TrustAndService { get; set; }

        [DataMember(Name = "currency")]
        public string Currency { get; set; }

        [DataMember(Name = "cancel_penalty_amount")]
        public string CancelPenaltyAmount { get; set; }

    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum LyftRideTypeEnum
    {
        [EnumMember(Value = "unknow")]
        Unknown,
        [EnumMember(Value = "lyft")]
        Lyft,
        [EnumMember(Value = "lyft_plus")]
        Plus,
        [EnumMember(Value = "lyft_line")]
        Line,
        [EnumMember(Value = "lyft_premier")]
        Premier,
        [EnumMember(Value = "lyft_luxsuv")]
        LuxSuv,
        [EnumMember(Value = "lyft_lux")]
        Lux,
    }
}
