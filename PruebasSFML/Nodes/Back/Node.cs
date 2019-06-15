using System;
using System.Collections.Generic;
using SFML.Audio;
using SFML.Window;
using SFML.System;
using SFML.Graphics;
using PruebasSFML.System;

namespace PruebasSFML.Game.Back
{
    public class Node
    {
        public static uint count;
        public readonly float factor = 0.01f;
        private float worldLimits = 10f;
        private Vector2u WindowSize;
        private float frictionFactor = 0.09f;
        private GameTime time;


        public float Mass
        {
            get
            {
                return Shape.Scale.X;
            }
        }

        public Vector2f ActualPosition, Acceleration, Velocity;
        public Shape Shape { get; private set; }
        public List<Node> Neighbours { get; private set; }
        public int Identificator { get; private set; }

        static Node()
        {
            count = 0;
        }

        public Node(Shape shape, Vector2f position, int id, Vector2u size, GameTime time)
        {
            Shape = shape;
            Shape.Position = position;
            Neighbours = new List<Node>();
            Identificator = id;
            WindowSize = size;
            this.time = time;
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

        public void Update(Vector2f acceleration)
        {
            Acceleration = acceleration;
            Update();
        }

        public void Update()
        {
            CheckBoundaries();
            ApplyFriction();
            UpdatePosition();
        }

        private void ApplyFriction()
        {
            Vector2f direction = -Vector2fLibrary.Normalize(Velocity);

            Acceleration += direction * (Mass * frictionFactor);
        }

        private void UpdatePosition()
        {
            Velocity += Acceleration * time.DeltaTime;
            Shape.Position += Velocity * time.DeltaTime;
        }

        private void CheckBoundaries()
        {
            if (Shape.Position.X < worldLimits) Shape.Position = new Vector2f(worldLimits, Shape.Position.Y);
            if (Shape.Position.X > WindowSize.X - worldLimits) Shape.Position = new Vector2f(WindowSize.X - worldLimits, Shape.Position.Y);
            if (Shape.Position.Y < worldLimits) Shape.Position = new Vector2f(Shape.Position.X, worldLimits);
            if (Shape.Position.Y > WindowSize.Y - worldLimits) Shape.Position = new Vector2f(Shape.Position.X, WindowSize.Y - worldLimits);
        }
    }
}
