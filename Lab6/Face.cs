using System.Collections.Generic;

namespace Lab6
{
    public class Face
    {
        public List<Vertex> Vertices { get; private set; }
        public Face(List<Vertex> vertices)
        {
            Vertices = vertices;
        }
    }
}