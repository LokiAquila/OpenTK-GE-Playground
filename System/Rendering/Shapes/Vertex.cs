using OpenTK.Mathematics;

namespace OTKPlayground
{

    public struct VertexAttribute
    {

        private readonly string name;
        private readonly int index;
        private readonly int componentCount;
        private readonly int offset;

        public VertexAttribute(string name, int index, int componentCount, int offset)
        {
            this.name = name;
            this.index = index;
            this.componentCount = componentCount;
            this.offset = offset;
        }

        public string Name => name;
        public int Index => index;
        public int ComponentCount => componentCount;
        public int Offset => offset;

    }

    public class VertexInfo
    {

        private Type type;
        private int sizeInBytes;
        private VertexAttribute[] attributes;

        public VertexInfo(Type type, params VertexAttribute[] attributes)
        {
            this.type = type;
            this.attributes = attributes;
            sizeInBytes = 0;

            for(int i = 0; i < attributes.Length; ++i)
            {
                sizeInBytes += attributes[i].ComponentCount * sizeof(float);
            }

        }

        public Type Type => type;
        public int ByteSize => sizeInBytes;
        public VertexAttribute[] Attributes => attributes;

    }

    public struct Vertex
    {

        private readonly Vector3 position;
        private readonly Color4 color;

        public static readonly VertexInfo Info = new(typeof(Vertex), new VertexAttribute("position", 0, 3, 0), new VertexAttribute("color", 1, 4, 3 * sizeof(float)));

        public Vertex(Vector3 position, Color4 color)
        {
            this.position = position;
            this.color = color;
        }

        public Vector3 Position => position;
        public Color4 Color => color;

    }

}