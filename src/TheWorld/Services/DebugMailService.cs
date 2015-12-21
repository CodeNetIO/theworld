using System;
using System.Diagnostics;

namespace TheWorld.Services
{
    public class DebugMailService : IMailService
    {
        #region Implementation of IMailService

        public bool SendMail(string to, string @from, string subject, string body)
        {
            Debug.WriteLine($"Sending mail: To: {to}, Subject: {subject}");
            return true;
        }

        #endregion
    }

}
