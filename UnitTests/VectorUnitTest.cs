using Data;

namespace UnitTests
{
    [TestClass]
    public class VectorUnitTest
    {
        [TestMethod]
        public void ConstructorTestMethod()
        {
            Random randomGenerator = new();
            double XComponent = randomGenerator.NextDouble();
            double YComponent = randomGenerator.NextDouble();
            Vector newInstance = new(XComponent, YComponent);
            Assert.AreEqual<double>(XComponent, newInstance.x);
            Assert.AreEqual<double>(YComponent, newInstance.y);
        }
    }
}
