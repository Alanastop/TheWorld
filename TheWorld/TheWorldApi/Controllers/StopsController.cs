using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheWorldApi.Controllers.Interfaces;
using TheWorld.Models.Persistent;
using TheWorld.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using AutoMapper;
using TheWorld.MVC.Models;
using TheWorld.Services;
using TheWorldApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace TheWorldApi.Controllers
{   
    [Route("/api/stops")]
    public class StopsController : Controller, IApiController<Stop>
    {
        private readonly IEntityRepository<Stop> stopRepository;

        private readonly IEntityRepository<Trip> localTripRepository;

        private readonly ILogger<StopsController> stopsControllerlogger;

        public readonly GeoCoordsService GeoCoordsService;

        public StopsController(IEntityRepository<Stop> repository, ILogger<StopsController> logger,
            IEntityRepository<Trip> tripRepository, GeoCoordsService geoCoordsService)
        {
            this.stopRepository = repository;
            this.stopsControllerlogger = logger;
            this.localTripRepository = tripRepository;
            this.GeoCoordsService = geoCoordsService;
        }

        [HttpGet("/api/stops/")]
        public IActionResult Get(string username)
        {
            return this.Ok(this.stopRepository.GetByUserName(username));
        }

        [HttpGet("/api/stops/{entityName}/{username}/getentitybyname")]
        public IActionResult GetEntityByName(string entityName, string username)
        {
            return this.Ok(this.localTripRepository.GetEntityByName(entityName, username).Stops);
        }
                
        [HttpPost("/api/stops/{tripName}/{username}/post")]
        public async Task<IActionResult> Post(string tripName, string username, [FromBody]Stop stop)//string username, string stopLocation,
        {           
            var trip = this.localTripRepository.GetEntityByName(tripName, username);

            if (trip != null)
            {
                //this.GeoCoordsService.GetCoordsAsync(stop.Name).Wait();
                // Lookup the Geocodes
                var result = await this.GeoCoordsService.GetCoordsAsync(stop.Name);
                           
                if (!result.Success)
                {
                    this.stopsControllerlogger.LogError(result.Message);
                                       
                        // Save to the Database
                        this.stopRepository.AddStopByTrip(trip, stop);

                    if (await this.stopRepository.SaveChangesAsync())
                        return Created($"api/stops/{stop.Name} Without location", Mapper.Map<StopViewModel>(stop));
                    else
                        return BadRequest("Failed to store changes to database");
                }
                else
                {
                    stop.Latitude = result.Latitude;
                    stop.Longtitude = result.Longitude;
                    
                        // Save to the Database
                        this.stopRepository.AddStopByTrip(trip, stop);                    

                    if (await this.stopRepository.SaveChangesAsync())
                        return Created($"api/stops/{stop.Name}", Mapper.Map<StopViewModel>(stop));
                    else
                        return BadRequest("Failed to store changes to database");
                }
            }
            else
                return BadRequest("The given trip couldn't be found. Please give a valid trip name");
        }

        [HttpPost("/api/stops/{tripName}/{username}/update")]
        public async Task<IActionResult> Update(string tripName, string username, [FromBody]Stop stop)//string username, string stopLocation,
        {
            var trip = this.localTripRepository.GetEntityByName(tripName, username);

            if (trip != null)
            {
                //this.GeoCoordsService.GetCoordsAsync(stop.Name).Wait();
                // Lookup the Geocodes
                var result = await this.GeoCoordsService.GetCoordsAsync(stop.Name);

                if (!result.Success)
                {
                    this.stopsControllerlogger.LogError(result.Message);

                        // Save to the Database
                        this.stopRepository.UpdateEntity(stop);

                    if (await this.stopRepository.SaveChangesAsync())
                        return Created($"api/stops/{stop.Name} Without location", Mapper.Map<StopViewModel>(stop));
                    else
                        return BadRequest("Failed to store changes to database");
                }
                else
                {
                    stop.Latitude = result.Latitude;
                    stop.Longtitude = result.Longitude;

                    // Save to the Database
                    this.stopRepository.UpdateEntity(stop);

                    if (await this.stopRepository.SaveChangesAsync())
                        return Created($"api/stops/{stop.Name}", Mapper.Map<StopViewModel>(stop));
                    else
                        return BadRequest("Failed to store changes to database");
                }
            }
            else
                return BadRequest("The given trip couldn't be found. Please give a valid trip name");
        }

        [HttpPost("api/stops")]
        public Task<IActionResult> Post(Stop trip)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The post.
        /// </summary>
        /// <param name="stop">
        /// The trip.
        /// </param>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        [HttpPost("/api/stops/delete")]
        public async Task<IActionResult> Delete([FromBody]Stop stop)
        {
            this.stopRepository.DeleteEntity(stop.Id);

            if (await this.stopRepository.SaveChangesAsync())
            {
                return this.Ok("The stop removed successfully");
            }
            else
                return BadRequest("Failed to store changes to database");
        }
    }
}