using OpenTK.Graphics.OpenGL;

namespace OTKPlayground
{

    public class VertexBuffer : IDisposable
    {
        public static readonly int MinVertexCount = 1;

        private readonly VertexInfo info;

        private readonly int handle;
        private readonly int vertexCount;

        private readonly bool isStatic;

        private bool disposed;

        public VertexBuffer(VertexInfo info, int vertexCount, bool isStatic = false)
        {
            disposed = false;
            if(vertexCount < MinVertexCount)
            {
                throw new ArgumentOutOfRangeException(nameof(vertexCount));
            }

            this.info = info;
            this.vertexCount = vertexCount;
            this.isStatic = isStatic;

            BufferUsageHint usage = BufferUsageHint.StreamDraw;
            if(isStatic)
            {
                usage = BufferUsageHint.StaticDraw;
            }

            handle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, handle);
            GL.BufferData(BufferTarget.ArrayBuffer, vertexCount * info.ByteSize, IntPtr.Zero, usage);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        ~VertexBuffer()
        {
            Dispose();
        }

        public int Handle => handle;
        public bool Static => isStatic;
        public VertexInfo Info => info;

        public void SetData<T>(T[] data, int count) where T : struct
        {
            if(typeof(T) != info.Type)
            {
                throw new ArgumentException("Data type '"+typeof(T).Name+"' doesn't match this buffer's data type ('"+info.Type.Name+"').");
            }

            if(data == null || data.Length <= 0)
            {
                throw new ArgumentException("Data received is null or empty.");
            }

            if(count <= 0 || count > vertexCount || count > data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, handle);
            GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, count * info.ByteSize, data);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void Dispose()
        {
            if(disposed)
            {
                return;
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(handle);

            disposed = true;
            GC.SuppressFinalize(this);
        }

    }

}