using System.Collections.Generic;
using System.Linq;

namespace Cake.Graphite
{
    using ahd.Graphite;
    using Core.Diagnostics;
    using System;

    /// <summary>
    /// Class for Graphite.
    /// </summary>
    public class Graphite
    {
        private readonly ICakeLog _log;
        private readonly GraphiteSettings _settings;
        private readonly IGraphiteClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="Graphite"/> class.
        /// </summary>
        /// <param name="log">Cake log instance.</param>
        /// <param name="settings"><see cref="GraphiteSettings"/> instance.</param>
        public Graphite(
            ICakeLog log,
            GraphiteSettings settings)
        {
            _log = log;
            _settings = settings;

            _client = new GraphiteClientWrapper(new GraphiteClient(settings.Host)
            {
                HttpApiPort = settings.HttpApiPort,
                BatchSize = settings.BatchSize,
                UseSsl = settings.UseSsl,
            });
        }

        internal Graphite(
            ICakeLog log, 
            GraphiteSettings settings,
            IGraphiteClient client
        )
        {
            _log = log;
            _settings = settings;
            _client = client;
        }

        private string PrefixMetricName(string metricName)
        {
            var prefixedMetricName = metricName;
            if (!string.IsNullOrWhiteSpace(_settings.Prefix))
            {
                prefixedMetricName = $"{_settings.Prefix}.{metricName}";
            }

            return prefixedMetricName;
        }

        public void Send(string metricName, double value, DateTime timeStamp)
        {
            try
            {
                _log.Verbose($"Sending metric: '{PrefixMetricName(metricName)}' with value '{value}' and timestamp '{timeStamp.ToLongDateString()}'");
                _client.Send(PrefixMetricName(metricName), value, timeStamp);
            }
            catch(Exception ex)
            {
                if (_settings.ThrowExceptions) throw;
                _log.Verbose($"{nameof(Send)} would have thrown an {ex.GetType()}");
            }
        }

        public void Send(string metricName, double value)
        {
            try
            {
                _log.Verbose($"Sending metric: '{PrefixMetricName(metricName)}' with value '{value}'");
                _client.Send(PrefixMetricName(metricName), value);
            }
            catch(Exception ex)
            {
                if (_settings.ThrowExceptions) throw;
                _log.Verbose($"{nameof(Send)} would have thrown an {ex.GetType()}");
            }
        }

        private void TrySend(ICollection<Datapoint> datapoints){
            try
            {
                _client.Send(datapoints);
            }
            catch(Exception ex)
            {
                if (_settings.ThrowExceptions) throw;
                _log.Verbose($"{nameof(Send)} would have thrown an {ex.GetType()}");
            }
        }

        public void Send(ICollection<(string metricName,double value)> datapointTuples)
        {
            var now = DateTime.UtcNow;
            var datapoints = datapointTuples.Select(x => new Datapoint(PrefixMetricName(x.metricName), x.value, now)).ToArray();

            TrySend(datapoints);
        }

        public void Send(ICollection<(string metricName,double value,DateTime timeStamp)> datapointTuples)
        {
            var datapoints = datapointTuples.Select(x => new Datapoint(PrefixMetricName(x.metricName), x.value, x.timeStamp)).ToArray();

            TrySend(datapoints);
        }

        public void Send(ICollection<ahd.Graphite.Datapoint> datapoints)
        {
            TrySend(datapoints);
        }
    }
}
