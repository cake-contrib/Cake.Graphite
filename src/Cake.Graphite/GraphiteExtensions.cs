using System.Collections.Generic;
using ahd.Graphite;

namespace Cake.Graphite
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    /// Extension methods for <see cref="Graphite"/>
    /// </summary>
    [PublicAPI]
    public static class GraphiteExtensions
    {
        /// <summary>
        /// Send a single metric with timestamp of now
        /// </summary>
        /// <param name="graphite"></param>
        /// <param name="metricName"></param>
        /// <param name="value"></param>
        [PublicAPI]
        public static void Send(this Graphite graphite, string metricName, double value)
        {
            graphite.Send(metricName, value);
        }

        /// <summary>
        /// Send a single metric with a specific timestamp
        /// </summary>
        /// <param name="graphite"></param>
        /// <param name="metricName"></param>
        /// <param name="value"></param>
        /// <param name="timeStamp"></param>
        [PublicAPI]
        public static void Send(this Graphite graphite, string metricName, double value, DateTime timeStamp)
        {
            graphite.Send(metricName, value, timeStamp);
        }

        /// <summary>
        /// Send a collection of Datapoint
        /// </summary>
        /// <param name="graphite"></param>
        /// <param name="datapoints"></param>
        [PublicAPI]
        public static void Send(this Graphite graphite, IEnumerable<Datapoint> datapoints)
        {
            graphite.Send(datapoints);
        }

        /// <summary>
        /// Send a collection of Datapoint
        /// </summary>
        /// <param name="graphite"></param>
        /// <param name="datapoints"></param>
        [PublicAPI]
        public static void Send(this Graphite graphite, params Datapoint[] datapoints)
        {
            graphite.Send(datapoints);
        }
    }
}