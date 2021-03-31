using System;

namespace MonoPaint
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new MonoPaint())
                game.Run();
        }
    }
}
