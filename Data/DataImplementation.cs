using System;
using System.Diagnostics;

namespace Data
{
    internal class DataImplementation : DataAbstractAPI
    {
        #region ctor

        public DataImplementation()
        {
            //MoveTimer = new Timer(Move, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(10));
        }

        #endregion ctor

        #region DataAbstractAPI

        public override void Start(int numberOfBalls, Action<IVector, IBall> upperLayerHandler)
        {
            if (Disposed)
                throw new ObjectDisposedException(nameof(DataImplementation));
            if (upperLayerHandler == null)
                throw new ArgumentNullException(nameof(upperLayerHandler));
            Random random = new Random();




            for (int i = 0; i < numberOfBalls; i++)
            {
                Vector startingPosition = new(random.Next(100, 400 - 100), random.Next(100, 400 - 100));
                Vector velocity = new(random.Next(3, 5), random.Next(3, 5));
                double diameter = 50;
                double weight = 5;
                Ball newBall = new(startingPosition, velocity, diameter, weight, 400, 400);
                upperLayerHandler(startingPosition, newBall);
                BallsList.Add(newBall);
            }
        }

        #endregion DataAbstractAPI

        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                {
                    foreach (var ball in BallsList)
                    {
                        ball.Stop(); // zatrzymujemy wątki
                    }
                    BallsList.Clear();
                }
                Disposed = true;
            }
            else
            {
                throw new ObjectDisposedException(nameof(DataImplementation));
            }
        }


        public override void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable

        #region private

        //private bool disposedValue;
        private bool Disposed = false;

        //private readonly Timer MoveTimer;
        //private Random RandomGenerator = new();
        private List<Ball> BallsList = [];

     

        #endregion private

        #region TestingInfrastructure

        [Conditional("DEBUG")]
        internal void CheckBallsList(Action<IEnumerable<IBall>> returnBallsList)
        {
            returnBallsList(BallsList);
        }

        [Conditional("DEBUG")]
        internal void CheckNumberOfBalls(Action<int> returnNumberOfBalls)
        {
            returnNumberOfBalls(BallsList.Count);
        }

        [Conditional("DEBUG")]
        internal void CheckObjectDisposed(Action<bool> returnInstanceDisposed)
        {
            returnInstanceDisposed(Disposed);
        }

        #endregion TestingInfrastructure
    }
}
