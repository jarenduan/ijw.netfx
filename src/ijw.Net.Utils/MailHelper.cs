using System.Net;
using System.Net.Mail;
using System.Text;
#if NET45
using System.Threading.Tasks;
#endif

namespace ijw.Net {
    public class MailHelper
    {
        /// <summary>
        /// 使用SMTP服务器发送邮件
        /// </summary>
        /// <param name="smtpServer">SMTP发送服务器的地址，形如：smtp.163.com</param>
        /// <param name="port">SMTP服务器端口，一般是25</param>
        /// <param name="username">用户名，如abc@163.com</param>
        /// <param name="password">该用户的密码</param>
        /// <param name="from">发送者地址，一般与username相同</param>
        /// <param name="to">发送给谁，如def@qq.com</param>
        /// <param name="displayName">显示的发送人</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="message">正文</param>
        /// <param name="encoding">编码方式，一般采用UTF8</param>
        public void SendEmail(string smtpServer, int port, string username, string password, string from, string to, string displayName, string subject, string message, Encoding encoding) {
#if !NET35
            using (var client = new SmtpClient(smtpServer, port)) {
#else
#pragma warning disable IDE0017 // Simplify object initialization
            var client = new SmtpClient(smtpServer, port);
#pragma warning restore IDE0017 // Simplify object initialization
#endif
            client.Credentials = new NetworkCredential(username, password);
                MailAddress fromAddress = new MailAddress(from, displayName, encoding);
                MailAddress toAddress = new MailAddress(to);
                using (var mailMessage = new MailMessage(fromAddress, toAddress)) {
                    mailMessage.Body = message;
                    mailMessage.BodyEncoding = encoding;
                    mailMessage.Subject = subject;
                    mailMessage.SubjectEncoding = encoding;
                    client.Send(mailMessage);
                }
#if !NET35
            }
#endif
        }

        /// <summary>
        /// 使用SMTP服务器发送邮件。会自动猜测一个smtp发送服务器，使用UTF8发送邮件
        /// </summary>
        /// <param name="username">用户名，如abc@163.com</param>
        /// <param name="password">该用户的密码</param>
        /// <param name="to">发送给谁，如def@qq.com</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="message">正文</param>
        public void SendEmail(string username, string password, string to, string subject, string message) {
            SendEmail(
                getSmtpServerByUsername(username)
                , 25
                , username
                , password
                , username
                , to
                , username
                , subject
                , message
                , Encoding.UTF8
            );
        }

#if NET45
        /// <summary>
        /// 使用SMTP服务器异步发送邮件
        /// </summary>
        /// <param name="smtpServer">SMTP发送服务器的地址，形如：smtp.163.com</param>
        /// <param name="port">SMTP服务器端口，一般是25</param>
        /// <param name="username">用户名，如abc@163.com</param>
        /// <param name="password">该用户的密码</param>
        /// <param name="from">发送者地址，一般与username相同</param>
        /// <param name="to">发送给谁，如def@qq.com</param>
        /// <param name="displayName">显示的发送人</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="message">正文</param>
        /// <param name="encoding">编码方式，一般采用UTF8</param>
        /// <returns>异步任务</returns>
        public async Task SendEmailAsync(string smtpServer, int port, string username, string password, string from, string to, string displayName, string subject, string message, Encoding encoding) {
            using (var client = new SmtpClient(smtpServer, port)) {
                client.Credentials = new NetworkCredential(username, password);
                MailAddress fromAddress = new MailAddress(from, displayName, encoding);
                MailAddress toAddress = new MailAddress(to);
                using (var mailMessage = new MailMessage(fromAddress, toAddress)) {
                    mailMessage.Body = message;
                    mailMessage.BodyEncoding = encoding;
                    mailMessage.Subject = subject;
                    mailMessage.SubjectEncoding = encoding;
                    await client.SendMailAsync(mailMessage);
                }
            }
        }

        /// <summary>
        /// 使用SMTP服务器发送邮件。会自动猜测一个smtp发送服务器，使用UTF8发送邮件
        /// </summary>
        /// <param name="username">用户名，如abc@163.com</param>
        /// <param name="password">该用户的密码</param>
        /// <param name="to">发送给谁，如def@qq.com</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="message">正文</param>
        /// <returns>异步任务</returns>
        public async Task SendEmailAsync(string username, string password, string to, string subject, string message) {
            await SendEmailAsync(
                 getSmtpServerByUsername(username)
                 , 25
                 , username
                 , password
                 , username
                 , to
                 , username
                 , subject
                 , message
                 , Encoding.UTF8
             );
        }
#endif
        /// <summary>
        /// 通过邮件地址，猜测其smtp服务器地址
        /// </summary>
        /// <param name="username">邮件地址，形如abc@163.com</param>
        /// <returns>猜测的smtp服务器地址，形如smtp.163.com</returns>
        private string getSmtpServerByUsername(string email) {
            return "smtp." + email.Remove(0, email.IndexOf("@") + 1);
        }
    }
}
