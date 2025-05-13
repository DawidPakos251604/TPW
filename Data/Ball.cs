
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

        #endregion IBall

        #region private

        private Vector Position;

        public Vector GetPosition()
        {
            return Position;
        }

        private void RaiseNewPositionChangeNotification()
        {
            NewPositionNotification?.Invoke(this, Position);
        }

        internal void Move(Vector delta)
        {
            Position = new Vector(Position.x + delta.x, Position.y + delta.y);
            RaiseNewPositionChangeNotification();
        }



        #endregion private
    }
}
