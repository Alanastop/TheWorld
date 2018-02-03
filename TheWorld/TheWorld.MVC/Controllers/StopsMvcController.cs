using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheWorld.MVC.Controllers.Interfaces;
using TheWorld.MVC.Models;
using TheWorld.Models.Persistent;
using System.Net.Http;
using Newtonsoft.Json;
using AutoMapper;
using System.Text;

namespace TheWorld.MVC.Controllers
{
    public class StopsMvcController : Controller, IMvcController<StopViewModel>
    {
        public IActionResult Get()
        {
            List<Stop> stops = null;
            var httpClient = new HttpClient();
            var response = httpClient.GetAsync("http://localhost:10816/api/stops").Result;
            if (response.IsSuccessStatusCode)
            {
                var stateInfo = response.Content.ReadAsStringAsync().Result;
                stops = JsonConvert.DeserializeObject<List<Stop>>(stateInfo);
                var stopViewModel = Mapper.Map<IEnumerable<StopViewModel>>(stops);
                return this.View(stopViewModel);
            }
            return this.View();
        }
       
        public IActionResult GetEntityByName(string entityName)
        {
            var httpClient = new HttpClient();
            var response = httpClient.GetAsync($"http://localhost:10816/api/stops/{entityName}/GetEntityByName/").Result;
            if (response.IsSuccessStatusCode)
            {
                var stateInfo = response.Content.ReadAsStringAsync().Result;
                var localTrip = JsonConvert.DeserializeObject<Trip>(stateInfo);

                var stopViewModel = Mapper.Map<IEnumerable<StopViewModel>>(localTrip.Stops);
                return this.View(stopViewModel);
            }

            return this.View();
        }

        public IActionResult Index()
        {

            List<Stop> stops = null;
            var httpClient = new HttpClient();
            var response = httpClient.GetAsync("http://localhost:10816/api/stops/US_Trip/GetEntityByName/").Result;
            if (response.IsSuccessStatusCode)
            {
                var stateInfo = response.Content.ReadAsStringAsync().Result;
                stops = JsonConvert.DeserializeObject<List<Stop>>(stateInfo);
                var tripViewModels = Mapper.Map<IEnumerable<StopViewModel>>(stops);
            }
            return this.View();
        }

        public async Task<IActionResult> PostAsync([FromBody]StopViewModel stopViewModel)
        {
            if (ModelState.IsValid)
            {
                var stop = Mapper.Map<Stop>(stopViewModel);
                var httpClient = new HttpClient();

                var response = await httpClient.PostAsync(
                                   $"http://localhost:10816/api/stops/{stopViewModel.Name}/post/",
                                   new StringContent(JsonConvert.SerializeObject(stop), Encoding.UTF8, "application/json"));
                return this.RedirectToAction("Get");
            }
            else
                return BadRequest(ModelState);
        }
    }
}