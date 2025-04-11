
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

        public abstract void InitializeLogicParameters(double width, double height, double diameter);

        #region Layer API

        public static readonly Dimensions GetDimensions = new(50.0, 500.0, 500.0);

        public abstract void Start(int numberOfBalls, Action<IPosition, IBall> upperLayerHandler);

        #region IDisposable

        public abstract void Dispose();

        #endregion IDisposable

        #endregion Layer API

        #region private

        private static Lazy<LogicAbstractAPI> modelInstance = new Lazy<LogicAbstractAPI>(() => new LogicImplementation());

        #endregion private
    }
    /// <summary>
    /// Immutable type representing table dimensions
    /// </summary>
    /// <param name="BallDimension"></param>
    /// <param name="TableHeight"></param>
    /// <param name="TableWidth"></param>
    /// <remarks>
    /// Must be abstract
    /// </remarks>
    public record Dimensions(double BallDimension, double TableHeight, double TableWidth);

    public interface IPosition
    {
        double x { get; init; }
        double y { get; init; }
    }

    public interface IBall
    {
        event EventHandler<IPosition> NewPositionNotification;
    }
}
