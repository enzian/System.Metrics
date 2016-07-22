namespace System.Metrics
{
    public interface IMetricsSink
    {
        void Handle(string metricRecord);
    }
}