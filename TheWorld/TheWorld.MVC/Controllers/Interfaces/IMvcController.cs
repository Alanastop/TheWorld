using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWorld.MVC.Models;
using TheWorld.MVC.Models.Interfaces;

namespace TheWorld.MVC.Controllers.Interfaces
{
    interface IMvcController<T> where T : IViewModel
    {
        /// <summary>
        /// The get.
        /// </summary>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        IActionResult Get();

        /// <summary>
        /// The get.
        /// </summary>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        Task<IActionResult> PostAsync(T trip);

        IActionResult GetEntityByName(string entityName);
    }
}
