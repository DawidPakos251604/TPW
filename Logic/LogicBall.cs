
namespace Logic
{
    internal class Ball : IBall
    {
        private double _tableWidth;
        private double _tableHeight;
        private readonly Data.IBall _dataBall;
        public double Diameter => _dataBall.Diameter;
        public double Weight => _dataBall.Weight;

        public Ball(Data.IBall ball, double tableWidth, double tableHeight)
        {
            _dataBall = ball;
            _tableWidth = tableWidth;
            _tableHeight = tableHeight;
            ball.NewPositionNotification += RaisePositionChangeEvent;
        }

        #region IBall

        public event EventHandler<IPosition>? NewPositionNotification;

        #endregion IBall

        #region private

        private void RaisePositionChangeEvent(object? sender, Data.IVector e)
        {
            var position = new Position(e.x, e.y);
            position = position.LimitPosition(_tableWidth, _tableHeight, _dataBall.Diameter);
            NewPositionNotification?.Invoke(this, position);
        }

        public void UpdateTableSettings(double width, double height)
        {
            _tableWidth = width;
            _tableHeight = height;
        }

        #endregion private
    }
}
