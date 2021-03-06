namespace System.Metrics
{
    public interface IMetricsEndpoint : IPrefixedEndpoint
    {
        void Record<TMetric>(string metric, int value) where TMetric : IAllowsInteger;

        void Record<TMetric>(string metric, int value, bool isDelta = false) where TMetric : IAllowsInteger, IAllowsDelta;

        void Record<TMetric>(string metric, double value) where TMetric : IAllowsDouble;

        void Record<TMetric>(string metric, double value, bool isDelta = false) where TMetric : IAllowsDouble, IAllowsDelta;

        void Record<TMetric>(string metric, int value, double sampleRate) where TMetric : IAllowsInteger, IAllowsSampleRate;

        void Record<TMetric>(string metric, double value, double sampleRate) where TMetric : IAllowsDouble, IAllowsSampleRate;

        void Record<TMetric>(string metric, string value) where TMetric : IAllowsString;

        void AddBackend(IMetricsSink sink);
    }
}