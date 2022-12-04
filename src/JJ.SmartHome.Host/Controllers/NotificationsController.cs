//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Threading.Tasks;
//using JJ.SmartHome.Notifications.NotificationHub.Dto;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;

//namespace JJ.SmartHome.Host.Controllers
//{
//    [ApiController]
//    [Route("[controller]")]
//    public class NotificationsController : ControllerBase
//    {
//        [HttpPut("installations")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
//        public async Task<IActionResult> UpdateInstallation(
//            [Required] DeviceInstallation deviceInstallation)
//        {
//            var success = await _notificationService
//                .CreateOrUpdateInstallationAsync(deviceInstallation, HttpContext.RequestAborted);

//            if (!success)
//                return new UnprocessableEntityResult();

//            return new OkResult();
//        }

//        [HttpDelete("installations/{installationId}")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
//        public async Task<ActionResult> DeleteInstallation(
//            [Required][FromRoute] string installationId)
//        {
//            var success = await _notificationService
//                .DeleteInstallationByIdAsync(installationId, CancellationToken.None);

//            if (!success)
//                return new UnprocessableEntityResult();

//            return new OkResult();
//        }
//    }
//}
