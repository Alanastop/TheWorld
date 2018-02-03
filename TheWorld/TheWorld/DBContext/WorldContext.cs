// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorldContext.cs" company="DataComm">
//   The World
// </copyright>
// <summary>
//   The world context.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TheWorld.DbContext
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    #region

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using TheWorld.Models;
    using TheWorld.Models.Persistent;

    #endregion

    /// <summary>
    /// The world context.
    /// </summary>
    public class WorldContext : IdentityDbContext<WorldUser>
    {
        /// <summary>
        /// The local root.
        /// </summary>
        private readonly IConfiguration localRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldContext"/> class.
        /// </summary>
        /// <param name="config">
        /// The config.
        /// </param>
        /// <param name="options">
        /// The options.
        /// </param>
        public WorldContext(IConfiguration config, DbContextOptions options)
            : base(options)
        {
            this.localRoot = config;
        }

        /// <summary>
        /// Gets or sets the stops.
        /// </summary>
        public DbSet<Stop> Stops { get; set; }

        /// <summary>
        /// Gets or sets the trips.
        /// </summary>
        public DbSet<Trip> Trips { get; set; }

        /// <summary>
        /// The on configuring.
        /// </summary>
        /// <param name="optionsBuilder">
        /// The options builder.
        /// </param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=10.17.70.20;Initial Catalog=TheWorldDb;Integrated Security=True; MultipleActiveResultSets=true;"); //this.localRoot["ConnectionStrings:WorldContextConnection"]);
        }
    }
}