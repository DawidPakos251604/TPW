
namespace Logic
{
    internal class Ball : IBall
    {
        private double _tableWidth;
        private double _tableHeight;
        private readonly Data.IBall _dataBall;
        public double Diameter => _dataBall.Diameter;
        public double Weight => _dataBall.Weight;

        public Vector Position { get; private set; }
        public Vector Velocity { get; private set; }

        public Ball(Data.IBall ball, double tableWidth, double tableHeight)
        {
            _dataBall = ball;
            _tableWidth = tableWidth;
            _tableHeight = tableHeight;
            ball.NewPositionNotification += RaisePositionChangeEvent;

            Position = new Vector(0, 0);
            Velocity = new Vector(0, 0);
        }

        #region IBall

        public event EventHandler<IPosition>? NewPositionNotification;

        #endregion IBall

        #region private

        private void RaisePositionChangeEvent(object? sender, Data.IVector e)
        {
            // Aktualizuj dane
            Position = new Vector(e.x, e.y);
            Velocity = new Vector(_dataBall.Velocity.x, _dataBall.Velocity.y);

            // Sprawdź kolizję i zaktualizuj prędkość
            HandleWallCollision();

            // Informuj Model
            NewPositionNotification?.Invoke(this, new Position(Position.X, Position.Y));
        }

        private Vector HandleWallCollision(Vector currentPosition)
        {
            Vector newVelocity = Velocity;

            // Odbicie od lewej/prawej ściany
            if (currentPosition.X <= 0 || currentPosition.X + Diameter >= _tableWidth)
                newVelocity = new Vector(-newVelocity.X, newVelocity.Y);

            // Odbicie od góry/dół
            if (currentPosition.Y <= 0 || currentPosition.Y + Diameter >= _tableHeight)
                newVelocity = new Vector(newVelocity.X, -newVelocity.Y);

            // Zaktualizuj prędkość
            Velocity = newVelocity;

            // Zwracamy nową pozycję po ruchu (uwzględniając odbicie)
            return new Vector(currentPosition.X + Velocity.X, currentPosition.Y + Velocity.Y);
        }
        public void UpdateTableSettings(double width, double height)
        {
            _tableWidth = width;
            _tableHeight = height;
        }

        #endregion private
    }
}
