using OpenTK.Mathematics;

namespace OTKPlayground.Rendering
{

    public class Triangle
    {

        private float[] vertices;

        public Triangle(Vector3 a, Vector3 b, Vector3 c)
        {
            vertices = new float[9] { a.X, a.Y, a.Z, b.X, b.Y, b.Z, c.X, c.Y, c.Z };
        }

        public float[] Vertices => vertices;

    }

}