using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace Fullstockcode.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoveController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public LoveController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost("unlock")]
        public IActionResult Unlock([FromForm] string password)
        {
            if (password == "papa")
            {
                return Ok(new
                {
                    success = true,
                    message = "Unlocked! 💖"
                });
            }

            return Unauthorized(new
            {
                success = false,
                message = "Wrong Password ❌"
            });
        }
        [HttpPost("send-mail")]
        public async Task<IActionResult> SendLoveMail([FromForm] string Email)
        {
            if (string.IsNullOrEmpty(Email))
                return BadRequest(new { success = false, message = "Email is required" });

            try
            {
                // Build dynamic unlock URL
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                var deployUrl = $"{baseUrl}/api/Love/unlock";

                var body = $@"
                    <h2>Hey My Love 💖</h2>
                    <p>Click below to unlock something special 😘</p>
                    <a href='{deployUrl}'>Open Now 💌</a>";

                // Get SMTP settings from appsettings.json
                var smtpHost = _configuration["EmailSettings:Host"];
                var smtpPort = int.Parse(_configuration["EmailSettings:Port"]);
                var senderEmail = _configuration["EmailSettings:Email"];
                var senderPassword = _configuration["EmailSettings:Password"].Trim(); // remove accidental spaces or commas

                using (var smtp = new SmtpClient(smtpHost, smtpPort))
                {
                    smtp.Credentials = new NetworkCredential(senderEmail, senderPassword);
                    smtp.EnableSsl = true; // TLS required by Gmail

                    var message = new MailMessage
                    {
                        From = new MailAddress(senderEmail),
                        Subject = "A Surprise For You 💖",
                        Body = body,
                        IsBodyHtml = true
                    };
                    message.To.Add(Email);

                    await smtp.SendMailAsync(message);
                }

                return Ok(new
                {
                    success = true,
                    message = "Mail Sent Successfully 💌"
                });
            }
            catch (SmtpException smtpEx)
            {
                return BadRequest(new
                {
                    success = false,
                    message = $"SMTP Error: {smtpEx.Message}"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = $"Error sending email: {ex.Message}"
                });
            }
        }
    }
}
