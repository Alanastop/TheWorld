//\ --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEntityRepository.cs" company="DataComm">
//   The World
// </copyright>
// <summary>
//   The EntityRepository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TheWorld.Repositories.Interfaces
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TheWorld.Models.Persistent;
    using TheWorld.Models.Persistent.Interfaces;

    /// <summary>
    /// The EntityRepository interface.
    /// </summary>
    public interface IEntityRepository<T> where T :IEntity
    {
        /// <summary>
        /// The get all trips.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        IEnumerable<T> GetByUserName(string username);

        void AddEntity(T entity);

        void UpdateEntity(T entity);

        Task<bool> SaveChangesAsync();

        T GetEntityByName(string EntityName, string username);

       void AddStopByTrip(Trip trip, Stop stop);
        void DeleteEntity(int trip);
    }
}