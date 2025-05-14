
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

        public IPosition CurrentPosition => new Position(
           _dataBall.GetPosition().x,
           _dataBall.GetPosition().y
       );

        public IVector Velocity => _dataBall.Velocity;

        public void Move()
        {
            HandleWallCollision();
            _dataBall.MoveStep(); // musisz mieć metodę `MoveStep()` w IBall/Data.Ball
        }

        public void SetVelocity(IVector v)
        {
            _dataBall.SetVelocity(v);
        }

        private void HandleWallCollision()
        {
            var velocity = _dataBall.Velocity;
            var position = _dataBall.GetPosition();

            double x = position.x;
            double y = position.y;
            double d = Diameter;

            double vx = velocity.x;
            double vy = velocity.y;

            if (x <= 0 || x + d >= _tableWidth)
                vx = -vx;
            if (y <= 0 || y + d >= _tableHeight)
                vy = -vy;

            _dataBall.SetVelocity(new Vector(vx, vy));
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
