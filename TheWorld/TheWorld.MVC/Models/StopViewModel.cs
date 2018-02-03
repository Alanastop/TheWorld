using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TheWorld.MVC.Models.Interfaces;

namespace TheWorld.MVC.Models
{
    public class StopViewModel : IViewModel
    {
        [Required]
        public DateTime Arrival { get; set; }
               
        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longtitude.
        /// </summary>
        public double Longtitude { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        //[Required]
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }
    }
}
