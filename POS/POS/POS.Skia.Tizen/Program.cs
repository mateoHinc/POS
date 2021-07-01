using Tizen.Applications;
using Uno.UI.Runtime.Skia;

namespace POS.Skia.Tizen
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new TizenHost(() => new POS.App(), args);
            host.Run();
        }
    }
}
