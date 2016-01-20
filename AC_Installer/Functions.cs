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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Threading;
using System.Xml;
using Microsoft.Win32;

#endregion

namespace AC_Installer
{
    internal static class Functions
    {
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (client.OpenRead("http://www.google.com"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public static long GetDirectorySize(string folder)
        {
            var files = Directory.GetFiles(folder.Replace("\"", ""), "*.*", SearchOption.AllDirectories);
            long size = 0;
            foreach (var name in files)
            {
                var info = new FileInfo(name);
                size += info.Length;
            }
            return size;
        }

        private static bool IsWebsiteUp(Uri uri)
        {
            try
            {
                var request = WebRequest.Create(uri);
                request.Method = "HEAD";
                var response = (HttpWebResponse) request.GetResponse();
                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                KillMe($"{ex.GetType()}: {ex.Message}");
                return false;
            }
        }

        public static void ColoredWrite(ConsoleColor color, string text, bool newLine)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            var writer = newLine ? text + Environment.NewLine : text;
            Console.Write(writer);
            Console.ForegroundColor = originalColor;
        }

        public static void SemiColoredWrite(ConsoleColor color, string coloredText, string noColorText, bool newLine)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(coloredText);
            Console.ForegroundColor = originalColor;
            var writer = newLine ? noColorText + Environment.NewLine : noColorText;
            Console.Write(writer);
        }

        public static void KillMe(string reason)
        {
            ColoredWrite(ConsoleColor.Red, reason, true);
            Console.WriteLine("Press any key to close...");
            Console.ReadKey();
            Environment.Exit(0);
        }

        [SuppressMessage("ReSharper", "InvertIf")]
        public static void CheckForAuraCoreConnection()
        {
            var apiConnection = IsWebsiteUp(new Uri("http://api.auracore.net/"));
            var dlConnection = IsWebsiteUp(new Uri("http://dl.auracore.net/updater/"));

            if (!apiConnection)
            {
                ColoredWrite(ConsoleColor.Red, "ERROR", true);
                KillMe("API connection could not be made.");
            }
            if (!dlConnection)
            {
                ColoredWrite(ConsoleColor.Red, "ERROR", true);
                KillMe("Download server connection could not be made.");
            }

            ColoredWrite(ConsoleColor.Green, "OK", true);
        }

        public static bool CheckCppRedist()
            => Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\DevDiv\\VC\\Servicing\\11.0") != null;

        public static int CheckDxVersion()
        {
            using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\DirectX"))
            {
                var versionStr = key?.GetValue("Version").ToString();
                if (string.IsNullOrEmpty(versionStr)) return 0;

                var versionComponents = versionStr.Split('.');
                if (versionComponents.Length <= 1) return 0;

                int directXVersion;
                if (int.TryParse(versionComponents[1], out directXVersion))
                {
                    return directXVersion;
                }
            }
            return 0;
        }

        public static bool CheckNetVersion() => Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full") != null;

        public static bool DownloadAcu()
        {
            try
            {
                new WebClient().DownloadFile(new Uri("http://dl.auracore.net/updater/acu/files?get=ACU.exe"), $"{Program.InstallDir}\\ACU.exe");
                return true;
            }
            catch (Exception ex)
            {
                ColoredWrite(ConsoleColor.Red, $"{ex.GetType()}: {ex.Message}", true);
                return false;
            }
        }
    }
}