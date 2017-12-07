using MinaHallplatserApi.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MinaHallplatserApi.Models
{
    public static class DepartureModel
    {
        public class Departure
        {
            [JsonIgnore]
            private string _direction { get; set; }
            [JsonIgnore]
            private int? _timeLeft;
            [JsonIgnore]
            private string _time { get; set; }
            [JsonIgnore]
            private string _date { get; set; }
            [JsonIgnore]
            private string _rtTime { get; set; }
            [JsonIgnore]
            private string _rtDate { get; set; }

            public string Direction
            {
                set { _direction = value; }
                get { return !string.IsNullOrEmpty(_direction) && _direction.Contains(" via ") ? _direction.Substring(0, _direction.IndexOf(" via ")) : _direction; }
            }
            public string Via => !string.IsNullOrEmpty(_direction) && _direction.Contains(" via ") ? _direction.Substring(_direction.IndexOf(" via ") + 1) : null;
            public string Name { get; set; }
            public string Sname { get; set; }
            [JsonIgnore]
            public string Type { get; set; }
            [JsonIgnore]
            public string Stopid { get; set; }
            [JsonIgnore]
            public string Stop { get; set; }
            public string Time { set => _time = value; }
            public string Date { set => _date = value; }
            public string Journeyid { get; set; }
            public string Track { get; set; }
            public string RtTime { set => _rtTime = value; }
            public string RtDate { set => _rtDate = value; }
            public string FgColor { get; set; }
            public string BgColor { get; set; }
            [JsonIgnore]
            public string Stroke { get; set; }
            public string Accessibility { get; set; }
            public bool IsLive => _rtTime == null ? false : true;
            public int? TimeLeft { get => _rtTime == null ? GetMinutesUntil(_date, _time) : GetMinutesUntil(_rtDate, _rtTime); set => _timeLeft = value; }
            public int? TimeNext { get; set; }
        }

        public static int? GetMinutesUntil(string Date, string Time)
        {
            DateTime time = DateTime.Parse(Date + " " + Time);
            TimeSpan span = time - DateTime.Now;
            return span.Minutes + 1;
        }

        public class DepartureBoard
        {
            [JsonConverter(typeof(DepartureConverter))]
            public List<Departure> Departure { get; set; }
            public string Error { get; set; }
            public string ErrorText { get; set; }
        }

        public class DepartureObject
        {
            public DepartureBoard DepartureBoard { get; set; }
        }
    }
}
