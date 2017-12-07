using MinaHallplatserApi.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinaHallplatserApi.Models
{
    public class NearbyStopsModel
    {
        public class StopLocation
        {
            public string Name { get; set; }
            public string Id { get; set; }
            [JsonIgnore]
            public string Lat { get; set; }
            [JsonIgnore]
            public string Lon { get; set; }
            //[JsonIgnore]
            public string Track { get; set; }
        }

        public class LocationList
        {
            [JsonIgnore]
            public string noNamespaceSchemaLocation { get; set; }
            [JsonIgnore]
            public string servertime { get; set; }
            [JsonIgnore]
            public string serverdate { get; set; }
            [JsonConverter(typeof(StopLocationConverter))]
            public List<StopLocation> StopLocation { get; set; }
        }

        public class NearByStopsObject
        {
            public LocationList LocationList { get; set; }
        }
    }
}
