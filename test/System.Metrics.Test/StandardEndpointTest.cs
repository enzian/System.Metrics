using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace System.Metrics
{
    public class StandardEndpointTest
    {
        [Fact]
        public void TestIntegralValue()
        {
            // Arrange
            var subject = new StandardEndpoint();
            var fakeSink = new FakeSink();
            subject.AddSink(fakeSink);

            // Act
            subject.Record<Counting>("metric.test.total", 1);

            // Assert
            fakeSink.Metrics.Should().ContainSingle("metric.test.total:1|c");
        }

        internal class FakeSink : IMetricsSink
        {
            public List<string> Metrics { get; set; } = new List<string>();

            public async Task Handle(string metricRecord)
            {
                Metrics.Add(metricRecord);
                await Task.Delay(1000);
            }
        }
    }
}