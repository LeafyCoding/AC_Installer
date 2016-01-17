// -----------------------------------------------------------
// This program is private software, based on C# source code.
// To sell or change credits of this software is forbidden,
// except if someone approves it from AC_Installer INC. team.
// -----------------------------------------------------------
// Copyrights (c) 2016 AC_Installer INC. All rights reserved.
// -----------------------------------------------------------

#region

using System;
using System.Diagnostics;

#endregion

namespace AC_Installer
{
    internal static class Program
    {
        public static string InstallDir = string.Empty;

        private static void Main()
        {
            Console.Title = "AuraCore Installer 🍌";

            Console.WriteLine("================================");
            Functions.ColoredWrite(ConsoleColor.Magenta, "AuraCore Installer", true);
            Console.WriteLine("================================");
            Checks.Internet();
            Checks.AuraCore();
            Console.WriteLine("================================");
            Checks.CppRedist();
            Checks.DxVersion();
            Checks.NetVersion();
            Console.WriteLine("================================");
            Checks.GameDirectory();
            Console.WriteLine("================================");
            Checks.Acu();
            Console.WriteLine("================================");
            Console.WriteLine("INSTALLATION FINISHED -- Press any key to close...");
            Console.ReadKey();
            var startInfo = new ProcessStartInfo {WorkingDirectory = InstallDir, FileName = $"{InstallDir}\\ACU.exe"};
            Process.Start(startInfo);
            Environment.Exit(0);
        }
    }
}