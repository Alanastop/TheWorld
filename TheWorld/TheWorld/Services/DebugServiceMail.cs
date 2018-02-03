// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DebugServiceMail.cs" company="DataComm">
//   The World Service
// </copyright>
// <summary>
//   The debug service mail.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TheWorld.Services
{
    #region

    using System.Diagnostics;

    #endregion

    /// <summary>
    /// The debug service mail.
    /// </summary>
    public class DebugServiceMail : IMailService
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
        public void SendMail(string from, string to, string subject, string body)
        {
            Debug.WriteLine($"Sending Email: To: {to} From: {from} Subject: {subject} ");
        }
    }
}