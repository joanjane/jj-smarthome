﻿using JJ.SmartHome.Core.Alerts;
using JJ.SmartHome.WebApi.ControlPanel.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JJ.SmartHome.WebApi.ControlPanel
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControlPanelController : ControllerBase
    {
        private readonly AlertStatusProvider _alertStatusProvider;

        public ControlPanelController(AlertStatusProvider alertStatusProvider)
        {
            _alertStatusProvider = alertStatusProvider;
        }

        [HttpGet("alarm")]
        public ActionResult<AlarmStatusDetailsDto> GetAlarmStatus()
        {
            return new AlarmStatusDetailsDto
            {
                Status = _alertStatusProvider.GetAlertStatus(),
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
    }
}