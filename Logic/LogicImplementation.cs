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

        private readonly List<Ball> balls = new();

        #region ctor

        public LogicImplementation() : this(null)
        { }

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
