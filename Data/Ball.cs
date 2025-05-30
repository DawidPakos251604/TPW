﻿
namespace Data
{
    internal class Ball : IBall
    {
        #region ctor

        internal Ball(Vector initialPosition, Vector initialVelocity, double diameter, double weight)
        {
            Position = initialPosition;
            Velocity = initialVelocity;
            Diameter = diameter;
            Weight = weight;
        }

        #endregion ctor

        #region IBall

        public event EventHandler<IVector>? NewPositionNotification;

        public IVector Velocity { get; set; }

        public double Diameter { get; }

        public double Weight { get; }

        private readonly object _positionLock = new();
        public void MoveStep()
        {
            lock (_positionLock)
            {
                Position = new Vector(Position.x + Velocity.x, Position.y + Velocity.y);
                RaiseNewPositionChangeNotification();
            }
        }

        public void SetVelocity(IVector newVelocity)
        {
            lock (_positionLock)
            {
                Velocity = newVelocity;
            }
        }

        #endregion IBall

        public IVector GetPosition()
        {
            lock (_positionLock)
            {
                return new Vector(Position.x, Position.y);
            }
        }

        #region private

        private Vector Position;

        private void RaiseNewPositionChangeNotification()
        {
            var handler = NewPositionNotification;
            if (handler != null)
            {
                Task.Run(() => handler.Invoke(this, GetPosition()));
            }
        }
        #endregion private
    }
}
