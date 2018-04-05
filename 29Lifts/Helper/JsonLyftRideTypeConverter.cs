using _29Lifts.Api.Public.models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _29Lifts.Helper
{
    public class JsonLyftRideTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(LyftRideTypeEnum);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var val = (string)reader.Value;

            switch (val.ToLower())
            {
                case "lyft_line":
                    return LyftRideTypeEnum.Line;
                case "lyft":
                    return LyftRideTypeEnum.Lyft;
                case "lyft_plus":
                    return LyftRideTypeEnum.Plus;
                case "lyft_premier":
                    return LyftRideTypeEnum.Premier;
            }

            return LyftRideTypeEnum.Unknown;

        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
