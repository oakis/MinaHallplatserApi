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
        public async Task<IActionResult> GetDeparturesAsync(string Id)
        {
            try
            {
                if (Request.Headers.Keys.Contains("access_token"))
                {
                    AccessToken = Request.Headers["access_token"];
                }
                else
                {
                    return BadRequest(new { error = "access_token missing or invalid." });
                }

                // Try request
                var response = await DoDeparturesRequestAsync(AccessToken, Id);
                HttpRequestHelper.ThrowIfNotOk(response);
                string ResponseString = await response.Content.ReadAsStringAsync();
                DepartureObject jsonData = JsonConvert.DeserializeObject<DepartureObject>(ResponseString);

                if (jsonData.DepartureBoard.Error == "No journeys found")
                {
                    response = await DoDeparturesRequestAsync(AccessToken, Id, "1440");
                    HttpRequestHelper.ThrowIfNotOk(response);
                    ResponseString = await response.Content.ReadAsStringAsync();
                    jsonData = JsonConvert.DeserializeObject<DepartureObject>(ResponseString);
                    if (!String.IsNullOrWhiteSpace(jsonData.DepartureBoard.Error))
                    {
                        throw new Exception("Something went wrong.");
                    }
                }
                
                List<Departure> departures = new List<Departure>(jsonData.DepartureBoard.Departure);
                List<Departure> returnValue = new List<Departure>();

                for (var i = 0; i < departures.Count; i++)
                {
                    if (returnValue.Count == 0)
                    {
                        returnValue.Add(departures[i]);
                    }
                    else
                    {
                        var index = returnValue.FindIndex(departure => departure.Name == departures[i].Name && departure.Direction == departures[i].Direction);
                        if (index != -1 && returnValue[index].TimeNext == null)
                        {
                            var timeLeft = returnValue[index].TimeLeft;
                            var timeNext = returnValue[index].TimeNext;
                            if (timeLeft > timeNext)
                            {
                                returnValue[index].TimeLeft = timeNext;
                                returnValue[index].TimeNext = timeLeft;
                            }
                            else
                            {
                                returnValue[index].TimeNext = departures[i].TimeLeft;
                            }
                        }
                        else if (index == -1)
                        {
                            returnValue.Add(departures[i]);
                        }
                    }
                }


                return Ok(value: new {
                    departures = returnValue.OrderBy(x => x.TimeLeft).ThenBy(x => x.TimeNext),
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
