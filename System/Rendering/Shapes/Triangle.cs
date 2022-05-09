using OpenTK.Mathematics;

namespace OTKPlayground.Rendering
{

    public class Triangle : IDrawable
    {
        
        private readonly float[] vertices;

        private Color4 color;
        
        public Triangle(Vector3 a, Vector3 b, Vector3 c)
        {
            vertices = new float[21]
            {
                a.X, a.Y, a.Z, 1f, 1f, 1f, 1f,
                b.X, b.Y, b.Z, 1f, 1f, 1f, 1f,
                c.X, c.Y, c.Z, 1f, 1f, 1f, 1f
            };

        }
        
        public float[] Vertices => vertices;

        public Color4 Color
        {
            get => color;
            set
            {
                if(color == value)
                {
                    return;
                }

                color = value;
                for(int vertex = 0; vertex < 3; vertex++)
                {
                    vertices[7 * vertex + 3] = value.R;
                    vertices[7 * vertex + 4] = value.G;
                    vertices[7 * vertex + 5] = value.B;
                    vertices[7 * vertex + 6] = value.A;
                }

            }

        }

        void IDrawable.Draw()
        {
            
        }

    }

}