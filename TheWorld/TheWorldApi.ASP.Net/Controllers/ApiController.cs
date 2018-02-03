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
    #region

    using Microsoft.AspNetCore.Mvc;

    using TheWorld.Models.Persistent;
    using TheWorld.Repositories.Interfaces;

    using TheWorldApi.Controllers.Interfaces;

    #endregion

    /// <summary>
    /// The api controller.
    /// </summary>
    public class ApiController : Controller, IApiController
    {
        /// <summary>
        /// The local repository.
        /// </summary>
        private readonly IEntityRepository localRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiController"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        public ApiController(IEntityRepository repository)
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
        public IActionResult Get()
        {
            return this.Ok(this.localRepository.GetAllTrips());
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
        public IActionResult Post([FromBody]Trip trip)
        {
            if (ModelState.IsValid)
            {
                return Created($"api/trips/{trip.Name}", trip);
            }

            return BadRequest("Bad data");
        }
    }
}