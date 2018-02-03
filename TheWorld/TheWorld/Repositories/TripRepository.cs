// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorldRepository.cs" company="">
//   
// </copyright>
// <summary>
//   The world repository.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TheWorld.Repositories
{
    using System;
    #region      
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Extensions.Logging;

    using TheWorld.DbContext;
    using TheWorld.Models.Persistent;
    using TheWorld.Repositories.Interfaces;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    #endregion

    /// <summary>
    /// The world repository.
    /// </summary>
    public class TripRepository : IEntityRepository<Trip>
    {
        /// <summary>
        /// The local context.
        /// </summary>
        private readonly WorldContext localContext;

        /// <summary>
        /// The local logger.
        /// </summary>
        private readonly ILogger<TripRepository> localLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TripRepository"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public TripRepository(WorldContext context, ILogger<TripRepository> logger)
        {
            this.localContext = context;
            this.localLogger = logger;
        }

        public void AddEntity(Trip trip)
        {           
            localContext.Add(trip);            
        }

        public void DeleteEntity(int id)
        {
            var tripToBeDeleted = this.localContext.Trips.Include(t=>t.Stops).Where(trips => trips.Id == id).FirstOrDefault();
            foreach(var stop in tripToBeDeleted.Stops)
            {
                localContext.Stops.Remove(stop);
            }

            localContext.Trips.Remove(tripToBeDeleted);
        }

        public void AddStopByTrip(Trip trip, Stop stop)
        {
            throw new NotImplementedException();
        }        

        /// <summary>
        /// The get all trips.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public IEnumerable<Trip> GetByUserName(string username)
        {
            try
            {
                this.localLogger.LogInformation("Getting all trips from the Database");
                return this.localContext.Trips.Where(trip => trip.Username == username).ToList();
            }
            catch (SqlException ex)
            {
                this.localLogger.LogError($"Failed to get all trips: {ex}");
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                this.localLogger.LogError($"Failed to get all trips: {ex}");
                throw new Exception(ex.Message);
            }
        }

        public Trip GetEntityByName(string tripName, string username)
        {
            try
            {
                this.localLogger.LogInformation($"Getting trip {tripName} from the Database");
                return this.localContext.Trips
                    .Include(trip=>trip.Stops)
                    .Where(trip => (trip.Name == tripName) && (trip.Username == username)).FirstOrDefault();
            }
            catch (SqlException ex)
            {
                this.localLogger.LogError($"Failed to get trip {tripName} {ex}");
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                this.localLogger.LogError($"Failed to get trips {tripName} {ex}");
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await localContext.SaveChangesAsync())> 0;
        }

        public void UpdateEntity(Trip localTrip)
        {
            var databaseTrip = this.localContext.Trips.SingleOrDefault(trip => trip.Id == localTrip.Id);
            if (databaseTrip == null)
                throw new ArgumentException();

            databaseTrip.Name = localTrip.Name;            
        }
    }
}