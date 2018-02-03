// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactViewModel.cs" company="">
//   
// </copyright>
// <summary>
//   The contact view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TheWorld.ViewModels
{
    #region

    using System.ComponentModel.DataAnnotations;

    #endregion

    /// <summary>
    /// The contact view model.
    /// </summary>
    public class ContactViewModel
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        [Required]
        [StringLength(4096, MinimumLength = 10)]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}