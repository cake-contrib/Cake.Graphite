using ahd.Graphite;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Cake.Graphite
{
    /// <summary>
    /// Helper interface for test mocking
    /// </summary>
    public interface IGraphiteClient
    {
        /// <summary>
        /// Send a collection of Datapoint
        /// </summary>
        /// <param name="datapoints"></param>
        void Send(ICollection<Datapoint> datapoints);
        /// <summary>
        /// Send a array of Datapoint
        /// </summary>
        /// <param name="datapoints"></param>
        void Send(params Datapoint[] datapoints);
        /// <summary>
        /// Send a single metric with a specific timestamp
        /// </summary>
        /// <param name="metricName"></param>
        /// <param name="value"></param>
        void Send(string metricName, double value);
        /// <summary>
        /// Send a single metric with timestamp of now
        /// </summary>
        /// <param name="metricName"></param>
        /// <param name="value"></param>
        /// <param name="timeStamp"></param>
        void Send(string metricName, double value, DateTime timeStamp);
    }

    /// <summary>
    /// Wrapper of ahd.GraphiteClient using the IGraphiteClient interface
    /// </summary>
    [ExcludeFromCodeCoverage]
    class GraphiteClientWrapper : IGraphiteClient
    {
        private readonly GraphiteClient _client;

        public GraphiteClientWrapper(GraphiteClient client)
        {
            _client = client;
        }

        public void Send(string metricName, double value)
        {
            _client.Send(metricName, value);
        }

        public void Send(string metricName, double value, DateTime timeStamp)
        {
            _client.Send(metricName, value, timeStamp);
        }

        public void Send(ICollection<Datapoint> datapoints)
        {
            _client.Send(datapoints);
        }

        public void Send(params Datapoint[] datapoints)
        {
            _client.Send(datapoints);
        }
    }
}
