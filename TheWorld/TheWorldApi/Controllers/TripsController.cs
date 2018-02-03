// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiController.cs" company="DataComm">
//   The World Api.
// </copyright>
// <summary>
//   The api controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TheWorldApi.Controllers
{    
    using AutoMapper;
    #region

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System.Threading.Tasks;
    using TheWorld.Models.Persistent;
    using TheWorld.MVC.Models;
    using TheWorld.Repositories.Interfaces;

    using TheWorldApi.Controllers.Interfaces;
    using TheWorld.Models.Persistent.Interfaces;
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;

    #endregion
        
    /// <summary>
    /// The api controller.
    /// </summary>
    public class TripsController : Controller, IApiController<Trip>
    {
        /// <summary>
        /// The local repository.
        /// </summary>
        private readonly IEntityRepository<Trip> localRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TripsController"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        public TripsController(IEntityRepository<Trip> repository, ILogger<TripsController> logger)
        {
            this.localRepository = repository;
        }

        /// <summary>
        /// The get trip.
        /// </summary>
        /// <returns>
        /// The <see cref="JsonResult"/>.
        /// </returns>        
        [HttpGet("api/trips")]
        public IActionResult Get(string username)
        {
            var user = User.Identity.Name;
            return this.Ok(this.localRepository.GetByUserName(username));
        }

        //[HttpGet("api/trips/{entityName}/getentitybyname")]
        public IActionResult GetEntityByName(string entityName, string username)
        {
            return this.Ok(this.localRepository.GetEntityByName(entityName, username).Stops.OrderBy(stop => stop.Order));
        }

        /// <summary>
        /// The post.
        /// </summary>
        /// <param name="trip">
        /// The trip.
        /// </param>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        [HttpPost("api/trips")]
        public async Task<IActionResult> Post([FromBody]Trip trip)
        {            
                this.localRepository.AddEntity(trip);           

            if (await this.localRepository.SaveChangesAsync())
                return Created($"api/trips/{trip.Name}", Mapper.Map<TripViewModel>(trip));
            else
                return BadRequest("Failed to store changes to database");
        }

        /// <summary>
        /// The post.
        /// </summary>
        /// <param name="trip">
        /// The trip.
        /// </param>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        [HttpPost("api/trips/update")]
        public async Task<IActionResult> Update([FromBody]Trip trip)
        {
            this.localRepository.UpdateEntity(trip);

            if (await this.localRepository.SaveChangesAsync())
                return Created($"api/trips/{trip.Name}", Mapper.Map<TripViewModel>(trip));
            else
                return BadRequest("Failed to store changes to database");
        }

        /// <summary>
        /// The post.
        /// </summary>
        /// <param name="trip">
        /// The trip.
        /// </param>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        [HttpPost("api/trips/delete")]
        public async Task<IActionResult> Delete([FromBody]Trip trip)
        {
            this.localRepository.DeleteEntity(trip.Id);

            if (await this.localRepository.SaveChangesAsync())
            {              
                return this.Ok(this.localRepository.GetByUserName(trip.Username));
            }
            else
                return BadRequest("Failed to store changes to database");
        }

    }
}