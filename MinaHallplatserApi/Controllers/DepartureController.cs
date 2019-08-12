using Microsoft.AspNetCore.Mvc;
using MinaHallplatserApi.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static MinaHallplatserApi.Models.DepartureModel;

namespace MinaHallplatserApi.Controllers
{
    [Route("api/vasttrafik")]
    public class DepartureController : Controller
    {
        public string AccessToken { get; private set; }

        private async Task<HttpResponseMessage> DoDeparturesRequestAsync(string AccessToken, string Id, string TimeSpan = "90")
        {
            string Date = DateTime.Now.ToString("yyyy-MM-dd");
            string Time = DateTime.Now.ToString("HH:mm");

            // Skapa request
            HttpClient client = new HttpClient();
            var uri = new Uri($"https://api.vasttrafik.se/bin/rest.exe/v2/departureBoard?id={Id}&date={Date}&time={Time}&format=json&timeSpan={TimeSpan}&maxDeparturesPerLine=2&needJourneyDetail=0");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            // Returnera response
            return await client.GetAsync(uri);
        }

        [HttpPost("departures")]
        public async Task<IActionResult> GetDeparturesAsync(string Id) => BadRequest(new { error = "Please update Mina Hållplatser to the latest version." });
    }
}
