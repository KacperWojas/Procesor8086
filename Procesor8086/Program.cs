namespace Procesor8086
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new RergisterInput());
        }
    }
}