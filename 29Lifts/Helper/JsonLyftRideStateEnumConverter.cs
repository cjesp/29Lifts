using _29Lifts.Api.Rides.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _29Lifts.Helper
{
    public class JsonLyftRideStateEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(LyftRideState);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var str = (string)reader.Value;

            switch (str.ToLower())
            {
                case "pending":
                    return LyftRideState.Pending;
                case "accepted":
                    return LyftRideState.Accepted;
                case "arrived":
                    return LyftRideState.Arrived;
                case "pickedup":
                    return LyftRideState.PickedUp;
                case "droppedoff":
                    return LyftRideState.DroppedOff;
                case "canceled":
                    return LyftRideState.Canceled;
                default:
                    return LyftRideState.Error;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var str = value as string;
            if (str != null)
            {

            }
        }
    }
}
