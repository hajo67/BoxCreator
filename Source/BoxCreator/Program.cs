// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using netDxf;

namespace BoxCreator.BoxCreator
{
    using Geometry;

    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            CreateGeometry();
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
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
