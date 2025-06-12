
using Data;

namespace Logic
{
    internal class Ball : IBall
    {
        private readonly Data.IBall _dataBall;
        private double _tableWidth;
        private double _tableHeight;

        public Ball(Data.IBall ball, double tableWidth, double tableHeight)
        {
            _dataBall = ball;
            _tableWidth = tableWidth;
            _tableHeight = tableHeight;
            _dataBall.NewPositionNotification += RaisePositionChangeEvent;
        }

        public event EventHandler<IPosition>? NewPositionNotification;
        public double Diameter => _dataBall.Diameter;
        public double Weight => _dataBall.Weight;

        internal void NotifyBallCollision(Logic.Ball otherBall)
        {
         
            _dataBall.NotifyBallCollision(otherBall._dataBall);
        }


        public IPosition CurrentPosition => new Position(
           _dataBall.GetPosition().x,
           _dataBall.GetPosition().y
       );

        public IVector Velocity => _dataBall.Velocity;

        public void Move(double deltaTime)
        {
            HandleWallCollision();
            _dataBall.MoveStep(deltaTime);
        }

        public void SetVelocity(IVector v)
        {
            _dataBall.SetVelocity(v);
        }

        private readonly object _collisionLock = new();

        private void HandleWallCollision()
        {
            lock (_collisionLock)
            {
                var velocity = _dataBall.Velocity;
                var position = _dataBall.GetPosition();

             

                double x = position.x;
                double y = position.y;
                double d = Diameter;

                double vx = velocity.x;
                double vy = velocity.y;

                if (x <= 0)
                {
                    vx = Math.Abs(vx);
                    _dataBall.SetPosition(new Data.Vector(0, y));
                    _dataBall.NotifyWallCollision("Left");
                }
                else if (x + d >= _tableWidth)
                {
                    vx = -Math.Abs(vx);
                    _dataBall.SetPosition(new Data.Vector(_tableWidth - d, y));
                    _dataBall.NotifyWallCollision("Right");
                }

                if (y <= 0)
                {
                    vy = Math.Abs(vy);
                    _dataBall.SetPosition(new Data.Vector(x, 0));
                    _dataBall.NotifyWallCollision("Top");
                }
                else if (y + d >= _tableHeight)
                {
                    vy = -Math.Abs(vy);
                    _dataBall.SetPosition(new Data.Vector(x, _tableHeight - d));
                    _dataBall.NotifyWallCollision("Bottom");
                }

                _dataBall.SetVelocity(new Vector(vx, vy));
            }
        }

        private void RaisePositionChangeEvent(object? sender, IVector e)
        {
            var position = new Position(e.x, e.y)
                .LimitPosition(_tableWidth, _tableHeight, _dataBall.Diameter);

            NewPositionNotification?.Invoke(this, position);
        }


        public void UpdateTableSettings(double width, double height)
        {
            _tableWidth = width;
            _tableHeight = height;
        }
    }

}
