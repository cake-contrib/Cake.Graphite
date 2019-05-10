using ahd.Graphite;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Cake.Graphite
{
    internal interface IGraphiteClient
    {
        void Send(ICollection<Datapoint> datapoints);
        void Send(params Datapoint[] datapoints);
        void Send(string metricName, double value);
        void Send(string metricName, double value, DateTime timeStamp);
    }

    [ExcludeFromCodeCoverage]
    internal class GraphiteClientWrapper : IGraphiteClient
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
