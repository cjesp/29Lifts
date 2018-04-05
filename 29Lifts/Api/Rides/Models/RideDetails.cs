using _29Lifts.Api.Rides.Models.SubModels;
using _29Lifts.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace _29Lifts.Api.Rides.Models
{
    [DataContract]
    public class RideDetails
    {
        [DataMember(Name = "ride_id")]
        public string RideId { get; set; }

        //[JsonProperty(PropertyName = "status")]
        //[JsonConverter(typeof(JsonLyftRideStateEnumConverter))]
        [DataMember(Name = "status")]
        public LyftRideState Status { get; set; }

        [DataMember(Name = "ride_type")]
        public string RideType { get; set; }

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

        [DataMember(Name = "requested_at")]
        public string RequestedAt { get; set; }

        [DataMember(Name = "ride_profile")]
        public string RideProfile { get; set; }

        [DataMember(Name = "canceled_by")]
        public string CanceledBy { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum LyftRideState
    {
        [EnumMember(Value = "pending")]
        Pending,

        [EnumMember(Value = "accepted")]
        Accepted,

        [EnumMember(Value = "arrived")]
        Arrived,

        [EnumMember(Value = "pickedUp")]
        PickedUp,

        [EnumMember(Value = "droppedOff")]
        DroppedOff,

        [EnumMember(Value = "canceled")]
        Canceled,

        [EnumMember(Value = "error")]
        Error,
    }
}
