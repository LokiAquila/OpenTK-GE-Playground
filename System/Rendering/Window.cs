using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;

namespace OTKPlayground.Rendering
{

    public class Window : GameWindow
    {

        private int framesElapsed;
        private float timer;

        private float fovY;

        private Shader shader;

        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;
        private VertexArray vertexArray;

        private readonly Color4[] tmp_Colors = {Color4.Red, Color4.Lime, Color4.Blue, Color4.Yellow, Color4.Cyan, Color4.Magenta, Color4.White};
        private int tmp_ColorIndex = 0;
        private float tmp_Rotation;
        private float tmp_Color;

        private Matrix4 view;
        private Matrix4 projection;

        //private Triangle tmp_Triangle;
        private Quadrilateral tmp_Quad;

        public Window(GameWindowSettings gSettings, NativeWindowSettings nSettings) : base(gSettings, nSettings)
        {
            /*float fovX = MathHelper.DegreesToRadians(70f);
            fovY = 2f * MathF.Atan(MathF.Tan(fovX / 2) * (Size.Y / (float)Size.X));

            Console.WriteLine("Launching game with HFOV = " + MathHelper.RadiansToDegrees(fovX) + ", VFOV = " + MathHelper.RadiansToDegrees(fovY) + ".");*/
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            IsVisible = true;

            CenterWindow();

            //tmp_Triangle = new(new Vector3(-0.5f, -0.5f, 0f), new Vector3(0.5f, -0.5f, 0f), new Vector3(0f, 0.5f, 0f));
            //triangle.Color = Color4.LightBlue;

            tmp_Quad = new(new(-0.5f, 0.5f, 0f), new(0.5f, 0.5f, 0f), new(0.5f, -0.5f, 0f), new(-0.5f, -0.5f, 0f));
            //tmp_Quad = new(new(200f, 200f, 0f), new(200f, 600f, 0f), new(600f, 600f, 0f), new(600f, 200f, 0f));
            GL.ClearColor(Color4.Black);

            vertexBuffer = new VertexBuffer(Vertex.Info, tmp_Quad.Vertices.Length);
            vertexBuffer.SetData(tmp_Quad.Vertices, tmp_Quad.Vertices.Length);

            indexBuffer = new IndexBuffer(Quadrilateral.IndexBuffer.Length, true);
            indexBuffer.SetData(Quadrilateral.IndexBuffer, Quadrilateral.IndexBuffer.Length);

            vertexArray = new VertexArray(vertexBuffer);

            shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");

            GL.UseProgram(shader.Handle);
            //int[] viewport = new int[4];
            //GL.GetInteger(GetPName.Viewport, viewport);

            /*Matrix4 transform = Matrix4.Identity;

            int transformLoc = GL.GetUniformLocation(shader.Handle, "Transform");
            GL.UniformMatrix4(transformLoc, false, ref transform);

            //int viewportSizeUniformLoc = GL.GetUniformLocation(shader.Handle, "ViewportSize");
            //GL.Uniform2(viewportSizeUniformLoc, (float)viewport[2], (float)viewport[3]);
            GL.UseProgram(0);*/

            view = Matrix4.CreateTranslation(0f, 0f, -3f);
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Size.X / (float) Size.Y, 0.1f, 1000f);
            timer = 0f;
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            timer += (float)args.Time;
            framesElapsed++;

            if(timer >= 1000f)
            {
                Console.WriteLine(framesElapsed+" FPS recorded");
                timer -= 1000f;
                framesElapsed = 0;
            }

            if(KeyboardState.IsKeyPressed(Keys.Escape))
            {
                Close();
            }
            if(KeyboardState.IsKeyPressed(Keys.E))
            {
                ++tmp_ColorIndex;
                tmp_ColorIndex %= 7;
                tmp_Quad.Color = tmp_Colors[tmp_ColorIndex];
            }
            base.OnUpdateFrame(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            tmp_Rotation += 12.5f * (float)args.Time;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.BindVertexArray(vertexArray.Handle);

            GL.UseProgram(shader.Handle);
            Matrix4 model = Matrix4.Identity * Matrix4.CreateRotationZ((float)MathHelper.DegreesToRadians(tmp_Rotation));

            int modelVar = GL.GetUniformLocation(shader.Handle, "model");
            GL.UniformMatrix4(modelVar, true, ref model);

            int viewVar = GL.GetUniformLocation(shader.Handle, "view");
            GL.UniformMatrix4(viewVar, true, ref view);

            int projectionVar = GL.GetUniformLocation(shader.Handle, "projection");
            GL.UniformMatrix4(projectionVar, true, ref projection);

            Tmp_Render((float)args.Time);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer.Handle);
            GL.DrawElements(PrimitiveType.Triangles, Quadrilateral.IndexBuffer.Length, DrawElementsType.UnsignedInt, 0);
            
            SwapBuffers();
            base.OnRenderFrame(args);
        }

        private void Tmp_Render(float time)
        {
            tmp_Color += time;
            tmp_Color %= 1f;

            tmp_Quad.Color = new Color4(1f, tmp_Color, tmp_Color, 1f);

            vertexBuffer.SetData(tmp_Quad.Vertices, tmp_Quad.Vertices.Length);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnUnload()
        {
            indexBuffer?.Dispose();
            vertexBuffer?.Dispose();
            vertexArray?.Dispose();
            
            GL.UseProgram(0);
            GL.DeleteProgram(shader.Handle);

            base.OnUnload();
        }

    }

}