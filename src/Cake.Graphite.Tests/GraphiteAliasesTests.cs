using Cake.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ahd.Graphite;
using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Moq;

namespace Cake.Graphite.Tests
{
    public class GraphiteAliasesTests
    {
        private GraphiteSettings _settings;
        private ICakeLog _log;

        private ICakeConfiguration _configuration;
        private IFileSystem _fileSystem;
        private ICakeEnvironment _environment;
        private IGlobber _globber;
        private ICakeArguments _arguments;
        private IToolLocator _toolLocator;
        private IProcessRunner _processRunner;
        private ICakeDataService _dataService;

        private ICakeContext _context;


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
            _configuration = new FakeConfiguration();
            _environment = new FakeEnvironment(PlatformFamily.Windows)
            {
                WorkingDirectory = new DirectoryPath(Environment.CurrentDirectory)
            };
            _fileSystem = new FakeFileSystem(_environment);
            _globber = new Globber(_fileSystem, _environment);

            var mockArguments = new Mock<ICakeArguments>();
            mockArguments.Setup(x => x.GetArgument(It.IsAny<string>())).Returns(string.Empty);
            mockArguments.Setup(x => x.HasArgument(It.IsAny<string>())).Returns(false);
            _arguments = mockArguments.Object;

            _toolLocator = new ToolLocator(_environment, new ToolRepository(_environment), new ToolResolutionStrategy(_fileSystem, _environment, _globber, _configuration));
            _processRunner = new ProcessRunner(_fileSystem, _environment, _log, _toolLocator, _configuration);

            var mockDataService = new Mock<ICakeDataService>();
            mockDataService.Setup(x => x.Add(It.IsAny<string>()));
            _dataService = mockDataService.Object;

            _context = new CakeContext(_fileSystem, _environment, _globber, _log, _arguments, _processRunner, new WindowsRegistry(), _toolLocator, _dataService, _configuration);
        }

