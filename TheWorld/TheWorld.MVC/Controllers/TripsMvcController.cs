using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheWorld.MVC.Controllers.Interfaces;
using TheWorld.MVC.Models;
using System.Net.Http;
using Newtonsoft.Json;
using TheWorld.Models.Persistent;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace TheWorld.MVC.Controllers
{
    public class TripsMvcController : Controller, IMvcController<TripViewModel>
    {
        [Authorize]
        public IActionResult Get()
        {
            
            List<Trip> trips = null;
            var httpClient = new HttpClient();
            var response = httpClient.GetAsync("http://localhost:10816/api/trips").Result;
            if (response.IsSuccessStatusCode)
            {
                var stateInfo = response.Content.ReadAsStringAsync().Result;
                trips = JsonConvert.DeserializeObject<List<Trip>>(stateInfo);
                var tripViewModels = Mapper.Map<IEnumerable<TripViewModel>>(trips);
            }
            return this.View(trips);
        }

        public IActionResult GetEntityByName(string entityName)
        {
            var httpClient = new HttpClient();
            var response = httpClient.GetAsync($"http://localhost:10816/api/trips/getentitybyname?id={entityName}").Result;
            if (response.IsSuccessStatusCode)
            {
                var stateInfo = response.Content.ReadAsStringAsync().Result;
                var localTrip = JsonConvert.DeserializeObject<Trip>(stateInfo);                
                return this.View(localTrip);
            }

            return this.View();
        }

        [Authorize]
        public IActionResult GetJs()
        {
            return this.View();
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public async Task<IActionResult> PostAsync([FromBody]TripViewModel tripViewModel)
        {
            if (ModelState.IsValid)
            {
                var trip = Mapper.Map<Trip>(tripViewModel);
                var httpClient = new HttpClient();

                var response = await httpClient.PostAsync(
                                   "http://localhost:10816/api/trips",
                                   new StringContent(JsonConvert.SerializeObject(trip), Encoding.UTF8, "application/json"));
                return this.RedirectToAction("Get");
            }
            else
                return BadRequest(ModelState);
        }
                
    }
}