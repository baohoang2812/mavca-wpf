using IronPython.Hosting;
using MavcaDetection.Constants;
using Microsoft.Scripting.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace MavcaDetection.Services
{
    public class PyService
    {
        private readonly ScriptEngine _Engine;
        private readonly ScriptSource _ScriptSource;
        private Config _config;
        private string BaseDirectory;
        public string Source { get; set; }
        public PyService()
        {
            LoadJson();
            BaseDirectory = _config.DetectionRootFolder;
            if (_Engine != null)
            {
                _Engine = Python.CreateEngine();
                ICollection<string> searchPaths = _Engine.GetSearchPaths();
                searchPaths.Add($"{BaseDirectory}\\mavca-venv");
                searchPaths.Add($"{BaseDirectory}\\mavca-venv\\Lib\\site-packages");
                searchPaths.Add($"{BaseDirectory}\\mavca-venv\\Lib");
                _Engine.SetSearchPaths(searchPaths);
                _ScriptSource = _Engine.CreateScriptSourceFromFile(Source);
            }
        }

        public bool RunScript(List<string> args)
        {
            if (args != null)
            {
               _Engine.GetSysModule().SetVariable("argv", args);
            }
            if(_ScriptSource == null)
            {
                return false;
            }
            var scope = _Engine.CreateScope();
            _ScriptSource.Execute(scope);
            DisplayOutput();
            return true;
        }

        private void LoadJson()
        {
            var json = File.ReadAllText("config.json");
            _config = JsonConvert.DeserializeObject<Config>(json);
        }

        public bool LoadConfiguration(string sourcePath)
        {
            var fileNameWithoutExt = Path.GetFileNameWithoutExtension(sourcePath);
            var fileName = Path.GetFileName(sourcePath);
            if (fileName != "mavca-detect.zip")
            {
                return false;
            }
            var currentDirectory = Directory.GetCurrentDirectory();
            var targetPath = $"{currentDirectory}\\{fileName}";
            if (File.Exists(sourcePath))
            {
                File.Copy(sourcePath, targetPath, overwrite: true);
                var extractDirectory = $"{currentDirectory}\\{fileNameWithoutExt}";
                if (!Directory.Exists(extractDirectory))
                {
                    Directory.CreateDirectory(extractDirectory);
                }
                ZipFile.ExtractToDirectory(targetPath, extractDirectory, overwriteFiles: true);
                BaseDirectory = extractDirectory.Replace(@"\", @"/");
            }
            return true;
        }

        public void RunScript(string violation)
        {
            var fileName = $"{BaseDirectory}/mavca-venv/Scripts/python.exe";
            var info = new ProcessStartInfo(fileName);
            info.FileName = "cmd.exe";
            info.CreateNoWindow = false;
            info.RedirectStandardInput = true;
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;
            using (var process = new Process())
            {
                process.StartInfo = info;
                process.Start();
                var activateCommand = $"{BaseDirectory}/mavca-venv/Scripts/activate.bat";
                process.StandardInput.WriteLine(activateCommand);
                var changeDirectoryCommand = $"cd {BaseDirectory}";
                process.StandardInput.WriteLine(changeDirectoryCommand);
                if(violation == DetectionTypeConstant.Phone)
                {
                    process.StandardInput.WriteLine("python main_tracking_mobile.py");
                }
                else if (violation == DetectionTypeConstant.Hand)
                {
                    process.StandardInput.WriteLine("python main_tracking.py");
                }
                else if(violation == DetectionTypeConstant.TableWare)
                {
                    process.StandardInput.WriteLine("python main.py");
                }
                // read multiple output lines
                while (!process.StandardOutput.EndOfStream)
                {
                    var line = process.StandardOutput.ReadLine();
                    Console.WriteLine(line);
                }
            }
        }

        private void DisplayOutput()
        {
            var eIO = _Engine.Runtime.IO;
            var errors = new MemoryStream();
            var results = new MemoryStream();
            eIO.SetErrorOutput(errors, Encoding.UTF8);
            eIO.SetOutput(results, Encoding.UTF8);
            string str(byte[] o) => Encoding.Default.GetString(o);
            Console.WriteLine("ERRORS:");
            Console.WriteLine(str(errors.ToArray()));

            Console.WriteLine("Results:");
            Console.WriteLine(str(results.ToArray()));
        }

    }
}
