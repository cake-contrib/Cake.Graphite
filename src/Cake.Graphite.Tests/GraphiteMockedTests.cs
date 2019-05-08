using Cake.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using ahd.Graphite;
using Cake.Core.Diagnostics;
using Moq;

namespace Cake.Graphite.Tests
{
    public class GraphiteMockedTests
    {
        private GraphiteSettings _settings;
        private ICakeLog _log;

        [SetUp]
        public void Setup()
        {
            _settings = new GraphiteSettings()
            {
                Host = "192.0.2.0",
                HttpApiPort = 10,
                BatchSize = 500,
                ThrowExceptions = true
            };
            _log = new FakeLog();
        }

        [Test]
        public void FailingSend_ThrowsException()
        {
            var mockClient = new Mock<IGraphiteClient>();
            mockClient.Setup(x => x.Send(It.IsAny<string>(), It.IsAny<double>())).Throws<Exception>();
            var graphite = new Graphite(_log, _settings, mockClient.Object);

            Assert.Throws<Exception>(() => { graphite.Send("test", 1); });
        }

        [Test]
        public void FailingSend_Ignored_DoesNotThrowException()
        {
            var mockClient = new Mock<IGraphiteClient>();
            mockClient.Setup(x => x.Send(It.IsAny<string>(), It.IsAny<double>())).Throws<Exception>();
            _settings.ThrowExceptions = false;
            var graphite = new Graphite(_log, _settings, mockClient.Object);

            Assert.DoesNotThrow(() => { graphite.Send("test", 1); });
        }

        [Test]
        public void Send_Works()
        {
            var mockClient = new Mock<IGraphiteClient>();
            mockClient.Setup(x => x.Send(It.IsAny<string>(), It.IsAny<double>()));
            _settings.ThrowExceptions = true;
            var graphite = new Graphite(_log, _settings, mockClient.Object);

            Assert.DoesNotThrow(() => { graphite.Send("test", 1); });
        }

        [Test]
        public void Send_Datapoint_Works()
        {
            var mockClient = new Mock<IGraphiteClient>();
            mockClient.Setup(x => x.Send(It.IsAny<Datapoint[]>()));
            _settings.ThrowExceptions = true;
            var graphite = new Graphite(_log, _settings, mockClient.Object);

            Assert.DoesNotThrow(() => { graphite.Send(new Datapoint("test", 1, DateTime.UtcNow)); });
        }

        [Test]
        public void SendingWithPrefix_Works()
        {
            const string prefix = "prefix_test";
            const string testMetricName = "test";
            string returnValue = null;
            var expectedName = $"{prefix}.{testMetricName}";

            _settings.Prefix = prefix;

            var mockClient = new Mock<IGraphiteClient>();
            mockClient.Setup(x => x.Send(It.IsAny<string>(), It.IsAny<double>()))
                .Callback((string metricName, double value) => { returnValue = metricName; });

            var graphite = new Graphite(_log, _settings, mockClient.Object);
            graphite.Send(testMetricName, 1);

            Assert.IsNotNull(returnValue);
            Assert.AreEqual(returnValue, expectedName);
        }

        [Test]
        public void SendingDatapointWithPrefix_Works()
        {
            const string prefix = "prefix_test";
            const string testMetricName = "test";
            string returnValue = null;
            var expectedName = $"{prefix}.{testMetricName}";

            _settings.Prefix = prefix;

            var mockClient = new Mock<IGraphiteClient>();
            mockClient.Setup(x => x.Send(It.IsAny<ICollection<Datapoint>>()))
                .Callback((ICollection<Datapoint> x) => { returnValue = x.First().Series; });
            mockClient.Setup(x => x.Send(It.IsAny<Datapoint>()))
                .Callback((Datapoint[] x) => { returnValue = x[0].Series; });

            var graphite = new Graphite(_log, _settings, mockClient.Object);
            graphite.Send(new Datapoint(testMetricName, 1,DateTime.Now));

            Assert.IsNotNull(returnValue);
            Assert.AreEqual(expectedName, returnValue);
        }
    }
}