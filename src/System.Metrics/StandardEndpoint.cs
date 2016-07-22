using System.Collections.Generic;

namespace System.Metrics
{
    public class StandardEndpoint : Endpoint
    {
        internal List<IMetricsSink> MetricsSinks { get; set; } = new List<IMetricsSink>();

        internal readonly Dictionary<Type, string> units = new Dictionary<Type, string>
                                                                       {
                                                                           {typeof(Counting), "c"},
                                                                           {typeof(Timing), "ms"},
                                                                           {typeof(Gauge), "g"},
                                                                           {typeof(Histogram), "h"},
                                                                           {typeof(Meter), "m"},
                                                                           {typeof(Set), "s"}
                                                                       };

        public void Record<TMetric>(string metric, double value) where TMetric : IAllowsDouble
        {
            throw new NotImplementedException();
        }

        public void Record<TMetric>(string metric, string value) where TMetric : IAllowsString
        {
            throw new NotImplementedException();
        }

        public void Record<TMetric>(string metric, int value) where TMetric : IAllowsInteger
        {
            var command = string.Format("{0}:{1}|{2}", metric, value, units[typeof(TMetric)]);
            
            foreach (var sink in MetricsSinks)
            {
                sink.Handle(command);
            }
        }

        public void Record<TMetric>(string metric, double value, double sampleRate) where TMetric : IAllowsDouble, IAllowsSampleRate
        {
            throw new NotImplementedException();
        }

        public void Record<TMetric>(string metric, double value, int sampleRate) where TMetric : IAllowsInteger, IAllowsSampleRate
        {
            throw new NotImplementedException();
        }

        public void AddSink(IMetricsSink sink)
        {
            MetricsSinks.Add(sink);
        }
    }
}