        [Test]
        public void CtorNullContext_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => { GraphiteAliases.Graphite(null, _settings); });
        }

        [Test]
        public void CtorNullSettings_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => { _context.Graphite(null); });
        }

        [Test]
        public void CtorNullSettingsHost_ThrowsException()
        {
            _settings.Host = null;
            Assert.Throws<ArgumentNullException>(() => { _context.Graphite(_settings); });
        }

        [Test]
        public void Ctor_Ok()
        {
            var result = _context.Graphite(_settings);

            Assert.IsInstanceOf<Graphite>(result);
        }

        [Test]
        public void Extension_Send_string_double_Throws_Ok()
        {
            var mockClient = new Mock<IGraphiteClient>();
            mockClient.Setup(x => x.Send(It.IsAny<string>(), It.IsAny<double>())).Throws<Exception>();
            _settings.ThrowExceptions = false;
            var graphite = _context.Graphite(_settings, mockClient.Object);

            Assert.IsInstanceOf<Graphite>(graphite);
            Assert.DoesNotThrow(() =>
            {
                GraphiteExtensions.Send(graphite, "test", 1);
            });
        }

        [Test]
        public void Extension_Send_string_double_timestamp_Throws_Ok()
        {
            var mockClient = new Mock<IGraphiteClient>();
            mockClient.Setup(x => x.Send(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<DateTime>())).Throws<Exception>();
            _settings.ThrowExceptions = false;
            var graphite = _context.Graphite(_settings, mockClient.Object);

            Assert.IsInstanceOf<Graphite>(graphite);
            Assert.DoesNotThrow(() =>
            {
                GraphiteExtensions.Send(graphite, "test", 1, DateTime.UtcNow);
            });
        }

        [Test]
        public void Extension_Send_string_double_Ok()
        {
            var mockClient = new Mock<IGraphiteClient>();
            mockClient.Setup(x => x.Send(It.IsAny<string>(), It.IsAny<double>()));
            var graphite = _context.Graphite(_settings, mockClient.Object);

            Assert.IsInstanceOf<Graphite>(graphite);
            Assert.DoesNotThrow(() =>
            {
                GraphiteExtensions.Send(graphite, "test", 1);
            });
        }

        [Test]
        public void Extension_Send_string_double_timestamp_Ok()
        {
            var mockClient = new Mock<IGraphiteClient>();
            mockClient.Setup(x => x.Send(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<DateTime>()));
            var graphite = _context.Graphite(_settings, mockClient.Object);

            Assert.IsInstanceOf<Graphite>(graphite);
            Assert.DoesNotThrow(() =>
            {
                GraphiteExtensions.Send(graphite, "test", 1, DateTime.UtcNow);
            });
        }

        [Test]
        public void Extension_Send_string_double_Throws()
        {
            var mockClient = new Mock<IGraphiteClient>();
            mockClient.Setup(x => x.Send(It.IsAny<string>(), It.IsAny<double>())).Throws<Exception>();
            _settings.ThrowExceptions = true;
            var graphite = _context.Graphite(_settings, mockClient.Object);

            Assert.IsInstanceOf<Graphite>(graphite);
            try
            {
                GraphiteExtensions.Send(graphite, "test", 1);
                Assert.Fail($"{nameof(Extension_Send_string_double_Throws)} should have thrown an exception");
            }
            catch
            {
                Assert.Pass();
            }
        }

        [Test]
        public void Extension_Send_string_double_timestamp_Throws()
        {
            var mockClient = new Mock<IGraphiteClient>();
            mockClient.Setup(x => x.Send(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<DateTime>())).Throws<Exception>();
            _settings.ThrowExceptions = true;
            var graphite = _context.Graphite(_settings, mockClient.Object);

            Assert.IsInstanceOf<Graphite>(graphite);
            try
            {
                GraphiteExtensions.Send(graphite, "test", 1,DateTime.Now);
                Assert.Fail($"{nameof(Extension_Send_string_double_timestamp_Throws)} should have thrown an exception");
            }
            catch
            {
                Assert.Pass();
            }
        }

        [Test]
        public void Extension_Send_Collection_Tuple_Throws()
        {
            var mockClient = new Mock<IGraphiteClient>();
            mockClient.Setup(x => x.Send(It.IsAny<Datapoint[]>())).Throws<Exception>();
            mockClient.Setup(x => x.Send(It.IsAny<ICollection<Datapoint>>())).Throws<Exception>();
            _settings.ThrowExceptions = true;
            var graphite = _context.Graphite(_settings, mockClient.Object);

            Assert.IsInstanceOf<Graphite>(graphite);
            try
            {
                GraphiteExtensions.Send(graphite, new List<(string, double)>(){("test", 1)});
                Assert.Fail($"{nameof(Extension_Send_Collection_Tuple_Throws)} should have thrown an exception");
            }
            catch
            {
                Assert.Pass();
            }
        }

        [Test]
        public void Extension_Send_Collection_Tuple_With_Datetime_Throws()
        {
            var mockClient = new Mock<IGraphiteClient>();
            mockClient.Setup(x => x.Send(It.IsAny<Datapoint[]>())).Throws<Exception>();
            mockClient.Setup(x => x.Send(It.IsAny<ICollection<Datapoint>>())).Throws<Exception>();
            _settings.ThrowExceptions = true;
            var graphite = _context.Graphite(_settings, mockClient.Object);

            Assert.IsInstanceOf<Graphite>(graphite);
            try
            {
                GraphiteExtensions.Send(graphite, new List<(string, double, DateTime)>(){("test", 1, DateTime.UtcNow)});
                Assert.Fail($"{nameof(Extension_Send_Collection_Tuple_Throws)} should have thrown an exception");
            }
            catch
            {
                Assert.Pass();
            }
        }

        [Test]
        public void Extension_Send_Collection_Datapoint_Throws()
        {
            var mockClient = new Mock<IGraphiteClient>();
            mockClient.Setup(x => x.Send(It.IsAny<Datapoint[]>())).Throws<Exception>();
            mockClient.Setup(x => x.Send(It.IsAny<ICollection<Datapoint>>())).Throws<Exception>();
            _settings.ThrowExceptions = true;
            var graphite = _context.Graphite(_settings, mockClient.Object);

            Assert.IsInstanceOf<Graphite>(graphite);
            try
            {
                GraphiteExtensions.Send(graphite, new Collection<Datapoint>{new Datapoint("test", 1, DateTime.UtcNow)});
                Assert.Fail($"{nameof(Extension_Send_Collection_Tuple_Throws)} should have thrown an exception");
            }
            catch
            {
                Assert.Pass();
            }
        }
    }
}