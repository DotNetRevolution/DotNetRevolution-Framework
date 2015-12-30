using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using Serilog.Sinks.Email;
using Serilog.Sinks.PeriodicBatching;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Logging.Serilog.Sink
{
    public class EmailSink : PeriodicBatchingSink
    {
        private readonly EmailConnectionInfo _connectionInfo;
        private readonly SmtpClient _smtpClient;
        private readonly ITextFormatter _textFormatter;
        
        public EmailSink(EmailConnectionInfo connectionInfo) 
            : base(100, TimeSpan.FromSeconds(30.0))
        {
            Contract.Requires(connectionInfo != null);

            _connectionInfo = connectionInfo;
            _textFormatter = new MessageTemplateTextFormatter(Logger.Template, null);
            _smtpClient = CreateSmtpClient();
            _smtpClient.SendCompleted += SendCompletedCallback;
        }
        
        private SmtpClient CreateSmtpClient()
        {
            Contract.Ensures(Contract.Result<SmtpClient>() != null);

            var client = new SmtpClient();

            if (!string.IsNullOrWhiteSpace(_connectionInfo.MailServer))
            {
                if (_connectionInfo.NetworkCredentials == null)
                {
                    client.UseDefaultCredentials = true;
                }
                else
                {
                    client.Credentials = _connectionInfo.NetworkCredentials;
                }

                SetHost(client);
                SetPort(client);
                
                client.EnableSsl = _connectionInfo.EnableSsl;
            }

            return client;
        }

        private void SetHost(SmtpClient client)
        {
            Contract.Requires(client != null);

            var host = _connectionInfo.MailServer;
            Contract.Assume(host != null);

            client.Host = host;
        }

        private void SetPort(SmtpClient client)
        {
            Contract.Requires(client != null);

            var port = _connectionInfo.Port;
            Contract.Assume(port >= 0 && port <= 65535);

            client.Port = port;
        }

        protected async override Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            var payload = new StringWriter();

            foreach (var logEvent in events)
            {
                _textFormatter.Format(logEvent, payload);
            }

            var message = new MailMessage
                {
                    From = new MailAddress(_connectionInfo.FromEmail),
                    Subject = _connectionInfo.EmailSubject,
                    Body = payload.ToString(),
                    BodyEncoding = Encoding.UTF8,
                    SubjectEncoding = Encoding.UTF8
                };

            foreach (var str in _connectionInfo.ToEmail.Split(",;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                message.To.Add(str);
            }

            await _smtpClient.SendMailAsync(message);
        }
        
        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                SelfLog.WriteLine("Received failed result {0}: {1}", "Cancelled", e.Error);
            }

            if (e.Error != null)
            {
                SelfLog.WriteLine("Received failed result {0}: {1}", "Error", e.Error);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                Contract.Assume(_smtpClient != null);

                _smtpClient.Dispose();
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_connectionInfo != null);
        }
    }
}
