// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IApiController.cs" company="DataComm">
//   The World Api
// </copyright>
// <summary>
//   The ApiController interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Web.Mvc;

namespace TheWorldApi.ASP.Net.Controllers.Interfaces
{
    /// <summary>
    /// The ApiController interface.
    /// </summary>
    public interface IApiController
    {
        /// <summary>
        /// The get.
        /// </summary>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        ActionResult Get();
    }
}