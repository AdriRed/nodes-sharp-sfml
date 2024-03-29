﻿using System;
using System.Collections.Generic;
using SFML.Audio;
using SFML.Window;
using SFML.System;
using SFML.Graphics;
using PruebasSFML.System;
using PruebasSFML.Nodes.Libraries;

namespace PruebasSFML.Nodes
{
    public class NodeViewer : GameLoop
    {
        List<Node> Nodes;
        List<Connection> Connections;
        public static Color OutlineColor = new Color(237, 242, 244), 
                            ConnectionColor = new Color(0, 0, 0, 50);

        private const float OutlineThickness = 0.5f;
        private int NodesCount = 15;

        private const float ConnectionForceConst = 0.01f;
        private const float ConnectionDefaultLength = 30f;
        private const float RepulsionConst = 30f;

        private const uint MinNeighbours = 1, MaxNeighbours = 8;
        private const bool blakLines = true;

        private const float MinRadii = 2f, MaxRadii = 10f;

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
            //InitializeRandomPositionNodes();
            //SetRandomNeighbours();
            //SetConnections();
            //SetAllVisuals();

            FileNodes fn = new FileNodes(@"C:\Users\arojo\Downloads\custom-networks\flower-network.edges", ' ');
            NodesCount = fn.greaterNode + 1;
            InitializeRandomPositionNodes();
            SetConnectionsFromFile(fn);
            SetAllVisuals();
        }

        private void SetConnectionsFromFile(FileNodes fn)
        {
            foreach (Tuple<int, int> item in fn.Connections)
            {
                Connections.Add(new Connection(Nodes[item.Item1], Nodes[item.Item2]));
                Nodes[item.Item1].AddNeighbour(Nodes[item.Item2]);
                Nodes[item.Item2].AddNeighbour(Nodes[item.Item1]);
            }
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
                
                //Text txt = new Text(item.GetHashCode().ToString(), DebugUtility.Font, 10);
                //txt.Position = item.Shape.Position - new Vector2f(DefaultRadii, DefaultRadii);
                //txt.Color = DebugUtility.FontColor;

                //Window.Draw(txt);
            }

            DebugUtility.DrawPerformanceData(this);
        }

        private void DrawConnections()
        {
            Vertex[] line;
            foreach (Connection item in Connections)
            {
                line = new Vertex[2];
                if (blakLines)
                {
                    line[0] = new Vertex(item.First.Shape.Position, ConnectionColor);
                    line[1] = new Vertex(item.Second.Shape.Position, ConnectionColor);
                } else
                {
                    line[0] = new Vertex(item.First.Shape.Position, item.First.Shape.FillColor);
                    line[1] = new Vertex(item.Second.Shape.Position, item.First.Shape.FillColor);
                }
                Window.Draw(line, PrimitiveType.Lines);

                Vector2f halfWay = (item.Second.Shape.Position - item.First.Shape.Position) / 2 + item.First.Shape.Position;

                Text txt = new Text(item.GetHashCode().ToString(), DebugUtility.Font, 10);
                txt.Position = halfWay;
                txt.Color = DebugUtility.FontColor;

                Window.Draw(txt);
            }
        }

        private void SetAllVisuals()
        {
            foreach (Node item in Nodes)
            {
                SetVisuals(item);
            }
        }

        private void InitializeRandomPositionNodes()
        {
            for (int i = 0; i < NodesCount; i++)
            {
                int x = rd.Next((int)Math.Ceiling(10f), (int)Math.Floor(Window.Size.X - 10f));
                int y = rd.Next((int)Math.Ceiling(10f), (int)Math.Floor(Window.Size.Y - 10f));

                Shape nodeshape = new CircleShape(MinRadii);
                nodeshape.Origin = new Vector2f(MinRadii, MinRadii);
                nodeshape.OutlineColor = OutlineColor;
                nodeshape.OutlineThickness = OutlineThickness;

                Nodes.Add(new Node(nodeshape, new Vector2f(x, y), i, Window.Size, GameTime));
            }
        }

        private void SetRandomNeighbours()
        {
            foreach (Node target in Nodes)
            {
                //if (target.Id != 0 && target.Id != Nodes.Count - 1)
                //{
                //    target.AddNeighbour(Nodes[target.Id - 1]);
                //    target.AddNeighbour(Nodes[target.Id + 1]);
                //} else
                //{
                //    if (target.Id == 0)
                //    {
                //        target.AddNeighbour(Nodes[1]);
                //        target.AddNeighbour(Nodes[Nodes.Count - 1]);
                //    } else
                //    {
                //        target.AddNeighbour(Nodes[Nodes.Count - 2]);
                //        target.AddNeighbour(Nodes[0]);
                //    }
                //}
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
                    if (!neighbor.Neighbours.Contains(target))
                        neighbor.Neighbours.Add(target);
                    if (!Connections.Contains(newConnection))
                        Connections.Add(newConnection);
                }
            }
        }

        void SetVisuals(Node target)
        {
            float rank = (float)target.Neighbours.Count / (float)Nodes.Count;

            SetSize(target, rank);
            SetColor(target, rank);
        }

        private void SetSize(Node target, float rank)
        {
            float diference = MaxRadii - MinRadii;
            float size = MinRadii + rank * diference;
            target.Shape.Scale = new Vector2f(size, size);
        }

        private void SetColor(Node target, float rank)
        {
            target.Shape.FillColor = HSV(120 + (rank * 360), 1, 1, 0.5f);
        }

        static Color HSV(float hue, float saturation, float value, float alpha)
        {
            Color rgbcolor;
            Vector3f colorprim;

            if (hue < 0)
            {
                while (hue < 0) hue += 360;
            } else
            {
                hue = hue % 360;
            }

            float c = saturation * value;
            float x = c * (1 - Math.Abs(hue / 60f % 2 - 1));

            float m = value - c;

            if (hue < 60)
            {
                colorprim = new Vector3f(c, x, 0);
            } else if (hue < 120)
            {
                colorprim = new Vector3f(x, c, 0);
            } else if (hue < 180)
            {
                colorprim = new Vector3f(0, c, x);
            } else if (hue < 240)
            {
                colorprim = new Vector3f(0, x, c);
            } else if (hue < 300)
            {
                colorprim = new Vector3f(x, 0, c);
            } else
            {
                colorprim = new Vector3f(c, 0, x);
            }

            rgbcolor = new Color((byte)((colorprim.X + m) * 255),
                (byte)((colorprim.Y + m) * 255),
                (byte)((colorprim.Z + m) * 255),
                (byte)(alpha * 255));

            return rgbcolor;
        }


    }
}
