using Logic;

namespace UnitTests
{
    [TestClass]
    public class LogicAbstractAPIUnitTest
    {
        //Error

        //[TestMethod]
        //public void LogicConstructorTestMethod()
        //{
        //    LogicAbstractAPI instance1 = LogicAbstractAPI.GetLogicLayer();
        //    LogicAbstractAPI instance2 = LogicAbstractAPI.GetLogicLayer();
        //    Assert.AreSame(instance1, instance2);
        //    instance1.Dispose();
        //    Assert.ThrowsException<ObjectDisposedException>(() => instance2.Dispose());
        //}

        [TestMethod]
        public void GetDimensionsTestMethod()
        {
            Assert.AreEqual<Dimensions>(new(10.0, 10.0, 10.0), LogicAbstractAPI.GetDimensions);
        }
    }
}
