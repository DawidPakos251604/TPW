using System;
using System.Collections.Generic;
using System.Diagnostics;

using UnderneathLayerAPI = Data.DataAbstractAPI;

namespace Logic
{
    internal class LogicImplementation : LogicAbstractAPI
    {
        private double tableWidth;
        private double tableHeight;
        private CancellationTokenSource? _cts;
        private Task? _gameLoopTask;

        private Stopwatch _stopwatch = Stopwatch.StartNew();


        private readonly List<Ball> balls = new();

        #region ctor

        private readonly Timer LogicTimer;

        public LogicImplementation() : this(null)
        {
            LogicTimer = new Timer(LogicTick, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(10));

        }

        private void LogicTick(object? state)
        {
            var elapsed = _stopwatch.Elapsed.TotalSeconds;
            _stopwatch.Restart();

            lock (balls)
            {
                HandleBallCollisions();
                foreach (var ball in balls)
                {
                    ball.Move(elapsed);
                }
            }
        }

        private void HandleBallCollisions()
        {
            for (int i = 0; i < balls.Count; i++)
            {
                for (int j = i + 1; j < balls.Count; j++)
                {
                    var b1 = balls[i];
                    var b2 = balls[j];

                    var p1 = b1.CurrentPosition;
                    var p2 = b2.CurrentPosition;

                    double dx = p2.x - p1.x;
                    double dy = p2.y - p1.y;
                    double distanceSq = dx * dx + dy * dy;
                    double radiusSum = (b1.Diameter + b2.Diameter) / 2;

                    if (distanceSq < radiusSum * radiusSum)
                    {
                        ResolveElasticCollision(b1, b2);
                        b1.NotifyBallCollision(b2);
                    }
                }
            }
        }

        private void ResolveElasticCollision(Ball b1, Ball b2)
        {
            var v1 = b1.Velocity;
            var v2 = b2.Velocity;
            var p1 = b1.CurrentPosition;
            var p2 = b2.CurrentPosition;

            var dx = p1.x - p2.x;
            var dy = p1.y - p2.y;
            var distance = Math.Sqrt(dx * dx + dy * dy);

            if (distance == 0) return; // unikanie dzielenia przez zero

            var nx = dx / distance;
            var ny = dy / distance;

            var dvx = v1.x - v2.x;
            var dvy = v1.y - v2.y;

            double dot = dvx * nx + dvy * ny;
            if (dot > 0) return; // piłki oddalają się - brak reakcji

            double m1 = b1.Weight;
            double m2 = b2.Weight;

            double coefficient = (2 * dot) / (m1 + m2);

            b1.SetVelocity(new Data.Vector(
                v1.x - coefficient * m2 * nx,
                v1.y - coefficient * m2 * ny));

            b2.SetVelocity(new Data.Vector(
                v2.x + coefficient * m1 * nx,
                v2.y + coefficient * m1 * ny));
        }

        internal LogicImplementation(UnderneathLayerAPI? underneathLayer)
        {
            layerBellow = underneathLayer == null ? UnderneathLayerAPI.GetDataLayer() : underneathLayer;
        }

        #endregion ctor

        #region LogicAbstractAPI

        public override void Dispose()
        {
            if (Disposed)
                throw new ObjectDisposedException(nameof(LogicImplementation));

            _cts?.Cancel();

            try
            {
                _gameLoopTask?.Wait();
            }
            catch (AggregateException ex)
            {
                foreach (var inner in ex.InnerExceptions)
                {
                    
                    Debug.WriteLine($"Game loop error: {inner.Message}");
                }
            }

            layerBellow.Dispose();
            Disposed = true;
        }


        public override void InitializeLogicParameters(double width, double height)
        {
            this.tableWidth = width;
            this.tableHeight = height;

            foreach (var ball in balls)
            {
                ball.UpdateTableSettings(tableWidth, tableHeight);
            }
        }

        private void StartGameLoop()
        {
            _cts = new CancellationTokenSource();
            CancellationToken token = _cts.Token;

            _gameLoopTask = Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    lock (balls) // zabezpieczenie listy piłek
                    {
                        var elapsed = _stopwatch.Elapsed.TotalSeconds;
                        _stopwatch.Restart();

                        HandleBallCollisions();
                        foreach (var ball in balls)
                        {
                            ball.Move(elapsed);
                        }
                    }

                    await Task.Delay(10, token); // 100 FPS
                }
            }, token);
        }


        public override void Start(int numberOfBalls, Action<IPosition, IBall> upperLayerHandler)
        {
            if (Disposed)
                throw new ObjectDisposedException(nameof(LogicImplementation));
            if (upperLayerHandler == null)
                throw new ArgumentNullException(nameof(upperLayerHandler));

            balls.Clear();

            layerBellow.Start(numberOfBalls, (startingPosition, databall) =>
            {
                var logicBall = new Ball(databall, tableWidth, tableHeight);
                balls.Add(logicBall);
                upperLayerHandler(new Position(startingPosition.x, startingPosition.y), logicBall);
            });
            StartGameLoop();

        }

        #endregion LogicAbstractAPI

        #region private

        private bool Disposed = false;
        private readonly UnderneathLayerAPI layerBellow;

        #endregion private

        #region TestingInfrastructure

        [Conditional("DEBUG")]
        internal void CheckObjectDisposed(Action<bool> returnInstanceDisposed)
        {
            returnInstanceDisposed(Disposed);
        }

        #endregion TestingInfrastructure
    }
}
