
namespace Data
{
    ///  Two dimensions immutable vector
    public record Vector : IVector
    {
        #region IVector

        /// The X component of the vector.
        public double x { get; init; }

        /// The Y component of the vector.
        public double y { get; init; }

        #endregion IVector

        /// Creates new instance of <seealso cref="Vector"/> and initialize all properties
        public Vector(double XComponent, double YComponent)
        {
            x = XComponent;
            y = YComponent;
        }
    }
}
