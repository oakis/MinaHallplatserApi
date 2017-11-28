using Microsoft.AspNetCore.Mvc;
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
        [HttpPost("stops")]
        public async Task<IActionResult> GetStopsAsync(string busStop, string access_token)
        {
            try {
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                string time = DateTime.Now.ToString("HH:mm");

                // Skapa request
                HttpClient client = new HttpClient();
                var uri = new Uri($"https://api.vasttrafik.se/bin/rest.exe/v2/location.name?input={busStop}&format=json");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Skicka request
                var response = await client.GetAsync(uri);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException();
                    }
                    else
                    {
                        throw new HttpRequestException();
                    }
                }
                else
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var jsonData = JsonConvert.DeserializeObject<StopsObject>(responseBody);
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
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (HttpRequestException)
            {
                return StatusCode(500);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
