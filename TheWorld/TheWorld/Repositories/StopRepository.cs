using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TheWorld.DbContext;
using TheWorld.Models.Persistent;
using TheWorld.Repositories.Interfaces;

namespace TheWorld.Repositories
{
    public class StopRepository : IEntityRepository<Stop>
    {
        private readonly WorldContext localContext;

        private readonly ILogger<StopRepository> localLogger;

        public StopRepository(WorldContext context, ILogger<StopRepository> logger)
        {
            this.localContext = context;
            this.localLogger = logger;
        }

        public void AddEntity(Stop stop)
        {
            this.localContext.Add(stop);
        }

        public void AddStopByTrip(Trip trip, Stop stop)
        {
            trip.Stops.Add(stop);
            this.localContext.Update<Trip>(trip);
        }

        public void DeleteEntity(int id)
        {
            var localstop = this.localContext.Stops.SingleOrDefault(stop => stop.Id == id);
            if (localstop == null)
                throw new ArgumentException();

            this.localContext.Stops.Remove(localstop);
        }

        public IEnumerable<Stop> GetByUserName(string username)
        {
            try
            {
                this.localLogger.LogInformation("Getting all stops from the Database");
                return this.localContext.Stops.ToList();
            }
            catch (SqlException ex)
            {
                this.localLogger.LogError($"Failed to get all stops: {ex}");
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                this.localLogger.LogError($"Failed to get all stops: {ex}");
                throw new Exception(ex.Message);
            }
        }

        public Stop GetEntityByName(string stopName, string username)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await localContext.SaveChangesAsync()) > 0;
        }

        public void UpdateEntity(Stop localStop)
        {
            var databaseStop = this.localContext.Stops.SingleOrDefault(stop => stop.Id == localStop.Id);
            if (databaseStop == null)
                throw new ArgumentException();

            databaseStop.Name = localStop.Name;
            databaseStop.Arrival = localStop.Arrival;
        }
    }
}
