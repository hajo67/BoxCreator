// Created by Hans-Jörg Schmid
// Licensed under MIT license

using netDxf;

namespace BoxCreator.BoxCreator
{
    using Geometry;

    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new ApplicationWindow());
        }
    }
}
