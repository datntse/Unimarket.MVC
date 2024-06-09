
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace Unimarket.MVC.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
    public class MailService : IMailService
    {
        public MailService()
        {
            
        }
  
        public  Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailToSend = new MimeMessage();
            emailToSend.From.Add(new MailboxAddress("Unimarket", "minhtam250102@gmail.com"));
            emailToSend.To.Add(MailboxAddress.Parse(email));
            emailToSend.Subject = subject;

            string htmlBody = @"
            <!DOCTYPE html>
            <html lang=""en"">

            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            </head>

            <body>
                <div id=""wrapper"" style=""display: flex; align-items: center; justify-content: center; margin: 0px auto; max-width:600px;"">
                    <div class=""form"" style=""width: 600px; height: 450px; border: 1px solid black; border-radius: 10px;"">
                        <div class=""header"" style=""border: 1px solid gray; border-radius: 10px; background-color: #EFC15F;"">
                            <div class=""logo"" style=""height: 90px; width: 200px; background-color: white; margin: 10px auto; border-radius: 10px; display: flex; align-items: center; justify-content: center;"">
                                <img src=""https://firebasestorage.googleapis.com/v0/b/bmosfile.appspot.com/o/Logo.png?alt=media&token=bec2eac4-0dea-4f53-b881-4dda53481ae5"" alt=""Logo"" srcset="""" style=""width: 100px; height: 100%; margin: 0 auto;"">
                            </div>
                            <div class=""text"" style=""text-align: center;"">
                                <h1>Chào mừng bạn đến với Unimarket</h1>
                                <div class=""link"" style=""width: 150px; height: 30px; background-color: aliceblue; border-radius: 5px; margin: 0 auto; display: flex; align-items: center; justify-content: center; border: 1px solid black;"">"
                                + htmlMessage +
                                @"</div>
                                <div class=""headertext"" style=""margin-top: 5px; margin-bottom: 10px;"">
                                    <span>(Chúng tôi cần xác thực địa chỉ email của bạn để kích hoạt tài khoản)</span>
                                </div>
                            </div>
                        </div>

                        <div class=""footer"" style=""display: flex; align-items: center; justify-content: center;"">
                            <div class=""footercontent"" style=""width: 480px; text-align: center; margin: 0px auto;"">
                                <div class=""span1"" style=""margin-top: 10px; margin-bottom: 10px;"">
                                    <span>Copyright © 2024 Unimarket, Allrights reserved.</span>
                                </div>
                                <div><b>Địa chỉ của chúng tôi:</b></div>
                                <div class=""span2"" style=""margin-top: 10px;"">
                                    <span>C11-01, số 473 Man Thiện, Thủ Đức, KDT Geleximco Lê Trọng Tấn, Phường Dương Nội, Quận Hà Đông, Hà Nội</span></div>
                                <div class=""span3"" style=""margin-top: 10px;"">
                                    <span>Email: Unimarket8888@gmail.com</span>
                                </div>
                                <div class=""span4"" style=""margin-top: 10px;""><span>Hotline: 0979500611</span></div>
                            </div>
                         </div>
                      </div>
                   </div>
               </body>
            </html>";

            emailToSend.Body = new TextPart(TextFormat.Html)
            {
                Text = htmlBody
            };
            using (var emailClient = new SmtpClient())
            {
                emailClient.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                emailClient.Authenticate("minhtam250102@gmail.com", "ihrhvyplgkonevqy");
                emailClient.Send(emailToSend);
                emailClient.Disconnect(true);
            }
            return Task.CompletedTask;
        }
    }
}
