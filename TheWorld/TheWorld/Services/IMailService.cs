// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMailService.cs" company="DataComm">
//   The World Service
// </copyright>
// <summary>
//   The MailService interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TheWorld.Services
{
    /// <summary>
    /// The MailService interface.
    /// </summary>
    public interface IMailService
    {
        /// <summary>
        /// The send mail.
        /// </summary>
        /// <param name="from">
        /// The from.
        /// </param>
        /// <param name="to">
        /// The to.
        /// </param>
        /// <param name="subject">
        /// The subject.
        /// </param>
        /// <param name="body">
        /// The body.
        /// </param>
        void SendMail(string from, string to, string subject, string body);
    }
}