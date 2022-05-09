using OpenTK.Graphics.OpenGL;

namespace OTKPlayground
{

    public class VertexArray : IDisposable
    {

        private readonly VertexBuffer vertexBuffer;

        private readonly int handle;

        private readonly bool isStatic;

        private bool disposed;

        public VertexArray(VertexBuffer vertexBuffer)
        {
            disposed = false;
            this.vertexBuffer = vertexBuffer ?? throw new ArgumentNullException(nameof(vertexBuffer));
            
            int byteSize = vertexBuffer.Info.ByteSize;
            VertexAttribute[] attributes = vertexBuffer.Info.Attributes;

            handle = GL.GenVertexArray();
            GL.BindVertexArray(handle);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer.Handle);

            for(int i = 0; i < attributes.Length; ++i)
            {
                GL.VertexAttribPointer(attributes[i].Index, attributes[i].ComponentCount, VertexAttribPointerType.Float, false, byteSize, attributes[i].Offset);
                GL.EnableVertexAttribArray(attributes[i].Index);
            }

            GL.BindVertexArray(0);
        }

        ~VertexArray()
        {
            Dispose();
        }

        public VertexBuffer Buffer => vertexBuffer;
        public int Handle => handle;
        public bool Static => isStatic;

        public void Dispose()
        {
            if(disposed)
            {
                return;
            }

            GL.BindVertexArray(0);
            GL.DeleteVertexArray(handle);

            disposed = true;
            GC.SuppressFinalize(this);
        }

    }

}