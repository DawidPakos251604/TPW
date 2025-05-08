
namespace Logic
{
    public abstract class LogicAbstractAPI : IDisposable
    {
        #region Layer Factory

        public static LogicAbstractAPI GetLogicLayer()
        {
            return modelInstance.Value;
        }

        #endregion Layer Factory

        public abstract void InitializeLogicParameters(double width, double height);

        #region Layer API

        public abstract void Start(int numberOfBalls, Action<IPosition, IBall> upperLayerHandler);

        #region IDisposable

        public abstract void Dispose();

        #endregion IDisposable

        #endregion Layer API

        #region private

        private static Lazy<LogicAbstractAPI> modelInstance = new Lazy<LogicAbstractAPI>(() => new LogicImplementation());

        #endregion private
    }

    public interface IPosition
    {
        double x { get; init; }
        double y { get; init; }
    }

    public interface IBall
    {
        event EventHandler<IPosition> NewPositionNotification;
        double Diameter { get; }
        double Weight { get; }
    }
}
