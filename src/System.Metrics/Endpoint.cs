namespace System.Metrics
{
    public interface Endpoint
    {
        void Record<TMetric>(string metric, int value) where TMetric : IAllowsInteger;

        void Record<TMetric>(string metric, double value) where TMetric : IAllowsDouble;

        void Record<TMetric>(string metric, int value, double sampleRate) where TMetric : IAllowsInteger, IAllowsSampleRate;

        void Record<TMetric>(string metric, double value, double sampleRate) where TMetric : IAllowsDouble, IAllowsSampleRate;

        void Record<TMetric>(string metric, string value) where TMetric : IAllowsString;
    }
}