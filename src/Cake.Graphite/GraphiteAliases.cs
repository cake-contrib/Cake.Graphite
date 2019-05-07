using System.Runtime.CompilerServices;
using Cake.Core.Diagnostics;
using JetBrains.Annotations;

namespace Cake.Graphite
{
    using System;
    using Core;
    using Core.Annotations;

    /// <summary>
    /// Graphite aliases
    /// </summary>
    [CakeAliasCategory("Graphite")]
    [CakeNamespaceImport("Cake.Graphite")]
    public static class GraphiteAliases
    {
        /// <summary>
        /// Creates a new GraphiteClient using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// #addin nuget:?package=Cake.Graphite&amp;loaddependencies=true
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
        ///    // Executed BEFORE the first task.
        ///    Information("Running tasks...");
        /// 
        ///    var graphiteClient = Graphite(new GraphiteSettings{
        ///       Host = "localhost"
        ///    });
        /// 
        ///    return new BuildData(graphiteClient);
        /// });
        ///
        /// Task("Default")
        ///     .Does<BuildData>(data =>
        /// {
        ///     data.Graphite.Send("hest", 3);
        /// });
        /// ]]>
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Graphite")]
        [UsedImplicitly]
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