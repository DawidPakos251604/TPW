using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Data
{
    internal class DataImplementation : DataAbstractAPI
    {
        #region ctor

        public DataImplementation()
        {
            // inicjalizacja loggera z katalogiem i nazwą pliku
            string logFolder = @"C:\Logs2";
            string baseLogFileName = System.IO.Path.Combine(logFolder, "diagnostics_log.txt");
            _logger = new DiagnosticsLogger(baseLogFileName);
        }

        #endregion ctor

        #region DataAbstractAPI

        private readonly DiagnosticsLogger _logger;

        // Słownik przechowujący czas ostatniego logowania dla każdej piłki
        private readonly Dictionary<IBall, DateTime> _lastLoggedTimes = new();
        private readonly object _logLock = new();

        public override void Start(int numberOfBalls, Action<IVector, IBall> upperLayerHandler)
        {
            if (Disposed)
                throw new ObjectDisposedException(nameof(DataImplementation));
            if (upperLayerHandler == null)
                throw new ArgumentNullException(nameof(upperLayerHandler));

            Random random = new Random();
            double minDistance = 50; // średnica piłki + margines

            for (int i = 0; i < numberOfBalls; i++)
            {
                Vector startingPosition = new Vector(0, 0);
                bool positionFound = false;

                // Sprawdzanie kolizji z już istniejącymi piłkami
                while (!positionFound)
                {
                    startingPosition = new Vector(random.Next(50, 350), random.Next(50, 300));

                    positionFound = true;
                    foreach (Ball existingBall in BallsList)
                    {
                        double distance = Math.Sqrt(Math.Pow(startingPosition.x - existingBall.GetPosition().x, 2) +
                                                    Math.Pow(startingPosition.y - existingBall.GetPosition().y, 2));
                        if (distance < minDistance)  // kolizja - szukaj innej pozycji
                        {
                            positionFound = false;
                            break;
                        }
                    }
                }

                Vector velocity = new Vector(random.Next(50, 100), random.Next(50, 100));
                double weight = random.Next(5, 10);
                double diameter = 4 * weight;
                Ball newBall = new Ball(startingPosition, velocity, diameter, weight);

                // Podpinamy event powiadomień pozycji piłki do loggera z throttlingiem
                newBall.NewPositionNotification += (sender, position) =>
                {
                    if (sender is IBall ball)
                    {
                        var now = DateTime.UtcNow;

                        lock (_logLock)
                        {
                            if (_lastLoggedTimes.TryGetValue(ball, out var lastTime))
                            {
                                if ((now - lastTime) < TimeSpan.FromMilliseconds(500))
                                    return; // za wcześnie, pomijamy log
                            }

                            _lastLoggedTimes[ball] = now;
                        }

                        _logger.LogBallState(ball, position, ball.Velocity);
                    }
                };

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
                    BallsList.Clear();
                    lock (_logLock)
                    {
                        _lastLoggedTimes.Clear();
                    }
                    _logger.Dispose();
                }
                Disposed = true;
            }
        }

        public override void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable

        #region private

        private bool Disposed = false;
        private readonly List<Ball> BallsList = new();

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
