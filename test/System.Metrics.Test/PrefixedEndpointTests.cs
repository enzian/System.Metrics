using FluentAssertions;
using Xunit;

namespace System.Metrics
{
    public class PrefixedEndpointTests
    {
        [Theory]
        [InlineData("metric", "simple", "simple.metric")]
        [InlineData("metric", "simple.", "simple.metric")]
        [InlineData("metric", ".simple.", "simple.metric")]
        [InlineData("metric", ".simple.complex.", "simple.complex.metric")]
        [InlineData("metric", ".simple..complex.", "simple..complex.metric")]
        public void TestPrefix_WithVariousFormats(string metric, string prefix, string expected)
        {
            // Arrange
            var subject = new StandardEndpoint();
            var fakeSink = new FakeSink();
            subject.AddSink(fakeSink);

            subject.Prefix = prefix;

            // Act
            subject.Record<Counting>(metric, 1);

            // Assert
            fakeSink.Metrics.Should().Contain(x => x.StartsWith($"{expected}:"));
        }
    }
}