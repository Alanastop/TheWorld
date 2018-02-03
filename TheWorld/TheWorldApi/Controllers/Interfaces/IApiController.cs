// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IApiController.cs" company="DataComm">
//   The World Api
// </copyright>
// <summary>
//   The ApiController interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TheWorldApi.Controllers.Interfaces
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using TheWorld.Models.Persistent;
    using TheWorld.Models.Persistent.Interfaces;

    /// <summary>
    /// The ApiController interface.
    /// </summary>
    public interface IApiController<T> where T : IEntity
    {
        /// <summary>
        /// The get.
        /// </summary>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        IActionResult Get(string username);

        /// <summary>
        /// The post.
        /// </summary>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        Task<IActionResult> Post(T trip);

        /// <summary>
        /// The post.
        /// </summary>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        Task<IActionResult> Delete(T trip);

        IActionResult GetEntityByName(string entityName, string username);
    }
}