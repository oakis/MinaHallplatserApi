using Microsoft.AspNetCore.Mvc;
using MinaHallplatserApi.Helpers;
using MinaHallplatserApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MinaHallplatserApi.Controllers
{
    [Route("api/vasttrafik")]
    public class StopsController : Controller
    {
        public string AccessToken { get; private set; }

        public static async Task<HttpResponseMessage> DoStopsRequestAsync(string AccessToken, string Search)
        {
            // Skapa request
            HttpClient client = new HttpClient();
            var uri = new Uri($"https://api.vasttrafik.se/bin/rest.exe/v2/location.name?input={Search}&format=json");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Returnera response
            return await client.GetAsync(uri);
        }

        [HttpPost("stops")]
        public async Task<IActionResult> GetStopsAsync(string Search) => BadRequest(new { error = "Please update Mina Hållplatser to the latest version." });
    }
}
