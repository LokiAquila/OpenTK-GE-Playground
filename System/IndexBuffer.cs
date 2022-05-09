using OpenTK.Graphics.OpenGL;

namespace OTKPlayground
{

    public class IndexBuffer : IDisposable
    {
        public static readonly int MinIndexCount = 1;

        private readonly int indexCount;
        private readonly int handle;

        private readonly bool isStatic;

        private bool disposed;

        public IndexBuffer(int indexCount, bool isStatic = false)
        {
            disposed = false;
            if(indexCount < MinIndexCount)
            {
                throw new ArgumentOutOfRangeException(nameof(indexCount));
            }

            this.indexCount = indexCount;
            this.isStatic = isStatic;

            BufferUsageHint usage = BufferUsageHint.StreamDraw;
            if(isStatic)
            {
                usage = BufferUsageHint.StaticDraw;
            }

            handle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, handle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indexCount * sizeof(int), IntPtr.Zero, usage);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        ~IndexBuffer()
        {
            Dispose();
        }

        public int Handle => handle;
        public bool Static => isStatic;

        public void SetData(int[] data, int count)
        {
            if(data == null || data.Length <= 0)
            {
                throw new ArgumentException("Data received is null or empty.");
            }

            if(count <= 0 || count > indexCount || count > data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, handle);
            GL.BufferSubData(BufferTarget.ElementArrayBuffer, IntPtr.Zero, count * sizeof(int), data);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        public void Dispose()
        {
            if(disposed)
            {
                return;
            }

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.DeleteBuffer(handle);

            disposed = true;
            GC.SuppressFinalize(this);
        }

    }

}