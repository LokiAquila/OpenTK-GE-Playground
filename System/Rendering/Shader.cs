using OpenTK.Graphics.OpenGL4;

namespace OTKPlayground.Rendering
{

    public class Shader
    {
        private readonly int handle;

        public Shader(string vertPath, string fragPath)
        {
            string shaderCode = File.ReadAllText(vertPath);
            int vertexShader = CreateShader(ShaderType.VertexShader, shaderCode);

            shaderCode = File.ReadAllText(fragPath);
            int fragmentShader = CreateShader(ShaderType.FragmentShader, shaderCode);

            handle = GL.CreateProgram();
            GL.AttachShader(handle, vertexShader);
            GL.AttachShader(handle, fragmentShader);

            GL.LinkProgram(handle);
            GL.GetProgram(handle, GetProgramParameterName.LinkStatus, out int code);

            if (code != (int)All.True)
            {
                throw new Exception($"Compilation of Shader Program #{handle} failed: {GL.GetProgramInfoLog(handle)}");
            }

            GL.DetachShader(handle, vertexShader);
            GL.DetachShader(handle, fragmentShader);

            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);
        }

        public int Handle => handle;

        private static int CreateShader(ShaderType type, string sourceCode)
        {
            int shader = GL.CreateShader(type);
            GL.ShaderSource(shader, sourceCode);

            GL.CompileShader(shader);
            GL.GetShader(shader, ShaderParameter.CompileStatus, out int code);

            if (code != (int)All.True)
            {
                throw new Exception($"Compilation of Shader #{shader} failed: {GL.GetShaderInfoLog(shader)}");
            }
            return shader;
        }

    }

}