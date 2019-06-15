using System;
using System.Collections.Generic;
using PruebasSFML.System;

namespace Nodes.Back
{
    public class Node
    {
        public static uint count;
        public readonly float factor = 0.01f;
        private float worldLimits = 75f;
        private Vector2u WindowSize;
        private float frictionFactor = 0.6f;
        private GameTime time;
        private float velocityLimit = 20f;


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

        public void Update(Random rd, Vector2f acceleration)
        {
            Acceleration = acceleration;
            Update(rd);
        }

        public void Update(Random rd)
        {
            //CheckBoundaries();
            //AddFriction();
            UpdateVelocity(rd);
            
        }

        private void AddFriction()
        {
            Vector2f direction = Vector2fLibrary.Normalize(Velocity);

            Acceleration += -direction * Mass;
        }

        private void CheckBoundaries()
        {
            if (Shape.Position.X < worldLimits) Acceleration.X += worldLimits - Shape.Position.X;
            if (Shape.Position.X > WindowSize.X - worldLimits) Acceleration.X += WindowSize.X - worldLimits - Shape.Position.X;
            if (Shape.Position.Y < worldLimits) Acceleration.Y += worldLimits - Shape.Position.Y;
            if (Shape.Position.Y > WindowSize.X - worldLimits) Acceleration.Y += WindowSize.X - worldLimits - Shape.Position.Y;
        }

        private void UpdateVelocity(Random rd)
        {
            //Acceleration += new Vector2f((float)(rd.NextDouble() * 2 - 1), (float)(rd.NextDouble() * 2 - 1));

            Velocity += Acceleration /** time.DeltaTime*/;

            double magnitude = Vector2fLibrary.GetMagnitude(Velocity);

            if (magnitude > velocityLimit)
            {
                Velocity = Vector2fLibrary.SetMagnitude(Velocity, velocityLimit);
            }

            Shape.Position += Velocity * time.DeltaTime;
        }
    }
}
