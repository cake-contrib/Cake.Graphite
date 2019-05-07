using NUnit.Framework;

namespace Cake.Graphite.Tests
{
    public class GraphiteSettingsTests
    {
        [Test]
        public void NormalConstructor_OnlyHost_Ok()
        {
            var settings = new GraphiteSettings("localhost");
            Assert.That(settings.Host, Is.EqualTo("localhost"));
        }

        [Test]
        public void NormalConstructor_HostAndPort_Ok()
        {
            var settings = new GraphiteSettings("localhost", 80);
            Assert.That(settings.Host, Is.EqualTo("localhost"));
            Assert.That(settings.HttpApiPort, Is.EqualTo(80));
        }

        [Test]
        public void NormalConstructor_HostPortAndSsl_Ok()
        {
            var settings = new GraphiteSettings("localhost", 80, false);
            Assert.That(settings.Host, Is.EqualTo("localhost"));
            Assert.That(settings.HttpApiPort, Is.EqualTo(80));
            Assert.That(settings.UseSsl, Is.EqualTo(false));
        }

        [Test]
        public void NormalConstructor_HostPortSslAndBatchSize_Ok()
        {
            var settings = new GraphiteSettings("localhost", 80, false, 123);
            Assert.That(settings.Host, Is.EqualTo("localhost"));
            Assert.That(settings.HttpApiPort, Is.EqualTo(80));
            Assert.That(settings.UseSsl, Is.EqualTo(false));
            Assert.That(settings.BatchSize, Is.EqualTo(123));
        }

        [Test]
        public void NormalConstructor_HostPortSslBatchSizeAndThrowExceptions_Ok()
        {
            var settings = new GraphiteSettings("localhost", 80, false, 123, true);
            Assert.That(settings.Host, Is.EqualTo("localhost"));
            Assert.That(settings.HttpApiPort, Is.EqualTo(80));
            Assert.That(settings.UseSsl, Is.EqualTo(false));
            Assert.That(settings.BatchSize, Is.EqualTo(123));
            Assert.That(settings.ThrowExceptions, Is.EqualTo(true));
        }
    }
}