// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAppController.cs" company="DataComm">
//   The World
// </copyright>
// <summary>
//   The AppController interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TheWorld.Controllers.Web.Interfaces
{
    #region

    using Microsoft.AspNetCore.Mvc;

    using TheWorld.ViewModels;

    #endregion

    /// <summary>
    /// The AppController interface.
    /// </summary>
    public interface IAppController 
    {
        /// <summary>
        /// The about.
        /// </summary>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        IActionResult About();

        /// <summary>
        /// The contact.
        /// </summary>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        IActionResult Contact();

        /// <summary>
        /// The contact.
        /// </summary>
        /// <param name="contact">
        /// The contacts.
        /// </param>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        IActionResult Contact(ContactViewModel contact);

        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        IActionResult Index();
    }
}