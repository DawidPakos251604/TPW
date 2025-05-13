using System;
using System.Numerics;
using System.Threading.Tasks;


namespace Data
{
    internal class Ball : IBall
    {
        #region ctor

        internal Ball(Vector initialPosition, Vector initialVelocity, double diameter, double weight, double boundaryWidth, double boundaryHeight)
        {
            {
                Position = initialPosition;
                Velocity = initialVelocity;
                Diameter = diameter;
                Weight = weight;
                Width = boundaryWidth;
                Height = boundaryHeight;

                _ = Task.Run(MoveLoop);
            }
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
        private readonly double Width;
        private readonly double Height;
        private bool isRunning = true;

        private async Task MoveLoop()
        {
            while (isRunning)
            {
                MoveOneStep();
                RaiseNewPositionChangeNotification();
                await Task.Delay(10); // Δt = 10 ms
            }
        }

        private void MoveOneStep()
        {
            Position = new Vector(Position.x + Velocity.x, Position.y + Velocity.y);
        }

        private void RaiseNewPositionChangeNotification()
        {
            NewPositionNotification?.Invoke(this, Position);
        }

        internal void Stop()
        {
            isRunning = false;
        }




        #endregion private
    }
}
