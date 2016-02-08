using System.Diagnostics.Contracts;
using System.Net;
using System.Net.Configuration;
using Serilog.Sinks.Email;

namespace DotNetRevolution.Logging.Serilog.Helper
{
    public static class SerilogSmtpHelper
    {
        public static EmailConnectionInfo GetEmailConnectionInfo(SmtpSection smtpSection, string subject, string fromEmail, string toEmail)
        {
            Contract.Requires(smtpSection != null);
            Contract.Requires(smtpSection.Network != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(subject));
            Contract.Requires(!string.IsNullOrWhiteSpace(toEmail));
            Contract.Ensures(Contract.Result<EmailConnectionInfo>() != null);

            var info = new EmailConnectionInfo
            {
                Port = smtpSection.Network.Port,
                MailServer = smtpSection.Network.Host,
                FromEmail = fromEmail,
                EnableSsl = smtpSection.Network.EnableSsl,
                EmailSubject = subject,
                ToEmail = toEmail
            };

            if (!smtpSection.Network.DefaultCredentials)
            {
                info.NetworkCredentials = new NetworkCredential(smtpSection.Network.UserName, smtpSection.Network.Password);
            }
            
            return info;
        }
    }
}
