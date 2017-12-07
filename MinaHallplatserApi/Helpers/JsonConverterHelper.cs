using MinaHallplatserApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinaHallplatserApi.Helpers
{
    public class StopLocationConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                return serializer.Deserialize<List<StopLocation>>(reader);
            }
            else
            {
                StopLocation media = serializer.Deserialize<StopLocation>(reader);
                return new List<StopLocation>(new[] { media });
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public class DepartureConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                return serializer.Deserialize<List<DepartureModel.Departure>>(reader);
            }
            else
            {
                DepartureModel.Departure media = serializer.Deserialize<DepartureModel.Departure>(reader);
                return new List<DepartureModel.Departure>(new[] { media });
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
