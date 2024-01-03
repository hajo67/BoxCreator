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
            CreateGeometry();
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new ApplicationWindow());
        }

        private static void CreateGeometry()
        {
            var document = new Document();
            var polyline = new Polyline(Vector2.Zero);
            using var dxfFileStream = new FileStream(@"C:\Users\goerky\Temp\test.dxf",
                FileMode.OpenOrCreate,
                FileAccess.Write);

            polyline.AddLine(new Vector2(400, 0));
            polyline.AddLine(new Vector2(400, 300));
            polyline.AddLine(new Vector2(0, 300));
            document.AddPolyline(polyline);
            document.SaveDxf(dxfFileStream);
        }
    }
}
