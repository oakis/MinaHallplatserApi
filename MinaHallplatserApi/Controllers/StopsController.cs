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
        public async Task<IActionResult> GetStopsAsync(string Search)
        {
            try {

                if (Request.Headers.Keys.Contains("access_token"))
                {
                    AccessToken = Request.Headers["access_token"];
                }
                else
                {
                    return BadRequest(new { error = "access_token missing or invalid." });
                }

                var response = await DoStopsRequestAsync(AccessToken, Search);
                HttpRequestHelper.ThrowIfNotOk(response);
                string ResponseString = await response.Content.ReadAsStringAsync();
                var jsonData = JsonConvert.DeserializeObject<StopsObject>(ResponseString);
                if (jsonData.LocationList.StopLocation == null)
                    return NotFound(value: new { data = "Hittade inga hållplatser. Prova att söka på något annat.", timestamp = DateTime.Now });

                var stops = new List<StopLocation>(jsonData.LocationList.StopLocation);
                stops.RemoveAll(s => s.Name.StartsWith("."));
                if (stops.Count > 10)
                    stops.RemoveRange(10, stops.Count - 10);

                return Ok(value: new
                {
                    data = stops,
                    timestamp = DateTime.Now
                });
                
            }
            catch (UnauthorizedAccessException)
            {
                return BadRequest(new { error = "access_token missing or invalid." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
