// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Trip.cs" company="DataComm">
//   The World
// </copyright>
// <summary>
//   The trip.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TheWorld.Models.Persistent
{
    #region

    using System;
    using System.Collections.Generic;

    using TheWorld.Models.Persistent.Interfaces;

    #endregion

    /// <summary>
    /// The trip.
    /// </summary>
    public class Trip : IEntity
    {
        /// <summary>
        /// Gets or sets the date time.
        /// </summary>
        public DateTime DateCreated { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the stops.
        /// </summary>
        public ICollection<Stop> Stops { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }
    }
}