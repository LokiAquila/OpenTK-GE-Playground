using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OTKPlayground.Rendering;

namespace OTKPlayground
{

    public class Engine
    {

        static void Main(string[] args)
        {
            NativeWindowSettings settings = new NativeWindowSettings()
            {
                Size = new Vector2i(1600, 900),
                Title = "Open TK Playground",
                Flags = ContextFlags.ForwardCompatible
            };

            Window window = new Window(GameWindowSettings.Default, settings);
            window.Run();
        }

    }

}