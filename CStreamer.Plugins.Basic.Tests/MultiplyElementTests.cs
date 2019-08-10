namespace CStreamer.Plugins.Basic.Tests
{
    using CStreamer.Plugins.Basic;
    using Xunit;

    public class MultiplyElementTests
    {
        [Theory]
        [InlineData(1,1,1)]
        public void ShouldMultiply(double multiplier, double value, double result)
        {
            var element = new MultiplyElement
            {
                Multiplier = multiplier
            };

            Assert.Equal(result, element.Transform(value));
        }
    }
}
