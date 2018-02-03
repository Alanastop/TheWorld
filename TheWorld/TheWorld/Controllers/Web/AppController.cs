// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppController.cs" company="DataComm">
//   The World
// </copyright>
// <summary>
//   The app controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TheWorld.Controllers.Web
{
    #region

    using System;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using TheWorld.Controllers.Web.Interfaces;
    using TheWorld.DbContext;
    using TheWorld.Repositories.Interfaces;
    using TheWorld.Services;
    using TheWorld.ViewModels;
    using TheWorld.Models.Persistent;
    using TheWorld.Models.Persistent.Interfaces;

    #endregion

    /// <summary>
    /// The app controller.
    /// </summary>
    public class AppController : Controller, IAppController
    {
        /// <summary>
        /// The email service.
        /// </summary>
        private readonly IMailService emailService;

        /// <summary>
        /// The config local.
        /// </summary>
        private readonly IConfigurationRoot configLocal;

        /// <summary>
        /// The local repository.
        /// </summary>
        private readonly IEntityRepository<IEntity> localRepository;

        /// <summary>
        /// The local logger.
        /// </summary>
        private readonly ILogger<AppController> localLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppController"/> class.
        /// </summary>
        /// <param name="mailService">
        /// The mail service.
        /// </param>
        /// <param name="config">
        /// The config.
        /// </param>
        /// <param name="repository">
        /// The world Context.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public AppController(IMailService mailService, IConfigurationRoot config, IEntityRepository<IEntity> repository, ILogger<AppController> logger)
        {
            this.configLocal = config;
            this.localRepository = repository;
            this.emailService = mailService;
            this.localLogger = logger;
        }

        /// <summary>
        /// The aboout.
        /// </summary>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        public IActionResult About()
        {
            return this.View();
        }

        /// <summary>
        /// The contact.
        /// </summary>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        public IActionResult Contact()
        {
            return this.View();
        }

        /// <summary>
        /// The contact.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (model.Email.Contains("aol.com"))
            {
                this.ModelState.AddModelError("Email", "We don't support AOL addresses");
                if (this.ViewBag.UserMessage == "Message sent")
                {
                    ModelState.Clear();
                }
            }

            if (this.ModelState.IsValid)
            {
                this.emailService.SendMail(
                    this.configLocal["MailSettings:ToAddress"],
                    model.Email,
                    "From The World",
                    model.Message);
                ModelState.Clear();
                this.ViewBag.UserMessage = "Message sent";
            }

            return this.View();
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        public IActionResult Index()
        {
            try
            {
                string username = "";
                var data = this.localRepository.GetByUserName(username);
                return this.View(data);
            }
            catch (Exception ex)
            {
                this.localLogger.LogError($"Failed to get trips in Index page : {ex.Message}");
                return this.Redirect("/error'");
            }
        }
    }
}