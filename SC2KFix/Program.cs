using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;

namespace SC2KFix
{
    internal class Program
    {
        static private void RegisterGame(string name = "Mayor Cool", string company = "Cool Company")
        {
            Console.WriteLine("Registering the game in registry...");
            const string userRoot = "HKEY_CURRENT_USER";
            const string subkey = "\\Software\\Maxis\\SimCity 2000\\";
            string gamePath = Directory.GetCurrentDirectory();

            Registry.SetValue(userRoot + subkey + "REGISTRATION", "Mayor Name", name);
            Registry.SetValue(userRoot + subkey + "REGISTRATION", "Company Name", company);
            Registry.SetValue(userRoot + subkey + "Localize", "Language", "USA");
            Registry.SetValue(userRoot + subkey + "Options", "Speed", "00000001", Microsoft.Win32.RegistryValueKind.DWord);
            Registry.SetValue(userRoot + subkey + "Options", "Sound", "00000001", Microsoft.Win32.RegistryValueKind.DWord);
            Registry.SetValue(userRoot + subkey + "Options", "Music", "00000001", Microsoft.Win32.RegistryValueKind.DWord);
            Registry.SetValue(userRoot + subkey + "Options", "AutoGoto", "00000001", Microsoft.Win32.RegistryValueKind.DWord);
            Registry.SetValue(userRoot + subkey + "Options", "AutoBudget", "00000000", Microsoft.Win32.RegistryValueKind.DWord);
            Registry.SetValue(userRoot + subkey + "Options", "Disasters", "00000000", Microsoft.Win32.RegistryValueKind.DWord);
            Registry.SetValue(userRoot + subkey + "Options", "AutoSave", "00000001", Microsoft.Win32.RegistryValueKind.DWord);
            Registry.SetValue(userRoot + subkey + "Paths", "Home", gamePath);
            Registry.SetValue(userRoot + subkey + "Paths", "Graphics", gamePath + "\\Bitmaps");
            Registry.SetValue(userRoot + subkey + "Paths", "Music", gamePath + "\\Sounds");
            Registry.SetValue(userRoot + subkey + "Paths", "Data", gamePath + "\\Data");
            Registry.SetValue(userRoot + subkey + "Paths", "Goodies", gamePath + "\\Goodies");
            Registry.SetValue(userRoot + subkey + "Paths", "SaveGame", gamePath + "\\Cities");
            Registry.SetValue(userRoot + subkey + "Paths", "Scenarios", gamePath + "\\Scenario");
            Registry.SetValue(userRoot + subkey + "Version", "SimCity 2000", "00000100", Microsoft.Win32.RegistryValueKind.DWord);
            Registry.SetValue(userRoot + subkey + "Windows", "Display", "8 1");
            Registry.SetValue(userRoot + subkey + "Windows", "Color Check", "00000000", Microsoft.Win32.RegistryValueKind.DWord);
            Registry.SetValue(userRoot + subkey + "Windows", "Last Color Depth", "00000020", Microsoft.Win32.RegistryValueKind.DWord);

            Console.WriteLine("Registered successfully :-)");
        }

        static private bool Patch()
        {
            Console.WriteLine("Running patcher...");

            if (!File.Exists(Directory.GetCurrentDirectory() + "\\bspatch.exe"))
            {
                Console.WriteLine("The bspatch.exe file is missing! Unable to patch.");
                return false;
            }

            if (!File.Exists(Directory.GetCurrentDirectory() + "\\SIMCITY.patch"))
            {
                Console.WriteLine("The SIMCITY.patch file is missing! Unable to patch.");
                return false;
            }

            if (!File.Exists(Directory.GetCurrentDirectory() + "\\SIMCITY.exe"))
            {
                Console.WriteLine("SIMCITY.exe file is missing! Make sure to run this tool from the game directory.");
                return false;
            }

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Directory.GetCurrentDirectory() + "\\bspatch.exe",
                    Arguments = "SIMCITY.exe SIMCITY_PATCHED.exe SIMCITY.patch",
                    UseShellExecute = false,
                    RedirectStandardOutput = false,
                    CreateNoWindow = true,
                    WorkingDirectory = Directory.GetCurrentDirectory()
                }
            };

            proc.Start();

            Console.WriteLine("Game patched and saved as SIMCITY_PATCHED.exe");

            return true;
        }

        static void Main(string[] args)
        {
            bool isPatchOk = true;

            Console.WriteLine("Neo\'s SimCity 2000 Special Edition fix tool");
            Console.WriteLine("https://github.com/kasperfm\n\n");

            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case "register":
                        RegisterGame(args[1] ?? "Mayor Cool", args[2] ?? "Cool Company");
                        break;

                    case "patch":
                        isPatchOk = Patch();
                        break;

                    default:
                        RegisterGame();
                        isPatchOk = Patch();
                        break;
                }
            } else {
                RegisterGame();
                isPatchOk = Patch();
            }

            if (isPatchOk)
            {
                Console.WriteLine("Build your city and have fun!");
            }

            Console.ReadKey();
        }
    }
}
