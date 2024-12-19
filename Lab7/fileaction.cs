using Lab7;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab7
{
    internal class fileaction
    {

        public Polyhedron LoadFromOBJ(string filePath)
        {
            List<Vertex> vertices = new List<Vertex>();
            List<Face> faces = new List<Face>();

            foreach (var line in File.ReadLines(filePath))
            {
                if (line.StartsWith("v "))
                {
                    var parts = line.Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);
                    double x = double.Parse(parts[1], CultureInfo.InvariantCulture);
                    double y = double.Parse(parts[2], CultureInfo.InvariantCulture);
                    double z = double.Parse(parts[3], CultureInfo.InvariantCulture);
                    vertices.Add(new Vertex(x, y, z));
                }
                else if (line.StartsWith("f "))
                {
                    var partsFace = line.Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);
                    int i = 0;
                    List<Vertex> tempListVertex = new List<Vertex>();
                    //foreach (var p in parts) 
                    //{
                    //    vv[i] = vertices[int.Parse(p)];
                    //    i++;

                    //}
                    for (int j = 1; j < partsFace.Length; j++) 
                    {

                        tempListVertex.Add(vertices[int.Parse(partsFace[j]) - 1]);
                    }
                    faces.Add(new Face(tempListVertex));
                   // List<Face> faceVertices = parts.Skip(1).Select(index => vertices[int.Parse(index)]);
                    //                                 //.Select(index => vertices[int.Parse(index) - 1])
                    //                                 //.ToList();
                    //faces.Add(new Face(faceVertices));
                }
            }
           
            return new Polyhedron(vertices, faces);
        }

        public void SaveToOBJ(Polyhedron polyhedron, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var vertex in polyhedron.Vertices)
                {
                    writer.WriteLine(string.Format(CultureInfo.InvariantCulture, "v {0:0.###} {1:0.###} {2:0.###}", vertex.X, vertex.Y, vertex.Z));

                }

                foreach (var face in polyhedron.Faces)
                {
                    var indices = face.Vertices.Select(v => polyhedron.Vertices.IndexOf(v) + 1);
                    writer.WriteLine("f " + string.Join(" ", indices));
                }
            }
        }
    }
}

//private void LoadFile_Click(object sender, EventArgs e)
//{
//    OpenFileDialog openFileDialog = new OpenFileDialog();
//    if (openFileDialog.ShowDialog() == DialogResult.OK)
//    {

//        pop = LoadFromOBJ(openFileDialog.FileName);
//        pnts = pop.Faces;
//        pictureBox1.Invalidate();
//    }
//}





//    }
//}
