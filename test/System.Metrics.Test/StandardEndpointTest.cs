using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace System.Metrics
{
    public class StandardEndpointTest
    {
        [Fact]
        public void TestCounting_WithIntegralValue()
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

        [Fact]
        public void TestCounting_WithSampleRate()
        {
            // Arrange
            var subject = new StandardEndpoint();
            var fakeSink = new FakeSink();
            subject.AddSink(fakeSink);

            // Act
            subject.Record<Counting>("metric.test.total", 1, 0.1);

            // Assert
            fakeSink.Metrics.Should().ContainSingle("metric.test.total:1|c|0.1");
        }

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
}