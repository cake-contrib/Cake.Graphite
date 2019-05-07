using Cake.Testing;
using NUnit.Framework;
using System;
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

            Assert.Throws<Exception>(() => {
                graphite.Send("test", 1);
            });
        }

        [Test]
        public void FailingSend_Ignored_DoesNotThrowException()
        {
            var mockClient = new Mock<IGraphiteClient>();
            mockClient.Setup(x => x.Send(It.IsAny<string>(), It.IsAny<double>())).Throws<Exception>();
            _settings.ThrowExceptions = false;
            var graphite = new Graphite(_log, _settings, mockClient.Object);

            Assert.DoesNotThrow(() => {
                graphite.Send("test", 1);
            });
        }

        [Test]
        public void SendingWithPrefix_Works()
        {
            const string prefix = "prefix_test";
            const string testMetricName = "test";
            var expectedName = $"{prefix}.{testMetricName}";

            _settings.Prefix = prefix;

            var mockClient = new Mock<IGraphiteClient>();
            mockClient.Setup(x => x.Send(It.IsAny<string>(), It.IsAny<double>())).Callback((string metricName, double value) =>
                {
                    Assert.AreEqual(metricName, expectedName);
                });
            
            var graphite = new Graphite(_log, _settings, mockClient.Object);
            graphite.Send(testMetricName, 1);
        }
    }
}