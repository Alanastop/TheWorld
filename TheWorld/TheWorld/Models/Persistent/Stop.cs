// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Stop.cs" company="">
//   
// </copyright>
// <summary>
//   The stop.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TheWorld.Models.Persistent
{
    #region

    using System;

    using TheWorld.Models.Persistent.Interfaces;

    #endregion

    /// <summary>
    /// The stop.
    /// </summary>
    public class Stop : IEntity
    {
        /// <summary>
        /// Gets or sets the arrival.
        /// </summary>
        public DateTime Arrival { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

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
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        public int Order { get; set; }
    }
}