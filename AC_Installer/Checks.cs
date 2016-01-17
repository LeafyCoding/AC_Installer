// -----------------------------------------------------------
// This program is private software, based on C# source code.
// To sell or change credits of this software is forbidden,
// except if someone approves it from AC_Installer INC. team.
// -----------------------------------------------------------
// Copyrights (c) 2016 AC_Installer INC. All rights reserved.
// -----------------------------------------------------------

#region

#region

using System;
using System.IO;
using System.Threading;

#endregion

// ReSharper disable InvertIf

#endregion

namespace AC_Installer
{
    internal static class Checks
    {
        public static void Acu()
        {
            Console.Write("[Downloading ACU] ");
            Thread.Sleep(250);
            if (Functions.DownloadAcu())
            {
                Functions.ColoredWrite(ConsoleColor.Green, "OK", true);
            }
            else
            {
                Functions.ColoredWrite(ConsoleColor.Red, "ERROR", true);
            }
        }

        public static void AuraCore()
        {
            Console.Write("[Checking AuraCore connectivity] ");
            Thread.Sleep(250);
            Functions.CheckForAuraCoreConnection();
        }

        public static void CppRedist()
        {
            Console.Write("[Checking C++ Redistributables] ");
            Thread.Sleep(250);
            if (Functions.CheckCppRedist())
            {
                Functions.ColoredWrite(ConsoleColor.Green, "OK", true);
            }
            else
            {
                Functions.ColoredWrite(ConsoleColor.Red, "ERROR", true);
                Functions.KillMe("C++ redistributable not found.");
            }
        }

        public static void DxVersion()
        {
            Console.Write("[Checking DirectX version] ");
            try
            {
                var dxVersion = Functions.CheckDxVersion();

                if (dxVersion < 9)
                {
                    Functions.KillMe($"Insufficient DirectX version ({dxVersion}).");
                }
                else
                {
                    Functions.ColoredWrite(ConsoleColor.Green, "OK", true);
                }
            }
            catch (Exception ex)
            {
                Functions.KillMe($"{ex.GetType()}: {ex.Message}");
            }

            if (File.Exists("dxv.xml"))
            {
                File.Delete("dxv.xml");
            }
        }

        public static void GameDirectory()
        {
            Console.Write("[Checking game installation] ");

            var validInstall = false;
            do
            {
                if (InstallationDir())
                {
                    validInstall = true;
                }
            } while (!validInstall);
        }

        public static void NetVersion()
        {
            Console.Write("[Checking .NET version] ");
            Thread.Sleep(250);
            if (Functions.CheckNetVersion())
            {
                Functions.ColoredWrite(ConsoleColor.Green, "OK", true);
            }
            else
            {
                Functions.ColoredWrite(ConsoleColor.Red, "ERROR", true);
                Functions.KillMe(".NET 4.5 not found.");
            }
        }

        private static bool InstallationDir()
        {
            Functions.ColoredWrite(ConsoleColor.DarkCyan,
                "Drag your MW2 installation folder to this window and press enter.", true);
            var dir = Console.ReadLine();
            Console.Write("[Checking game files] ");
            Thread.Sleep(200);
            if (!string.IsNullOrEmpty(dir))
            {
                var goodFiles = 0;
                var files = Directory.GetFiles(dir.Replace("\"", ""), "*.*", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    if (file.EndsWith("localization.txt") || file.EndsWith("iw_00.iwd"))
                    {
                        goodFiles++;
                    }
                }
                if (goodFiles == 2)
                {
                    if (Functions.GetDirectorySize(dir) >= 7.5e+8)
                    {
                        Functions.ColoredWrite(ConsoleColor.Green, "OK", true);
                        Program.InstallDir = dir.Replace("\"", "");
                        return true;
                    }
                }
            }
            Functions.ColoredWrite(ConsoleColor.Red, "ERROR: Invalid MW2 directory.", true);
            return false;
        }

        public static void Internet()
        {
            Console.Write("[Checking Internet connectivity] ");
            Thread.Sleep(250);
            if (Functions.CheckForInternetConnection())
            {
                Functions.ColoredWrite(ConsoleColor.Green, "OK", true);
            }
            else
            {
                Functions.ColoredWrite(ConsoleColor.Red, "ERROR", true);
                Functions.KillMe("No internet connection was available.");
            }
        }
    }
}