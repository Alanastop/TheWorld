using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheWorld.Services;
using Microsoft.Extensions.Configuration;
using TheWorld.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using TheWorld.Models.Persistent.Interfaces;
using TheWorld.MVC.Models;
using TheWorld.Models.Persistent;
using System.Net.Http;
using Newtonsoft.Json;

namespace TheWorld.MVC.Controllers
{
    public class AppController : Controller
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
            List<Trip> trips = null;
            var httpClient = new HttpClient();
            var response = httpClient.GetAsync("http://localhost:10816/api/trips").Result;
            if (response.IsSuccessStatusCode)
            {
                var stateInfo = response.Content.ReadAsStringAsync().Result;
                trips = JsonConvert.DeserializeObject<List<Trip>>(stateInfo);
                return this.View(trips);
            }
            else 
            {
                this.localLogger.LogError($"Failed to get trips in Index page");
                return this.Redirect("/error'");
            }
        }
    }
}