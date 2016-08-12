using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace System.Metrics
{
    public class StandardEndpoint : IMetricsEndpoint, IPrefixedEndpoint
    {
        internal List<IMetricsSink> MetricsSinks { get; set; } = new List<IMetricsSink>();

        private readonly string Format = "{0}:{1}|{2}";

        private readonly string Format_WithSampleRate = "{0}:{1}|{2}|@{3}";

        private string _Prefix = string.Empty;

        internal readonly Dictionary<Type, string> units = new Dictionary<Type, string>
                                                                       {
                                                                           {typeof(Counting), "c"},
                                                                           {typeof(Timing), "ms"},
                                                                           {typeof(Gauge), "g"},
                                                                           {typeof(Histogram), "h"},
                                                                           {typeof(Meter), "m"},
                                                                           {typeof(Set), "s"}
                                                                       };
        
        public string Prefix
        {
            get
            {
                return _Prefix;
            }
            set
            {
                _Prefix = value.Trim('.');
            }
        }

        public void Record<TMetric>(string metric, double value) where TMetric : IAllowsDouble
        {
            var command = CompileCommand(metric, value.ToString(CultureInfo.InvariantCulture), units[typeof(TMetric)]);
            SendCommand(command);
        }

        public void Record<TMetric>(string metric, double value, bool isDelta) where TMetric : IAllowsDouble, IAllowsDelta
        {
            var command = CompileCommand(metric, value >= 0 ? "+" + value.ToString() : value.ToString(), units[typeof(TMetric)]);
            SendCommand(command);
        }

        public void Record<TMetric>(string metric, string value) where TMetric : IAllowsString
        {
            throw new NotImplementedException();
        }

        public void Record<TMetric>(string metric, int value) where TMetric : IAllowsInteger
        {
            var command = CompileCommand(metric, value.ToString(CultureInfo.InvariantCulture), units[typeof(TMetric)]);
            SendCommand(command);
        }
        
        public void Record<TMetric>(string metric, int value, bool isDelta = false) where TMetric : IAllowsInteger, IAllowsDelta
        {
            var number = value.ToString(CultureInfo.InvariantCulture);

            if(isDelta)
            {
                number = value >= 0 ? "+" + number : number;
            }
            
            var command = CompileCommand(metric, number, units[typeof(TMetric)]);

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
            // Concatenate prefix and metric name
            var prefixedMetric = _Prefix != string.Empty ? string.Format("{0}.{1}", _Prefix, metric) : metric;

            if(sampleRate != null)
            {
                return string.Format(CultureInfo.InvariantCulture, Format_WithSampleRate, prefixedMetric, value, type, sampleRate);
            }

            return string.Format(CultureInfo.InvariantCulture, Format, prefixedMetric, value, type);
        }

        private void SendCommand(string command){            
            var tasks = MetricsSinks.Select(x => x.Handle(command));
            Task.WhenAll(tasks).Wait();
        }

        public void AddBackend(IMetricsSink sink)
        {
            this.MetricsSinks.Add(sink);
        }
    }
}