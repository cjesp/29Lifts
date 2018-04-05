using _29Lifts.Api.Public.models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace _29Lifts.Helper
{
    public class JsonLyftRideTypeConverterOld : CustomCreationConverter<LyftRideTypeEnum>
    {
        public override LyftRideTypeEnum Create(Type objectType)
        {
            throw new NotImplementedException();
        }

        public LyftRideTypeEnum Create(object incomingObject)
        {
            var type = (string)incomingObject;

            switch (type.ToLower())
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

            throw new Exception(String.Format("The Lyft ride type {0} is not supported!", type));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Load JObject from stream 
            //var jObject = JObject.Load(reader);

            // Create target object based on JObject 
            //var _existingValue = (string)existingValue;

            var target = Create(existingValue);

            // Populate the object properties 
            //serializer.Populate(jObject.CreateReader(), target);

            return target;
        }

    }
}
