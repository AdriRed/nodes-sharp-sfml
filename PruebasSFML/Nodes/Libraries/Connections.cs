using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebasSFML.Nodes.Libraries
{
    class Connection : IEquatable<Connection>
    {
        public int Id { get; private set; }
        public Node First { get; set; }
        public Node Second { get; set; }

        public Connection(Node first, Node second)
        {
            First = first;
            Second = second;
            Id = (first.Id > second.Id) ?
                Int32.Parse(first.Id.ToString() + 0.ToString() + second.Id.ToString()) :
                Int32.Parse(second.Id.ToString() + 0.ToString() + first.Id.ToString());
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
            return Id;
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
