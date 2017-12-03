using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MinaHallplatserApi.Controllers;
using Newtonsoft.Json;
using MinaHallplatserApi.Models;

namespace MinaHallplatser.Tests
{
    [TestClass]
    public class StopsTests
    {
        [TestMethod]
        public async Task StopNotStartingWithDot()
        {
            string AccessToken = "4cf77fe1-573f-3792-8c87-f1a0c2a4520d";
            var response = await StopsController.DoStopsRequestAsync(AccessToken, "asd");
            string ResponseString = await response.Content.ReadAsStringAsync();
            var jsonData = JsonConvert.DeserializeObject<StopsObject>(ResponseString);
            
            var stops = new List<StopLocation>(jsonData.LocationList.StopLocation);
            stops.RemoveAll(s => s.Name.StartsWith("."));

            foreach (var stop in stops)
            {
                Assert.AreNotEqual(".", stop.Name.Substring(0,1));
            }
        }
    }
}
