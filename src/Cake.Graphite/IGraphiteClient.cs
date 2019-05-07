using ahd.Graphite;
using System;
using System.Collections.Generic;

namespace Cake.Graphite
{
    /// <summary>
    /// Helper interface for test mocking
    /// </summary>
    public interface IGraphiteClient
    {
        void Send(ICollection<Datapoint> datapoints);
        void Send(params Datapoint[] datapoints);
        void Send(string metricName, double value);
        void Send(string metricName, double value, DateTime timeStamp);
    }

    /// <summary>
    /// Wrapper of ahd.GraphiteClient using the IGraphiteClient interface
    /// </summary>
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
