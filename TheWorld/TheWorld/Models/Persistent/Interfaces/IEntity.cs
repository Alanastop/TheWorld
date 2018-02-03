// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEntity.cs" company="DataComm">
//   The World
// </copyright>
// <summary>
//   The Entity interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TheWorld.Models.Persistent.Interfaces
{
    /// <summary>
    /// The Entity interface.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        string Name { get; set; }
    }
}