using Cake.Testing;
using NUnit.Framework;
using System;

namespace Cake.Graphite.Tests
{
    public class GraphiteTests
    {
        private GraphiteSettings _settings;
        private Graphite _graphite;

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
            var cakeLog = new FakeLog();
            _graphite = new Graphite(cakeLog, _settings);
        }

        [Test]
        public void NullHost_ThrowsException()
        {
            _settings.ThrowExceptions = true;
            _settings.Host = null;
            var cakeLog = new FakeLog();

            Assert.Throws<ArgumentNullException>(() => {
                _graphite = new Graphite(cakeLog, _settings);
            });
        }

        [Test]
        public void NonAnsweringHost_ThrowsException()
        {
            _settings.ThrowExceptions = true;

            try
            {
                _graphite.Send("test", 1);
                Assert.Fail($"{nameof(NonAnsweringHost_ThrowsException)} should have thrown an exception");
            }
            catch
            {
                Assert.Pass();
            }
        }
    }
}