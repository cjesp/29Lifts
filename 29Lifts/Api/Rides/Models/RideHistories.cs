
using _29Lifts.Api.Rides.Models.SubModels;
using _29Lifts.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace _29Lifts.Api.Rides.Models
{
    [DataContract]
    public class RideHistory
    {
        [DataMember(Name = "ride_id")]
        public string RideId { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        //[JsonConverter(typeof(JsonLyftRideTypeConverter))]
        [JsonProperty(PropertyName = "ride_type")]
        public Public.models.LyftRideTypeEnum RideType { get; set; }

        [DataMember(Name = "passenger")]
        public Passenger Passenger { get; set; }

        [DataMember(Name = "driver")]
        public Driver Driver { get; set; }

        [DataMember(Name = "vehicle")]
        public Vehicle Vehicle { get; set; }

        [DataMember(Name = "origin")]
        public Origin Origin { get; set; }

        [DataMember(Name = "destination")]
        public Destination Destination { get; set; }

        [DataMember(Name = "pickup")]
        public Pickup Pickup { get; set; }

        [DataMember(Name = "dropoff")]
        public Dropoff Dropoff { get; set; }

        [DataMember(Name = "location")]
        public Location Location { get; set; }

        [DataMember(Name = "primetime_percentage")]
        public string PrimetimePercentage { get; set; }

        [DataMember(Name = "price")]
        public Price Price { get; set; }

        [DataMember(Name = "line_items")]
        public List<LineItem> LineItems { get; set; }

        [DataMember(Name = "eta_seconds")]
        public int EtaSeconds { get; set; }

        [DataMember(Name = "requested_at")]
        public DateTime RequestedAt { get; set; }
    }

    [DataContract]
    public class RideHistories
    {
        [DataMember(Name = "ride_history")]
        public List<RideHistory> RideHistory { get; set; }
    }
}
