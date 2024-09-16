using Highgeek.McWebApp.Common.Helpers;
using Highgeek.McWebApp.Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class NotificationApi : Controller
    {
        private readonly IEmailSender _emailSender;

        private readonly string _email;

        public NotificationApi(IEmailSender emailSender)
        {
            _emailSender = emailSender;
            _email = ConfigProvider.GetConfigString("NotificationSettings:ToSend");
        }


        [AllowAnonymous]
        [Route("api/notification/gameserver/{code}")]
        [HttpGet]
        public async Task<IActionResult> Get(string code)
        {
            switch (code)
            {
                case "1":
                    await _emailSender.SendEmailAsync(_email, "Notification API endpoint hit! code: " + code, code);
                    return Ok();
                case "2":
                    await _emailSender.SendEmailAsync(_email, "Notification API endpoint hit! code: " + code, code);
                    return Ok();
                case "3":
                    await _emailSender.SendEmailAsync(_email, "Notification API endpoint hit! code: " + code, code);
                    return Ok();

                default:
                    return NotFound();

            }
        }
    }
}
