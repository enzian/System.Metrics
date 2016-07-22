using System.Threading.Tasks;

namespace System.Metrics
{
    public interface IMetricsSink
    {
        Task Handle(string metricRecord);
    }
}