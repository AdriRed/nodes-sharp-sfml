﻿using System;
using SFML.System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebasSFML.Game
{
    static class Vector2fLibrary
    {
        public static double Dist(Vector2f First, Vector2f Second)
        {
            double distance;

            distance = Math.Sqrt(SqDist(First, Second));

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
            Vector2f result;

            float magnitude = (float)Dist(new Vector2f(0, 0), vector);
            result = vector / magnitude;

            return result;
        }
    }
}
