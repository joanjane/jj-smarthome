using JJ.SmartHome.Core.Alarm;
using JJ.SmartHome.Core.Occupancy.Evaluation;
using JJ.SmartHome.WebApi.ControlPanel.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace JJ.SmartHome.WebApi.ControlPanel
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControlPanelController : ControllerBase
    {
        private readonly AlarmStatusProvider _alertStatusProvider;
        private readonly IOptionsSnapshot<OccupancyDevicesConfiguration> _devicesConfiguration;

        public ControlPanelController(
            AlarmStatusProvider alertStatusProvider, 
            IOptionsSnapshot<OccupancyDevicesConfiguration> devicesConfiguration)
        {
            _alertStatusProvider = alertStatusProvider;
            _devicesConfiguration = devicesConfiguration;
        }

        [HttpGet("alarm")]
        public ActionResult<AlarmStatusDetailsDto> GetAlarmStatus()
        {
            return new AlarmStatusDetailsDto
            {
                Status = _alertStatusProvider.GetAlarmStatus(),
                LastFiredAlert = _alertStatusProvider.GetLastFiredAlert(),
            };
        }

        [HttpPut("alarm")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        public ActionResult<AlarmStatusDto> SetAlarmStatus([FromBody] AlarmStatusDto request)
        {
            _alertStatusProvider.SetAlertStatus(request.Status);
            return request;
        }


        [HttpGet("config/devices")]
        public ActionResult<OccupancyDevicesConfiguration> GetDevicesConfig()
        {
            return _devicesConfiguration.Value;
        }
    }
}
