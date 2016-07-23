using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace System.Metrics
{
    public class StandardEndpoint : Endpoint
    {
        internal List<IMetricsSink> MetricsSinks { get; set; } = new List<IMetricsSink>();

        private readonly string Format = "{0}:{1}|{2}";

        private readonly string Format_WithSampleRate = "{0}:{1}|{2}|{3}";

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
            var command = CompileCommand(metric, value.ToString(CultureInfo.InvariantCulture), units[typeof(TMetric)], null);
            SendCommand(command);
        }

        public void Record<TMetric>(string metric, double value, double sampleRate) where TMetric : IAllowsDouble, IAllowsSampleRate
        {
            throw new NotImplementedException();
        }

        public void Record<TMetric>(string metric, int value, double sampleRate) where TMetric : IAllowsInteger, IAllowsSampleRate
        {
            var command = CompileCommand(metric, value.ToString(CultureInfo.InvariantCulture), units[typeof(TMetric)], sampleRate);
            SendCommand(command);
        }

        public void AddSink(IMetricsSink sink)
        {
            MetricsSinks.Add(sink);
        }

        private string CompileCommand(string metric, string value, string type, double? sampleRate = null)
        {            
            if(sampleRate != null)
            {
                return string.Format(CultureInfo.InvariantCulture, Format_WithSampleRate, metric, value, type, sampleRate);
            }

            return string.Format(CultureInfo.InvariantCulture, Format, metric, value, type);
        }

        private void SendCommand(string command){            
            var tasks = MetricsSinks.Select(x => x.Handle(command));
            Task.WhenAll(tasks).Wait();
        }
    }
}