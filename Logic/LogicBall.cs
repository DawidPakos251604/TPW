
namespace Logic
{
    internal class Ball : IBall
    {
        private double _tableWidth;
        private double _tableHeight;
        private double _diameter;
        public Ball(Data.IBall ball, double tableWidth, double tableHeight, double ballDiameter)
        {
            _tableWidth = tableWidth;
            _tableHeight = tableHeight;
            _diameter = ballDiameter;
            ball.NewPositionNotification += RaisePositionChangeEvent;
        }

        #region IBall

        public event EventHandler<IPosition>? NewPositionNotification;

        #endregion IBall

        #region private

        private void RaisePositionChangeEvent(object? sender, Data.IVector e)
        {
            var position = new Position(e.x, e.y);
            position = position.LimitPosition(_tableWidth, _tableHeight, _diameter);
            NewPositionNotification?.Invoke(this, position);
        }

        public void UpdateTableSettings(double width, double height, double diameter)
        {
            _tableWidth = width;
            _tableHeight = height;
            _diameter = diameter;
        }


        #endregion private
    }
}
