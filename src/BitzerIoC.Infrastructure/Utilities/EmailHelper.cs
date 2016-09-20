using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using BitzerIoC.Infrastructure.AppConstants;
using System.Diagnostics;
using MimeKit;
using MailKit.Net.Smtp;

namespace BitzerIoC.Infrastructure.Utilities
{
    /// <summary>
    /// Helper class to send email
    /// </summary>
    public class EmailHelper
    {
        #region properties
        private string _to { get; set; }
        private string _from { get; set; }
        private string _subject { get; set; }
        private string _plainTextMesage { get; set; }
        private string _htmlMessage { get; set; }
        private string _replyTo { get; set; }
        #endregion

        /// <summary>
        /// Parameterized Constructor :: need these parameters to send an email
        /// </summary>
        /// <param name="to">Reciever email address</param>
        /// <param name="from">Sender email address</param>
        /// <param name="subject">Subject of an email</param>
        /// <param name="plainTextMesage">If it is plain message</param>
        /// <param name="htmlMessage">If it is Html base message</param>
        /// <param name="replyTo">Optional</param>
        public EmailHelper(string to, string from, string subject, string plainTextMesage, string htmlMessage, string replyTo = null)
        {
            _to = to;
            _from = from;
            _subject = subject;
            _plainTextMesage = plainTextMesage;
            _htmlMessage = htmlMessage;
            _replyTo = replyTo;

        }

        /// <summary>
        /// Send an Email
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SendEmailAsync()
        {
            try
            {
                EmailSender oEmailSender = new EmailSender();
                var result = await oEmailSender.SendEmailAsync(null, _to, _from, _subject, _plainTextMesage, _htmlMessage, _replyTo);
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error:" + ex.Message);
                throw;
            }
        }


    }

    /// <summary>
    /// Options for SMTP
    /// </summary>
    public class SmtpOptions
    {
        public string Server { get; set; } = "smtp.gmail.com";
        public int Port { get; set; } = 587;
        public string User { get; set; } = "solidchips.gshare@gmail.com";
        public string Password { get; set; } = "P@kistan1";
        public bool UseSsl { get; set; } = false;
        public bool RequiresAuthentication { get; set; } = true;
        public string PreferredEncoding { get; set; } = string.Empty;
    }

    public class EmailSender
    {
        SmtpOptions defaultOptions = null;
        public EmailSender()
        {
            defaultOptions = new SmtpOptions();
        }

        /// <summary>
        /// Send email to single recipent
        /// </summary>
        /// <param name="smtpOptions">Pass null for default parameters</param>
        /// <param name="to">Email Address</param>
        /// <param name="from">Email Address</param>
        /// <param name="subject">Email Subject</param>
        /// <param name="plainTextMessage">If plain text message</param>
        /// <param name="htmlMessage">If html message</param>
        /// <param name="replyTo">Reply to email address</param>
        /// <returns></returns>
        public async Task<bool> SendEmailAsync(SmtpOptions smtpOptions, string to, string from,
                                         string subject, string plainTextMessage, string htmlMessage,
                                         string replyTo = null)
        {

            var hasPlainText = !string.IsNullOrWhiteSpace(plainTextMessage);
            var hasHtml = !string.IsNullOrWhiteSpace(htmlMessage);
            var message = new MimeMessage();

            #region Default Configuration
            if (smtpOptions == null)
            {
                smtpOptions = defaultOptions;
            }
            #endregion

            #region Argument Exceptions
            if (string.IsNullOrWhiteSpace(to))
            {
                throw new ArgumentException("no to address provided");
            }

            if (string.IsNullOrWhiteSpace(from))
            {
                throw new ArgumentException("no from address provided");
            }

            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentException("no subject provided");
            }

            if (!hasPlainText && !hasHtml)
            {
                throw new ArgumentException("no message provided");
            }
            #endregion

            message.From.Add(new MailboxAddress("", from));
            if (!string.IsNullOrWhiteSpace(replyTo))
            {
                message.ReplyTo.Add(new MailboxAddress("", replyTo));
            }
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;

            //m.Importance = MessageImportance.Normal;
            //Header h = new Header(HeaderId.Precedence, "Bulk");
            //m.Headers.Add()

            BodyBuilder bodyBuilder = new BodyBuilder();
            if (hasPlainText)
            {
                bodyBuilder.TextBody = plainTextMessage;
            }

            if (hasHtml)
            {
                bodyBuilder.HtmlBody = htmlMessage;
            }

            message.Body = bodyBuilder.ToMessageBody();

            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(
                        smtpOptions.Server,
                        smtpOptions.Port,
                        smtpOptions.UseSsl)
                        .ConfigureAwait(false);

                    // Note: since we don't have an OAuth2 token, disable
                    // the XOAUTH2 authentication mechanism.
                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    // Note: only needed if the SMTP server requires authentication
                    if (smtpOptions.RequiresAuthentication)
                    {
                        await client.AuthenticateAsync(smtpOptions.User, smtpOptions.Password)
                            .ConfigureAwait(false);
                    }

                    await client.SendAsync(message).ConfigureAwait(false);
                    await client.DisconnectAsync(true).ConfigureAwait(false);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error:" + ex.Message);
                throw;
            }

        }



        /// <summary>
        ///  Send email to multiple recipent
        /// </summary>
        /// <param name="smtpOptions">Pass null for default parameters</param>
        /// <param name="toCsv">Comma seperated multiple email addresses</param>
        /// <param name="from">Email Address</param>
        /// <param name="subject">Email Subject</param>
        /// <param name="plainTextMessage">If plain text message</param>
        /// <param name="htmlMessage">If html message</param>
        /// <returns></returns>
        public async Task SendMultipleEmailAsync(SmtpOptions smtpOptions, string toCsv, string from,
                                                string subject, string plainTextMessage, string htmlMessage)
        {

            if (smtpOptions == null)
            {
                smtpOptions = defaultOptions;
            }

            if (string.IsNullOrWhiteSpace(toCsv))
            {
                throw new ArgumentException("no to addresses provided");
            }

            if (string.IsNullOrWhiteSpace(from))
            {
                throw new ArgumentException("no from address provided");
            }

            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentException("no subject provided");
            }

            var hasPlainText = !string.IsNullOrWhiteSpace(plainTextMessage);
            var hasHtml = !string.IsNullOrWhiteSpace(htmlMessage);

            if (!hasPlainText && !hasHtml)
            {
                throw new ArgumentException("no message provided");
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("", from));
            string[] addresses = toCsv.Split(',');

            foreach (string item in addresses)
            {
                if (!string.IsNullOrEmpty(item)) { message.To.Add(new MailboxAddress("", item)); ; }
            }

            message.Subject = subject;
            message.Importance = MessageImportance.High;

            BodyBuilder bodyBuilder = new BodyBuilder();
            if (hasPlainText)
            {
                bodyBuilder.TextBody = plainTextMessage;
            }

            if (hasHtml)
            {
                bodyBuilder.HtmlBody = htmlMessage;
            }

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(
                    smtpOptions.Server,
                    smtpOptions.Port,
                    smtpOptions.UseSsl).ConfigureAwait(false);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                if (smtpOptions.RequiresAuthentication)
                {
                    await client.AuthenticateAsync(
                        smtpOptions.User,
                        smtpOptions.Password).ConfigureAwait(false);
                }

                await client.SendAsync(message).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }

        }


    }

}
