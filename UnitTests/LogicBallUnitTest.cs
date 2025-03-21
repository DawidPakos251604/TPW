using Data;

namespace UnitTests
   
{
    [TestClass]
    public sealed class LogicBallUnitTest
    {
        [TestMethod]
    public void MoveTestMethod()
    {
      //DataBallFixture dataBallFixture = new DataBallFixture();
      Ball newInstance = new(new Vector(0, 0), new Vector(1, 1));
      int numberOfCallBackCalled = 0;
      newInstance.NewPositionNotification += (sender, position) => { Assert.IsNotNull(sender); Assert.IsNotNull(position); numberOfCallBackCalled++; };
      newInstance.Move(new Vector(1, 1));
      Assert.AreEqual<int>(1, numberOfCallBackCalled);
    }

    #region testing instrumentation

    private class DataBallFixture : Data.IBall
    {
      public Data.IVector Velocity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

      public event EventHandler<Data.IVector>? NewPositionNotification;

      internal void Move()
      {
        NewPositionNotification?.Invoke(this, new VectorFixture(0.0, 0.0));
      }
    }

    private class VectorFixture : Data.IVector
    {
      internal VectorFixture(double X, double Y)
      {
        x = X; y = Y;
      }

      public double x { get; init; }
      public double y { get; init; }
    }

    #endregion testing instrumentation
  }
    }

