using JJ.SmartHome.Core.Alerts;
using JJ.SmartHome.Core.Alerts.Queries;
using JJ.SmartHome.Core.Notifications;
using JJ.SmartHome.Db;
using JJ.SmartHome.Db.Entities;
using JJ.SmartHome.Tests.Mocks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;

using System.Threading.Tasks;

namespace JJ.SmartHome.Tests.Builders
{ 
    public class OccupancyAlertServiceBuilder
    {
        public const string OccupancyTopic = "occupancy/test";

        public MqttClientMock MqttClient = new MqttClientMock();
        public ILastFiredAlertQuery LastFiredAlertQuery = Substitute.For<ILastFiredAlertQuery>();
        public IAlertsStore AlertsStore = Substitute.For<IAlertsStore>();
        public IAlertNotifier AlertNotifier = Substitute.For<IAlertNotifier>();
        private AlertsOptions AlertsOptions = new AlertsOptions() 
        {
            OccupancyTopic = OccupancyTopic
        };

        public OccupancyAlertServiceBuilder WithLastFiredAlert(DateTimeOffset lastFiredAlert)
        {
            LastFiredAlertQuery.CheckLastFiredAlertDate().Returns(
                Task.FromResult<DateTimeOffset?>(lastFiredAlert)
            );
            return this;
        }

        public OccupancyAlertServiceBuilder WithSnoozeAlertPeriod(TimeSpan period)
        {
            this.AlertsOptions.SnoozePeriodAfterAlerting = period;

            return this;
        }
        
        public OccupancyAlertServiceBuilder WithWriteMeasureSucceeded()
        {
            AlertsStore.WriteMeasure(Arg.Any<AlertMeasure>()).Returns(Task.CompletedTask);

            return this;
        }
        public OccupancyAlertServiceBuilder WithAlertNotifySucceeded()
        {
            AlertNotifier.Notify(Arg.Any<string>(), Arg.Any<string>()).Returns(Task.CompletedTask);

            return this;
        }
        

        public OccupancyAlertService Build()
        {
            var sut = new OccupancyAlertService(
                MqttClient,
                Options.Create(AlertsOptions),
                AlertsStore,
                LastFiredAlertQuery,
                AlertNotifier,
                new AlertStatusProvider(Options.Create(AlertsOptions)),
                LoggerFactory.Create(c => c.AddConsole()).CreateLogger<OccupancyAlertService>()
            );

            return sut;
        }
    }
}