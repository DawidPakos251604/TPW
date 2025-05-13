using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    internal struct Vector
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        // Dodawanie wektorów
        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.X + v2.X, v1.Y + v2.Y);
        }

        // Odejmowanie wektorów
        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v1.X - v2.X, v1.Y - v2.Y);
        }

        // Mnożenie przez skalar
        public static Vector operator *(Vector v, double scalar)
        {
            return new Vector(v.X * scalar, v.Y * scalar);
        }

        // Obliczanie długości wektora
        public double Length => Math.Sqrt(X * X + Y * Y);

        // Normalizacja wektora
        public Vector Normalize()
        {
            double length = Length;
            return length > 0 ? new Vector(X / length, Y / length) : new Vector(0, 0);
        }

        // Obliczanie iloczynu skalarnego
        public double DotProduct(Vector v)
        {
            return X * v.X + Y * v.Y;
        }
    }
}

