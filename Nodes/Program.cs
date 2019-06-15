using System;
using SFML.Audio;
using SFML.Window;
using SFML.System;
using SFML.Graphics;
using PruebasSFML.Game;

namespace PruebasSFML
{
    class Program
    {
        public static void Main(string[] args)
        {
            NodeViewer g = new NodeViewer(600, 600, "nodes", new Color(237, 242, 244));
            g.Run();
        }
    } 
}