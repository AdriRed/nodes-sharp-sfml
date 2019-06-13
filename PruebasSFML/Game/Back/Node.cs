using System;
using System.Collections.Generic;
using SFML.Audio;
using SFML.Window;
using SFML.System;
using SFML.Graphics;

namespace PruebasSFML.Game.Back
{
    public class Node
    {
        public static uint count;
        public readonly float factor = 0.01f;
        private float limits = 20f;
        private Vector2u WindowSize;

        public Vector2f ActualPosition, Acceleration, Velocity;
        public Shape Shape { get; private set; }
        public List<Node> Neighbours { get; private set; }
        public int Identificator { get; private set; }

        static Node()
        {
            count = 0;
        }

        public Node(Shape shape, Vector2f position, int id, Vector2u size)
        {
            Shape = shape;
            Shape.Position = position;
            Neighbours = new List<Node>();
            Identificator = id;
            WindowSize = size;
            /*
            _minwidth = limits[0].Item1;
            _maxwidth = limits[0].Item2;
            _minheight = limits[1].Item1;
            _maxheight = limits[1].Item2;*/

            count++;
        }

        public void AddNeighbour(Node neighbor)
        {
            Neighbours.Add(neighbor);
        }

        public void Update(Random rd, Vector2f acceleration)
        {
            Acceleration = acceleration;
            Update(rd);
        }

        public void Update(Random rd)
        {
            CheckBoundaries();
            UpdateVelocity(rd);
        }

        private void CheckBoundaries()
        {
            if (Shape.Position.X < limits) Acceleration.X += limits - Shape.Position.X;
            if (Shape.Position.X > WindowSize.X - limits) Acceleration.X += WindowSize.X - limits - Shape.Position.X;
            if (Shape.Position.Y < limits) Acceleration.Y += limits - Shape.Position.Y;
            if (Shape.Position.Y > WindowSize.X - limits) Acceleration.Y += WindowSize.X - limits - Shape.Position.Y;
        }

        private void UpdateVelocity(Random rd)
        {
            //Acceleration += new Vector2f((float)(rd.NextDouble() * 2 - 1), (float)(rd.NextDouble() * 2 - 1)) * factor;
            Velocity += Acceleration;
            float magnitude = (float)Math.Sqrt(Math.Pow(Velocity.X, 2) + Math.Pow(Velocity.Y, 2));
            Velocity = new Vector2f(Velocity.X / magnitude, Velocity.Y / magnitude) * factor;
            Shape.Position += Velocity;
        }
    }
}
