
namespace Logic
{
    internal record Position : IPosition
    {
        #region IPosition

        public double x { get; init; }
        public double y { get; init; }

        #endregion IPosition

        /// <summary>
        /// Creates new instance of <seealso cref="IPosition"/> and initialize all properties
        /// </summary>
        public Position(double posX, double posY)
        {
            x = posX;
            y = posY;
        }

        public Position LimitPosition(double tableWidth, double tableHeight, double diameter)
        {
            double xLimited = x;
            double yLimited = y;

            // Ograniczenia dla lewego górnego rogu
            if (x < 0) xLimited = 0;
            if (y < 0) yLimited = 0;

            // Ograniczenia dla prawej/dolnej krawędzi (x + średnica <= rozmiar stołu)
            if (x > tableWidth - diameter) xLimited = tableWidth - diameter;
            if (y > tableHeight - diameter) yLimited = tableHeight - diameter;

            return new Position(xLimited, yLimited);
        }
    }
}
