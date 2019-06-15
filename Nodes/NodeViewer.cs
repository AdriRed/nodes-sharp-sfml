using System;
using System.Collections.Generic;
using SFML.Audio;
using SFML.Window;
using SFML.System;
using SFML.Graphics;
using PruebasSFML.System;
using PruebasSFML.Game.Back;

namespace Nodes
{
    public class NodeViewer : GameLoop
    {
        List<Node> Nodes;
        List<Connection> Connections;
        private readonly Color FillColor = new Color(239, 35, 60), OutlineColor = new Color(237, 242, 244), ConnectionColor = new Color(159, 162, 178, 75);
        private const float OutlineThickness = 0.5f;
        private const uint NodesCount = 50;
        private const float MinSize = 3, MaxSize = 6;
        private const float AttractionConst = 0.00001f;
        Random rd;

        public NodeViewer(uint windowWidth, uint windowHeight, string windowTitle, Color backColor) : base (windowWidth, windowHeight, windowTitle, backColor)
        {

        }

        public override void LoadContent()
        {
            DebugUtility.LoadContent();

            Nodes = new List<Node>();
            rd = new Random();
            Connections = new List<Connection>();
        }

        public override void Initialize()
        {
            for (int i = 0; i < NodesCount; i++)
            {
                List<Tuple<uint, uint>> limits = new List<Tuple<uint, uint>>(2);

                /*limits.Add( new Tuple<uint, uint>( (uint)Math.Ceiling(10f), (uint)Math.Floor(Window.Size.X - 10f) ) );
                limits.Add( new Tuple<uint, uint>( (uint)Math.Ceiling(10f), (uint)Math.Floor(Window.Size.Y - 10f) ) );

                int x = rd.Next((int)limits[0].Item1, (int)limits[0].Item2);
                int y = rd.Next((int)limits[1].Item1, (int)limits[1].Item1);*/

                int x = rd.Next((int)Math.Ceiling(10f), (int)Math.Floor(Window.Size.X - 10f));
                int y = rd.Next((int)Math.Ceiling(10f), (int)Math.Floor(Window.Size.Y - 10f)); 

                Shape nodeshape = new CircleShape(1);
                nodeshape.Origin = new Vector2f(1, 1);
                nodeshape.FillColor = FillColor;
                nodeshape.OutlineColor = OutlineColor;
                nodeshape.OutlineThickness = OutlineThickness;

                Nodes.Add(new Node(nodeshape, new Vector2f(x, y), i, Window.Size, GameTime));
            }

            foreach (Node target in Nodes)
            {
                int neighbours = rd.Next(0, (int) (Nodes.Count - Nodes.Count * (1f / 3f)));

                for (int j = 0; j < neighbours; j++)
                {
                    Node selectedNode;
                    selectedNode = Nodes[rd.Next(Nodes.Count)];

                    if (selectedNode != target && !target.Neighbours.Contains(selectedNode))
                    {
                        target.AddNeighbour(selectedNode);
                        selectedNode.AddNeighbour(target);
                    }
                }
            }

            foreach (Node target in Nodes)
            {
                SetSize(target);

                foreach (Node neighbor in target.Neighbours)
                {
                    Connection newConnection = new Connection(target, neighbor);
                    if (!Connections.Contains(newConnection)) Connections.Add(newConnection);
                }
            }
        }

        void SetSize(Node target)
        {
            float difference = MaxSize - MinSize;
            float rank = target.Neighbours.Count / (Nodes.Count * (1f/ 3f));
            float size = MinSize + rank * difference;
            target.Shape.Scale = new Vector2f(size, size);

        }

        public override void Update(GameTime gameTime)
        {
            foreach (Node item in Nodes)
            {
                Vector2f acceleration = new Vector2f(0, 0);
                
                foreach (Node target in item.Neighbours)
                {
                    if (target != item)
                    {
                        Vector2f localAcceleration = target.Shape.Position - item.Shape.Position;
                        float distance = (float)Vector2fLibrary.SqDist(new Vector2f(0, 0), localAcceleration);
                        localAcceleration = Vector2fLibrary.Normalize(localAcceleration);

                        float strength = (AttractionConst * target.Shape.Scale.X * item.Shape.Scale.X) / distance;

                        acceleration += localAcceleration * strength;
                    }
                }


                item.Update(rd, acceleration);
            }
        }

        public override void Draw(GameTime gameTime)
        {

            foreach (Connection item in Connections)
            {
                Vertex[] line = new Vertex[2];

                line[0] = new Vertex(item.First.Shape.Position, ConnectionColor);
                line[1] = new Vertex(item.Second.Shape.Position, ConnectionColor);

                Window.Draw(line, PrimitiveType.Lines);
            }


            foreach (Node item in Nodes)
            {
                Window.Draw(item.Shape);
                //Text txt = new Text( Vector2fLibrary.GetMagnitude(item.Velocity).ToString("0.000") , DebugUtility.Font, 8);
                //txt.Color = Color.Black;
                //txt.Position = item.Shape.Position;
                //Window.Draw(txt);
            }

            //DrawCinematics(Nodes);
            //DebugUtility.DrawPerformanceData(this, Color.Black);
        }

        void DrawCinematics(List<Node> Nodes)
        {
            foreach (Node item in Nodes)
            {
                Vertex[] velocity = new Vertex[2], acceleration = new Vertex[2];

                velocity[0] = new Vertex(item.Shape.Position, Color.Black);
                velocity[1] = new Vertex(item.Shape.Position + item.Velocity * 10, Color.Black);

                Window.Draw(velocity, PrimitiveType.Lines);

                acceleration[0] = new Vertex(item.Shape.Position, Color.Blue);
                acceleration[1] = new Vertex(item.Shape.Position + item.Acceleration * 10, Color.Blue);

                Window.Draw(acceleration, PrimitiveType.Lines);
            }
        }

    }
}
