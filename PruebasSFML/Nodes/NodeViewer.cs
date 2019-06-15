using System;
using System.Collections.Generic;
using SFML.Audio;
using SFML.Window;
using SFML.System;
using SFML.Graphics;
using PruebasSFML.System;
using PruebasSFML.Game.Back;

namespace PruebasSFML.Game
{
    public class NodeViewer : GameLoop
    {
        List<Node> Nodes;
        List<Connection> Connections;
        private readonly Color FillColor = new Color(239, 35, 60), OutlineColor = new Color(237, 242, 244), ConnectionColor = new Color(159, 162, 178, 75);

        private const float OutlineThickness = 0.5f;
        private const uint NodesCount = 50;
        private const float MinSize = 3, MaxSize = 6;
        private const float ConnectionForceConst = 0.002f;
        private const float ConnectionDefaultLength = 30f;
        private const float RepulsionConst = 80f;
        private const uint MinNeighbours = 0, MaxNeighbours = 15;


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

            InitializeNodes();
            SetNeighbours();
            SetSizes();
            SetConnections();
            
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Node item in Nodes)
            {
                Vector2f acceleration = new Vector2f(0, 0);
                acceleration += RepulsionForce(item);
                acceleration += AttractionForce(item);
                item.Update(acceleration);
            }
        }

        private Vector2f AttractionForce(Node target)
        {
            Vector2f atractionAcceleration = new Vector2f(0, 0);

            foreach (Node item in target.Neighbours)
            {
                float distance = (float)Vector2fLibrary.Dist(item.Shape.Position, target.Shape.Position);
                Vector2f direction = Vector2fLibrary.Normalize(item.Shape.Position - target.Shape.Position);

                atractionAcceleration += direction * (ConnectionForceConst * (distance - ConnectionDefaultLength));

            }

            return atractionAcceleration;
        }

        private Vector2f RepulsionForce(Node target)
        {
            Vector2f repulsionAcceleration = new Vector2f(0, 0);

            foreach (Node item in Nodes)
            {
                if (item != target)
                {
                    double sqdistance = Vector2fLibrary.SqDist(target.Shape.Position, item.Shape.Position);
                    Vector2f direction = Vector2fLibrary.Normalize(target.Shape.Position - item.Shape.Position);
                    double force = RepulsionConst * item.Mass * target.Mass / sqdistance;
                    Vector2f localRepulsion = direction * (float)force;

                    repulsionAcceleration += localRepulsion;
                }
            }

            return repulsionAcceleration;
        }

        public override void Draw(GameTime gameTime)
        {
            DrawConnections();

            foreach (Node item in Nodes)
            {
                Window.Draw(item.Shape);
            }

            DebugUtility.DrawPerformanceData(this);
        }

        private void DrawConnections()
        {
            foreach (Connection item in Connections)
            {
                Vertex[] line = new Vertex[2];

                line[0] = new Vertex(item.First.Shape.Position, ConnectionColor);
                line[1] = new Vertex(item.Second.Shape.Position, ConnectionColor);

                Window.Draw(line, PrimitiveType.Lines);

                //Vector2f halfWay = (item.Second.Shape.Position - item.First.Shape.Position) / 2 + item.First.Shape.Position;

                //Text txt = new Text(item.GetHashCode().ToString(), DebugUtility.Font, 10);
                //txt.Position = halfWay;
                //txt.Color = DebugUtility.FontColor;

                //Window.Draw(txt);
            }
        }

        private void SetSizes()
        {
            foreach (Node item in Nodes)
            {
                SetSize(item);
            }
        }

        private void InitializeNodes()
        {
            for (int i = 0; i < NodesCount; i++)
            {
                int x = rd.Next((int)Math.Ceiling(10f), (int)Math.Floor(Window.Size.X - 10f));
                int y = rd.Next((int)Math.Ceiling(10f), (int)Math.Floor(Window.Size.Y - 10f));

                Shape nodeshape = new CircleShape(1);
                nodeshape.Origin = new Vector2f(1, 1);
                nodeshape.FillColor = FillColor;
                nodeshape.OutlineColor = OutlineColor;
                nodeshape.OutlineThickness = OutlineThickness;

                Nodes.Add(new Node(nodeshape, new Vector2f(x, y), i, Window.Size, GameTime));
            }
        }

        private void SetNeighbours()
        {
            foreach (Node target in Nodes)
            {
                int neighbours = rd.Next((int)MinNeighbours, (int)(MaxNeighbours - target.Neighbours.Count));

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
        }

        private void SetConnections()
        {
            foreach (Node target in Nodes)
            {
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
            float rank = target.Neighbours.Count / (Nodes.Count * (1f / 3f));
            float size = MinSize + rank * difference;
            target.Shape.Scale = new Vector2f(size, size);
        }

    }
}
