using System;
using System.Diagnostics;

namespace Data
{
    internal class DataImplementation : DataAbstractAPI
    {
        #region ctor

        public DataImplementation()
        {
            MoveTimer = new Timer(Move, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(10));
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
            double minDistance = 30; // Minimalna odległość, aby uniknąć kolizji (średnica piłki + margines)

            for (int i = 0; i < numberOfBalls; i++)
            {
                Vector startingPosition = new Vector(0, 0); // Inicjalizacja zmiennej startingPosition
                bool positionFound = false;

                // Sprawdzanie kolizji z już istniejącymi piłkami
                while (!positionFound)
                {
                    startingPosition = new Vector(random.Next(50, 350), random.Next(50, 300));

                    // Sprawdzamy, czy nowa piłka nie koliduje z istniejącymi
                    positionFound = true;
                    foreach (Ball existingBall in BallsList)
                    {
                        double distance = Math.Sqrt(Math.Pow(startingPosition.x - existingBall.GetPosition().x, 2) + Math.Pow(startingPosition.y - existingBall.GetPosition().y, 2));
                        if (distance < minDistance)  // Jeśli odległość jest mniejsza niż minimalna odległość, zmieniamy pozycję
                        {
                            positionFound = false;
                            break;
                        }
                    }
                }

                Vector velocity = new Vector(random.Next(10, 30), random.Next(10, 30));
                double diameter = 20;
                double weight = 5;
                Ball newBall = new Ball(startingPosition, velocity, diameter, weight);  // Teraz startingPosition jest zainicjalizowane

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
                    MoveTimer.Dispose();
                    BallsList.Clear();
                }
                Disposed = true;
            }
            else
                throw new ObjectDisposedException(nameof(DataImplementation));
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

        private readonly Timer MoveTimer;
        private Random RandomGenerator = new();
        private List<Ball> BallsList = [];

        private void Move(object? x)
        {
            foreach (Ball item in BallsList)
            {
                double dx = (RandomGenerator.NextDouble() - 0.5) * 2;
                double dy = (RandomGenerator.NextDouble() - 0.5) * 2;

                item.Move(new Vector(dx, dy));
            }
        }

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
