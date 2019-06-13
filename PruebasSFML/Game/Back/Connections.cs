using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Audio;
using SFML.Window;
using SFML.System;
using SFML.Graphics;

namespace PruebasSFML.Game.Back
{
    class Connection : IEquatable<Connection>
    {
        private int _identification;
        public Node First { get; set; }
        public Node Second { get; set; }

        public Connection(Node first, Node second)
        {
            First = first;
            Second = second;
            _identification = (first.Identificator > second.Identificator) ?
                Int32.Parse(first.Identificator.ToString() + second.Identificator.ToString()) :
                Int32.Parse(second.Identificator.ToString() + 0.ToString() + first.Identificator.ToString());
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Connection);
        }

        public bool Equals(Connection other)
        {
            return other != null &&
                   (
                       (EqualityComparer<Node>.Default.Equals(First, other.First) && EqualityComparer<Node>.Default.Equals(Second, other.Second)) 
                       ||
                       (EqualityComparer<Node>.Default.Equals(First, other.Second) && EqualityComparer<Node>.Default.Equals(Second, other.First))
                   );
        }

        public override int GetHashCode()
        {
            return _identification;
        }

        public static bool operator ==(Connection left, Connection right)
        {
            return EqualityComparer<Connection>.Default.Equals(left, right);
        }

        public static bool operator !=(Connection left, Connection right)
        {
            return !(left == right);
        }
    }
}
