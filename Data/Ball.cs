
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

        public event Action<IBall, string>? WallCollisionDetected;
        public event Action<IBall, IBall>? BallCollisionDetected;

        public void NotifyWallCollision(string wall)
        {
            WallCollisionDetected?.Invoke(this, wall);
        }

        public void NotifyBallCollision(IBall otherBall)
        {
            BallCollisionDetected?.Invoke(this, otherBall);
        }

        #endregion ctor

        #region IBall

        public event EventHandler<IVector>? NewPositionNotification;

        public IVector Velocity { get; set; }

        public double Diameter { get; }

        public double Weight { get; }

        private readonly object _positionLock = new();
        public void MoveStep(double deltaTime)
        {
            lock (_positionLock)
            {
                // Ruch zależny od czasu
                Position = new Vector(
                    Position.x + Velocity.x * deltaTime,
                    Position.y + Velocity.y * deltaTime
                );
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

        public void SetPosition(IVector newPosition)
        {
            lock (_positionLock)
            {
                Position = new Vector(newPosition.x, newPosition.y);
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
