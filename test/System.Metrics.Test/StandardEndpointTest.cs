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
            fakeSink.Metrics.Should().NotBeEmpty("No commands were sent, expected one!");
            fakeSink.Metrics.Should().Contain("metric.test.total:1|c");
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
            fakeSink.Metrics.Should().NotBeEmpty("No commands were sent, expected one!");
            fakeSink.Metrics.Should().Contain("metric.test.total:1|c|@0.1");
        }

        [Fact]
        public void TestGauge_Floating()
        {
            // Arrange
            var subject = new StandardEndpoint();
            var fakeSink = new FakeSink();
            subject.AddSink(fakeSink);

            // Act
            subject.Record<Gauge>("metric.test.load", 1.1);

            // Assert
            fakeSink.Metrics.Should().NotBeEmpty("No commands were sent, expected one!");
            fakeSink.Metrics.Should().Contain("metric.test.load:1.1|g");
        }

        [Theory]
        [InlineData(1.1, "+1.1")]
        [InlineData(-1.1, "-1.1")]
        [InlineData(0, "+0")]
        [InlineData(12345.123456789, "+12345.123456789")]
        [InlineData(-12345.123456789, "-12345.123456789")]
        public void TestGauge_WithDeltas_Decimal(double value, string expected)
        {
            // Arrange
            var subject = new StandardEndpoint();
            var fakeSink = new FakeSink();
            subject.AddSink(fakeSink);

            // Act
            subject.Record<Gauge>("metric.test.load", value, true);
            
            // Assert
            fakeSink.Metrics.Should().NotBeEmpty("No commands were sent, expected one!");
            fakeSink.Metrics.Should().Contain(x => x.Contains($":{expected}|"));
        }

        
        [Theory]
        [InlineData(1, "+1")]
        [InlineData(-1, "-1")]
        [InlineData(0, "+0")]
        public void TestGauge_WithDelta_Integral(int value, string expected)
        {
            // Arrange
            var subject = new StandardEndpoint();
            var fakeSink = new FakeSink();
            subject.AddSink(fakeSink);

            // Act
            subject.Record<Gauge>("metric.test.load", value, true);
            
            // Assert
            fakeSink.Metrics.Should().NotBeEmpty("No commands were sent, expected one!");
            fakeSink.Metrics.Should().Contain(x => x.Contains($":{expected}|"));
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