using Microsoft.AspNetCore.Mvc;
using MinaHallplatserApi.Models;
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
        [HttpPost("departures")]
        public async Task<IActionResult> GetDeparturesAsync(string id, string access_token)
        {
            try
            {
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                string time = DateTime.Now.ToString("HH:mm");

                // Skapa request
                HttpClient client = new HttpClient();
                var uri = new Uri($"https://api.vasttrafik.se/bin/rest.exe/v2/departureBoard?id={id}&date={date}&time={time}&format=json&timeSpan=90&maxDeparturesPerLine=2&needJourneyDetail=0");
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
                    string responseBody = await response.Content.ReadAsStringAsync();
                    DepartureObject jsonData = JsonConvert.DeserializeObject<DepartureObject>(responseBody);

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
