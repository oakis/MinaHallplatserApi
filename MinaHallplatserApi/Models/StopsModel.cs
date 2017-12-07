using MinaHallplatserApi.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinaHallplatserApi.Models
{
    public class StopLocation
    {
        public string Name { get; set; }
        [JsonIgnore]
        public string Lon { get; set; }
        [JsonIgnore]
        public string Lat { get; set; }
        public string Id { get; set; }
        [JsonIgnore]
        public string Idx { get; set; }
    }

    public class CoordLocation
    {
        public string Name { get; set; }
        public string Lon { get; set; }
        public string Lat { get; set; }
        public string Type { get; set; }
        public string Idx { get; set; }
    }

    public class LocationList
    {
        [JsonIgnore]
        public string NoNamespaceSchemaLocation { get; set; }
        [JsonIgnore]
        public string Servertime { get; set; }
        [JsonIgnore]
        public string Serverdate { get; set; }
        [JsonConverter(typeof(StopLocationConverter))]
        public List<StopLocation> StopLocation { get; set; }
        [JsonIgnore]
        public List<CoordLocation> CoordLocation { get; set; }
    }

    public class StopsObject
    {
        public LocationList LocationList { get; set; }
    }
}
