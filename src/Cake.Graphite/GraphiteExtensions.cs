using System.Collections.Generic;
using ahd.Graphite;

namespace Cake.Graphite
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    /// Extension methods for <see cref="Graphite"/>
    /// </summary>
    [UsedImplicitly]
    public static class GraphiteExtensions{
        /// <summary>
        /// Send a single metric with timestamp of now
        /// </summary>
        /// <param name="graphite"></param>
        /// <param name="metricName"></param>
        /// <param name="value"></param>
        [UsedImplicitly]
        public static void Send(this Graphite graphite, string metricName, double value){
            graphite.Send(metricName, value);
        }

        /// <summary>
        /// Send a single metric with a specific timestamp
        /// </summary>
        /// <param name="graphite"></param>
        /// <param name="metricName"></param>
        /// <param name="value"></param>
        /// <param name="timeStamp"></param>
        [UsedImplicitly]
        public static void Send(this Graphite graphite, string metricName, double value, DateTime timeStamp){
            graphite.Send(metricName, value, timeStamp);
        }

        [UsedImplicitly]
        public static void Send(this Graphite graphite, ICollection<(string metricName,double value)> datapointTuples){
            graphite.Send(datapointTuples);
        }

        [UsedImplicitly]
        public static void Send(this Graphite graphite, ICollection<(string metricName,double value,DateTime timestamp)> datapointTuples){
            graphite.Send(datapointTuples);
        }

        [UsedImplicitly]
        public static void Send(this Graphite graphite, ICollection<Datapoint> datapoints){
            graphite.Send(datapoints);
        }

        [UsedImplicitly]
        public static void Send(this Graphite graphite, params Datapoint[] datapoints){
            graphite.Send(datapoints);
        }
    }
}