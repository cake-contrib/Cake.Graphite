using Cake.Core.Diagnostics;
using JetBrains.Annotations;

namespace Cake.Graphite
{
    using Core;
    using Core.Annotations;
    using System;

    /// <summary>
    /// Graphite aliases
    /// </summary>
    [CakeAliasCategory("Graphite")]
    [CakeNamespaceImport("Cake.Graphite")]
    public static class GraphiteAliases
    {
        /// <summary>
        /// Creates a new GraphiteClient using the specified settings.
        /// Send methods are in <see cref="GraphiteExtensions"/>
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// #addin nuget:?package=Cake.Graphite&amp;loaddependencies=true
        /// // This is needed for the Datapoint struct
        /// using ahd.Graphite;
        /// public class BuildData
        /// {
        ///     public Graphite Graphite { get; }
        /// 
        ///     public BuildData(Graphite graphite)
        ///     {
        ///        Graphite = graphite;
        ///     }
        /// }
        /// 
        /// Setup(setupContext  =>
        /// {
        ///    var graphiteClient = Graphite(new GraphiteSettings{
        ///       Host = "localhost",
        ///       Prefix = "cake.graphite.example" // Include your api key in front if you are using HostedGraphite
        ///    });
        /// 
        ///    return new BuildData(graphiteClient);
        /// });
        ///
        /// Task("Default")
        ///     .Does<BuildData>(data =>
        /// {
        ///     // Send a single metric without time
        ///     data.Graphite.Send("single_metric", 1);
        ///     // Send a single metric with a specific time
        ///     data.Graphite.Send("single_metric2", 1, DateTime.UtcNow);
        ///
        ///     var now = DateTime.UtcNow;
        ///     var bulkDatapoints = new List<Datapoint>();
        ///     bulkDatapoints.Add(new Datapoint("bulk_metric1", 1, now));
        ///     bulkDatapoints.Add(new Datapoint("bulk_metric2", 1, now));
        ///     bulkDatapoints.Add(new Datapoint("bulk_metric3", 1, now));
        ///     bulkDatapoints.Add(new Datapoint("bulk_metric4", 1, now));
        ///     bulkDatapoints.Add(new Datapoint("bulk_metric5", 1, now));
        ///     bulkDatapoints.Add(new Datapoint("bulk_metric6", 1, now));
        ///
        ///     // Bulk sending of metrics
        ///     data.Graphite.Send(bulkDatapoints);
        /// });
        /// ]]>
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Graphite")]
        [PublicAPI]
        public static Graphite Graphite(this ICakeContext context, GraphiteSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings == null)
            {
                context.Log.Error("Calling Graphite() with GraphiteSettings being null throws an exception");
                throw new ArgumentNullException(nameof(settings));
            }

            if (string.IsNullOrWhiteSpace(settings.Host))
            {
                context.Log.Error("GraphiteSettings.Host has to be a non-empty string");
                throw new ArgumentNullException(nameof(settings.Host));
            }

            AddinInformation.LogVersionInformation(context.Log);
            return new Graphite(context.Log, settings);
        }

        internal static Graphite Graphite(this ICakeContext context, GraphiteSettings settings, IGraphiteClient graphiteClient)
        {
            return new Graphite(context.Log, settings, graphiteClient);
        }
    }
}