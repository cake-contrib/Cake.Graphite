namespace Cake.Graphite
{
    using System;
    using JetBrains.Annotations;

    [UsedImplicitly]
    public static class GraphiteExtensions{
        [UsedImplicitly]
        public static void Send(this Graphite graphite, string metricName, double value){
            graphite.Send(metricName, value);
        }

        [UsedImplicitly]
        public static void Send(this Graphite graphite, string metricName, double value, DateTime timeStamp){
            graphite.Send(metricName, value, timeStamp);
        }
    }
}