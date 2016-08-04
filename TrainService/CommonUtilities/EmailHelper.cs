#region Using

using System.Collections.Generic;
using System.IO;
using System.Net.Mail;

#endregion


namespace CommonUtilities
{
    public static class EmailHelper
    {
        public static void SendEmail(string sender, IEnumerable<string> to, IEnumerable<string> cc, IEnumerable<string> bcc, string subject, string body,
            IEnumerable<string> archives, bool isBodyHtml, MailPriority priority, IEnumerable<string> redirectTo)
        {
            var msg = new MailMessage();
            if (to != null)
            {
                foreach (var t in to)
                {
                    if (!string.IsNullOrWhiteSpace(t))
                    {
                        msg.To.Add(FormatEmailAddress(t));
                    }
                }
            }

            if (cc != null)
            {
                foreach (var c in cc)
                {
                    if (!string.IsNullOrWhiteSpace(c))
                    {
                        msg.CC.Add(FormatEmailAddress(c));
                    }
                }
            }

            if (bcc != null)
            {
                foreach (var c in bcc)
                {
                    if (!string.IsNullOrWhiteSpace(c))
                    {
                        msg.Bcc.Add(FormatEmailAddress(c));
                    }
                }
            }
            msg.Subject = subject;
            msg.Body = body;
            if (archives != null)
            {
                foreach (var archive in archives)
                {
                    if (File.Exists(archive))
                    {
                        msg.Attachments.Add(new Attachment(archive));
                    }
                }
            }
            msg.IsBodyHtml = isBodyHtml;
            if (redirectTo != null)
            {
                foreach (var r in redirectTo)
                {
                    msg.ReplyToList.Add(FormatEmailAddress(r));
                }
            }
            msg.From = FormatEmailAddress(sender);
            msg.Priority = priority;
            SendEmail(msg);
        }

        private static void SendEmail(MailMessage msg)
        {

            var client = new SmtpClient();
            client.Host = "10.193.236.12";
            client.Send(msg);

        }

        private static MailAddress FormatEmailAddress(string aliseOrAddress)
        {
            if (aliseOrAddress.Contains("@"))
            {
                return new MailAddress(aliseOrAddress);
            }
            return new MailAddress(aliseOrAddress + "@motorolasolutions.com");
        }
    }
}
