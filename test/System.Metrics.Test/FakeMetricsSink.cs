using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Metrics
{
    internal class FakeSink : IMetricsSink
    {
        public List<string> Metrics { get; set; } = new List<string>();

        public async Task Handle(string metricRecord)
        {
            Metrics.Add(metricRecord);
            // Adda delay to provoke timing issued if synchronization should be broken
            await Task.Delay(10);
        }
    }
}
