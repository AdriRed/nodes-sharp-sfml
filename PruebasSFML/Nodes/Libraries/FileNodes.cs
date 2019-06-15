using System;
using System.Collections.Generic;
using SFML.Audio;
using SFML.Window;
using SFML.System;
using SFML.Graphics;
using PruebasSFML.System;
using System.IO;

namespace PruebasSFML.Nodes.Libraries
{
    class FileNodes
    {

        private StreamReader sr;
        public List<Tuple<int, int>> Connections { get; set; }
        public int greaterNode { get; private set; }


        public FileNodes(string relativepath, char splitchar)
        {
            string filepath = Path.GetFullPath(relativepath);
            Connections = new List<Tuple<int, int>>();
            greaterNode = 0;
            try
            {
                sr = new StreamReader(filepath);

                while (sr.Peek() > 0)
                {
                    string line = sr.ReadLine();
                    if (line.Length > 0 && line[0] != '%')
                    {
                        string[] connections = line.Split(splitchar);

                        foreach (string item in connections)
                        {
                            int parseditem;
                            if (Int32.TryParse(item, out parseditem))
                            {
                                if (parseditem > greaterNode) greaterNode = Int32.Parse(item);
                            }
                        }


                        Connections.Add(new Tuple<int, int>(Int32.Parse(connections[0]), Int32.Parse(connections[1])));
                    }
                }

            } catch (FileNotFoundException exc)
            {

            } finally
            {
                sr.Close();
            }
        }

    }
}
