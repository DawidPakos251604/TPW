
namespace Data
{
    public abstract class DataAbstractAPI : IDisposable
    {
        #region Layer Factory

        public static DataAbstractAPI GetDataLayer()
        {
            return modelInstance.Value;
        }

        #endregion Layer Factory

        #region public API

        public abstract void Start(int numberOfBalls, Action<IVector, IBall> upperLayerHandler);

        #endregion public API

        #region IDisposable

        public abstract void Dispose();

        #endregion IDisposable

        #region private

        private static Lazy<DataAbstractAPI> modelInstance = new Lazy<DataAbstractAPI>(() => new DataImplementation());

        #endregion private
    }

    public interface IVector
    {
        /// The X component of the vector.
        double x { get; init; }

        /// The y component of the vector.
        double y { get; init; }
    }

    public interface IBall
    {
        event EventHandler<IVector> NewPositionNotification;

        IVector Velocity { get; set; }

        double Diameter { get; }
        double Weight { get; }
    }
}
