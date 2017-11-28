using Microsoft.AspNetCore.Mvc;
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
        [HttpPost("gps")]
        public async Task<IActionResult> GetGpsAsync(string latitude, string longitude, string access_token)
        {
            try
            {
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                string time = DateTime.Now.ToString("HH:mm");

                HttpClient client = new HttpClient();
                var uri = $"https://api.vasttrafik.se/bin/rest.exe/v2/location.nearbystops?originCoordLat={latitude}&originCoordLong={longitude}&format=json";
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
                    var jsonData = JsonConvert.DeserializeObject<NearByStopsObject>(responseBody);
                    //jsonData.LocationList.StopLocation.RemoveAll(x => x.Track != null);

                    var stops = new List<StopLocation>(jsonData.LocationList.StopLocation);
                    stops.RemoveAll(x => x.Track != null);

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
