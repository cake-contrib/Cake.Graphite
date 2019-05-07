using JetBrains.Annotations;

namespace Cake.Graphite
{
    /// <summary>
    /// Graphite settings.
    /// </summary>
    public class GraphiteSettings
    {
        /// <summary>
        /// Initializes a new instance of the GraphiteSettings class.
        /// </summary>
        /// <param name="host">Graphite host to send metrics to.</param>
        /// <param name="httpApiPort">Graphite host port.</param>
        /// <param name="useSsl">Whether to use ssl or not.</param>
        /// <param name="batchSize">Graphite client metric batch size.</param>
        /// <param name="throwExceptions">Whether or not to throw exceptions on failures.</param>
        public GraphiteSettings(string host = null, ushort httpApiPort = 443, bool useSsl = true, int batchSize = 500, bool throwExceptions = false)
        {
            Host = host;
            HttpApiPort = httpApiPort;
            UseSsl = useSsl;
            BatchSize = batchSize;
            ThrowExceptions = throwExceptions;
        }

        /// <summary>
        /// Gets or sets the option to whether Cake.Graphite should rethrow the exception or not
        /// </summary>
        public bool ThrowExceptions { get; set; }

        /// <summary>
        /// Gets or sets the graphite client option to use ssl.
        /// </summary>
        public bool UseSsl { get; set; }

        /// <summary>
        /// Gets or sets the graphite client http api port.
        /// </summary>
        public ushort HttpApiPort { get; set; }

        /// <summary>
        /// Gets or sets the graphite client batch size.
        /// </summary>
        public int BatchSize { get; set; }

        /// <summary>
        /// Gets the host metrics are sent to.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the graphite client metrics prefix (usually used for api key for hosted services like HostedGraphite).
        /// </summary>
        [UsedImplicitly]
        public string Prefix { get; set; } 
    }
}
