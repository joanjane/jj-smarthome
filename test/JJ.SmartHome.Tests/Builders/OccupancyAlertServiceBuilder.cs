using JJ.SmartHome.Notifications;
using JJ.SmartHome.Db;
using JJ.SmartHome.Db.Entities;
using JJ.SmartHome.Tests.Mocks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Threading.Tasks;
using JJ.SmartHome.Core.Occupancy;
using JJ.SmartHome.Core.Alarm.Queries;
using JJ.SmartHome.Core.Alarm;
using JJ.SmartHome.Core.Occupancy.Evaluation;
using System.Collections.Generic;
using NSubstitute.Extensions;

namespace JJ.SmartHome.Tests.Builders
{
    public class OccupancyAlertServiceBuilder
    {
        public const string MotionOccupancyTopic = "occupancy/motion";
        public const string MagnetOccupancyTopic = "occupancy/magnet";

        public MqttClientMock MqttClient = Substitute.ForPartsOf<MqttClientMock>();
        public ILastFiredAlertQuery LastFiredAlertQuery = Substitute.For<ILastFiredAlertQuery>();
        public IAlertsStore AlertsStore = Substitute.For<IAlertsStore>();
        public IAlertNotifier AlertNotifier = Substitute.For<IAlertNotifier>();
        private OccupancyOptions AlertsOptions = new()
        {
            OccupancyTopic = "occupancy/*",
            OccupancyAlertTopic= "alerts/occupancy",
            SnoozePeriodAfterAlerting = TimeSpan.Zero,
        };
        private OccupancyDevicesConfiguration OccupancyDevicesConfiguration = new()
        {
            Devices = new List<OccupancyDevicesConfiguration.Device>()
        };

        public OccupancyAlertServiceBuilder WithListenerTopic(string topic)
        {
            AlertsOptions.OccupancyTopic = topic;
            return this;
        }
        public OccupancyAlertServiceBuilder WithLastFiredAlert(DateTimeOffset lastFiredAlert)
        {
            LastFiredAlertQuery.CheckLastFiredAlert().Returns(
                Task.FromResult(new AlertMeasure
                {
                    Time = lastFiredAlert,
                    Location = "test",
                    Reason = "test"
                })
            );
            return this;
        }

        public OccupancyAlertServiceBuilder WithoutPreviousAlert()
        {
            LastFiredAlertQuery.CheckLastFiredAlert()
                .Returns(Task.FromResult<AlertMeasure>(null));
            
            return this;
        }

        public OccupancyAlertServiceBuilder WithSnoozeAlertPeriod(TimeSpan period)
        {
            AlertsOptions.SnoozePeriodAfterAlerting = period;

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

        public OccupancyAlertServiceBuilder WithDeviceConfiguration(OccupancyDevicesConfiguration.Device device)
        {
            OccupancyDevicesConfiguration.Devices.Add(device);

            return this;
        }

        public OccupancyAlertServiceBuilder WithDefaultDeviceConfiguration()
        {
            return WithDeviceConfiguration(new()
            {
                Evaluator = MotionSensorOccupancyEvaluatorStrategy.Evaluator,
                Topic = MotionOccupancyTopic
            })
            .WithDeviceConfiguration(new()
            {
                Evaluator = MagnetSensorOccupancyEvaluatorStrategy.Evaluator,
                Topic = MagnetOccupancyTopic
            });
        }

        public OccupancyAlertBackgroundService Build()
        {
            MqttClient
                .Publish(AlertsOptions.OccupancyAlertTopic, Arg.Any<string>())
                .Returns(Task.CompletedTask);

            var deviceOptions = Substitute.For<IOptionsSnapshot<OccupancyDevicesConfiguration>>();
            deviceOptions.Value.Returns(OccupancyDevicesConfiguration);

            var sut = new OccupancyAlertBackgroundService(
                MqttClient,
                Options.Create(AlertsOptions),
                AlertsStore,
                LastFiredAlertQuery,
                AlertNotifier,
                new AlarmStatusProvider(),
                new OccupancyEvaluator(new List<IOccupancyEvaluatorStrategy>
                {
                    new MotionSensorOccupancyEvaluatorStrategy(deviceOptions),
                    new MagnetSensorOccupancyEvaluatorStrategy(deviceOptions),

                }),
                LoggerFactory.Create(c => c.AddConsole()).CreateLogger<OccupancyAlertBackgroundService>()
            );

            return sut;
        }
    }
}