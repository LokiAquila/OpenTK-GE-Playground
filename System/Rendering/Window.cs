using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;

namespace OTKPlayground.Rendering
{

    public class Window : GameWindow
    {

        private DateTime lastFrame;
        private int framesElapsed;
        private float timer;

        private Shader shader;

        private int vertexBufferObject;
        private int vertexArrayObject;

        public Window(GameWindowSettings gSettings, NativeWindowSettings nSettings) : base(gSettings, nSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            CenterWindow();

            Triangle triangle = new(new Vector3(0f, -0.5f, 0f), new Vector3(0.5f, -0.5f, 0f), new Vector3(0f, 0.5f, 0f));
            GL.ClearColor(Color4.Black);

            vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, 9 * sizeof(float), triangle.Vertices, BufferUsageHint.StaticDraw);

            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
            shader.Use();

            lastFrame = DateTime.Now;
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            timer += (float)(DateTime.Now - lastFrame).TotalMilliseconds;
            lastFrame = DateTime.Now;
            framesElapsed++;

            if(timer >= 1000f)
            {
                Console.WriteLine(framesElapsed+" FPS recorded");
                timer -= 1000f;
                framesElapsed = 0;
            }

            if(KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            base.OnUpdateFrame(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            shader.Use();

            GL.BindVertexArray(vertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
            
        }

        protected override void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.DeleteBuffer(vertexBufferObject);
            GL.DeleteVertexArray(vertexArrayObject);

            GL.DeleteProgram(shader.Handle);

            base.OnUnload();
        }

    }

}