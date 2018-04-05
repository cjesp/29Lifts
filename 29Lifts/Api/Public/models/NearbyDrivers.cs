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
    public class Location
    {
        [DataMember(Name = "lat")]
        public double lat { get; set; }

        [DataMember(Name = "lng")]
        public double lng { get; set; }
    }

    [DataContract]
    public class Driver
    {
        [DataMember(Name = "locations")]
        public List<Location> Locations { get; set; }
    }

    [DataContract]
    public class NearbyDriver
    {
        [DataMember(Name = "drivers")]
        public List<Driver> Drivers { get; set; }

        [JsonProperty(PropertyName = "ride_type")]
        //[JsonConverter(typeof(JsonLyftRideTypeConverter))]
        public LyftRideTypeEnum RideType { get; set; }
    }

    [DataContract]
    public class NearbyDrivers
    {
        [DataMember(Name = "nearby_drivers")]
        public List<NearbyDriver> Drivers { get; set; }
    }
}
