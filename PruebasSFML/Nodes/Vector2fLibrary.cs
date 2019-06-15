using System;
using SFML.System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebasSFML.Game
{
    static class Vector2fLibrary
    {
        public readonly static Vector2f Zero = new Vector2f(0, 0);
        public static double Dist(Vector2f First, Vector2f Second)
        {
            double distance = Math.Sqrt(SqDist(First, Second));

            return distance;
        }

        public static double SqDist(Vector2f First, Vector2f Second)
        {
            double sqdistance;

            sqdistance = Math.Pow(Second.X - First.X, 2) + Math.Pow(Second.Y - First.Y, 2);
            
            return sqdistance;
        }

        public static Vector2f Normalize (Vector2f vector)
        {
            float magnitude = (float)GetMagnitude(vector);
            if (magnitude == 0)
            {
                return Zero;
            } else
            {
                return vector / magnitude;
            }
        }

        public static double GetMagnitude(Vector2f vector)
        {
            double magnitude;

            magnitude = Dist(Zero, vector);

            return magnitude;
        }

        public static Vector2f SetMagnitude(Vector2f vector, float magnitude)
        {
            Vector2f result;

            result = Normalize(vector) * magnitude;

            return result;
        }

    }
}
