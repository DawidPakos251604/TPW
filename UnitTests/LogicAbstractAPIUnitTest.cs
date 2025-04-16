using Logic;

namespace UnitTests
{
    [TestClass]
    public class LogicAbstractAPIUnitTest
    {
        [TestMethod]
        public void SingletonTestMethod()
        {
            LogicAbstractAPI instance1 = LogicAbstractAPI.GetLogicLayer();
            LogicAbstractAPI instance2 = LogicAbstractAPI.GetLogicLayer();
            Assert.AreSame< LogicAbstractAPI>(instance1, instance2);
        }
    }
}
