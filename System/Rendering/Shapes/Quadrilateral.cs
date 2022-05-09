using OpenTK.Mathematics;

namespace OTKPlayground.Rendering
{

    public class Quadrilateral : IDrawable
    {

        public static readonly int[] IndexBuffer =
        {
            0, 1, 2,
            0, 2, 3
        };

        private readonly Vertex[] vertices;
        
        private Color4 color;

        public Quadrilateral(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            color = Color4.White;
            vertices = new Vertex[4]
            {
                new Vertex(a, color),
                new Vertex(b, color),
                new Vertex(c, color),
                new Vertex(d, color)
            };

        }

        public Vertex[] Vertices => vertices;

        public Color4 Color
        {
            get => color;
            set
            {
                if (color == value)
                {
                    return;
                }

                color = value;
                for (int vertex = 0; vertex < 4; vertex++)
                {
                    vertices[vertex] = new(vertices[vertex].Position, color);
                }

            }

        }

        void IDrawable.Draw()
        {

        }

    }

}