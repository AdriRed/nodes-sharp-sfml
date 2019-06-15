using System;
using System.Collections.Generic;
using SFML.Audio;
using SFML.Window;
using SFML.System;
using SFML.Graphics;
using PruebasSFML.System;
using PruebasSFML.Nodes.Libraries;

namespace Pkmn.Clases
{
    class Pokemon : Node
    {
        public Pokemon(Shape shape, Vector2f position, int id, Vector2u windowsize, GameTime time) : base(shape, position, id, windowsize, time)
        {

        }
        public string Name { get; set; }


    }
}
