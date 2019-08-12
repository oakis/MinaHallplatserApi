using Microsoft.AspNetCore.Mvc;
using MinaHallplatserApi.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static MinaHallplatserApi.Models.NearbyStopsModel;

namespace MinaHallplatserApi.Controllers
{
    [Route("api/vasttrafik")]
    public class NearbyStopsController : Controller
    {
        public string AccessToken { get; private set; }

        private async Task<HttpResponseMessage> DoGpsRequestAsync(string AccessToken, string Latitude, string Longitude)
        {
            HttpClient client = new HttpClient();
            var uri = $"https://api.vasttrafik.se/bin/rest.exe/v2/location.nearbystops?originCoordLat={Latitude}&originCoordLong={Longitude}&format=json";
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Skicka request
            return await client.GetAsync(uri);
    }

        [HttpPost("gps")]
        public async Task<IActionResult> GetGpsAsync(string latitude, string longitude) => BadRequest(new { error = "Please update Mina Hållplatser to the latest version." });
    }
}
