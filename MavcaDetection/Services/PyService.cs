﻿using IronPython.Hosting;
using MavcaDetection.Constants;
using Microsoft.Scripting.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MavcaDetection.Services
{
    public class PyService
    {
        private readonly ScriptEngine _Engine;
        private readonly ScriptSource _ScriptSource;
        public int TablewareProcessId { get; set; }
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
                searchPaths.Add($"{BaseDirectory}");
                //searchPaths.Add($"{BaseDirectory}\\mavca-venv\\Lib\\site-packages");
                //searchPaths.Add($"{BaseDirectory}\\mavca-venv\\Lib");
                _Engine.SetSearchPaths(searchPaths);
                _ScriptSource = _Engine.CreateScriptSourceFromFile(Source);
            }
        }

        /// <summary>
        /// deprecated
        /// </summary>
        //public bool RunScript(List<string> args)
        //{
        //    if (args != null)
        //    {
        //        _Engine.GetSysModule().SetVariable("argv", args);
        //    }
        //    if (_ScriptSource == null)
        //    {
        //        return false;
        //    }
        //    var scope = _Engine.CreateScope();
        //    _ScriptSource.Execute(scope);
        //    DisplayOutput();
        //    return true;
        //}

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

        public Task RunScript(string violation, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var info = new ProcessStartInfo();
                //info.WorkingDirectory = $"{BaseDirectory}";
                Environment.CurrentDirectory = $"{BaseDirectory}";
                if (violation == DetectionTypeConstant.Phone)
                {
                    info.FileName = @"main_tracking_mobile.exe";
                }
                else if (violation == DetectionTypeConstant.Hand)
                {
                    info.FileName = @"main_tracking.exe";
                }
                else if (violation == DetectionTypeConstant.TableWare)
                {
                    info.FileName = @"main.exe";
                }
                info.CreateNoWindow = true;
                info.RedirectStandardInput = true;
                info.RedirectStandardOutput = true;
                info.StandardOutputEncoding = Encoding.UTF8;
                using (var process = new Process())
                {
                    process.StartInfo = info;
                    process.Start();
                    //var activateCommand = $"{BaseDirectory}/mavca-venv/Scripts/activate.bat";
                    //process.StandardInput.WriteLine(activateCommand);
                    //var changeDirectoryCommand = $"cd {BaseDirectory}";
                    //process.StandardInput.WriteLine(changeDirectoryCommand);
                    //if (violation == DetectionTypeConstant.Phone)
                    //{
                    //    process.StandardInput.WriteLine("main_tracking_mobile.exe");
                    //}
                    //else if (violation == DetectionTypeConstant.Hand)
                    //{
                    //    process.StandardInput.WriteLine("main_tracking.exe");
                    //}
                    //else if (violation == DetectionTypeConstant.TableWare)
                    //{
                    //    process.StandardInput.WriteLine("main.exe");
                    //}
                    // read multiple output lines
                    while (!process.StandardOutput.EndOfStream)
                    {
                        var line = process.StandardOutput.ReadLine();
                        Console.WriteLine(line);
                    }
                }
            }, cancellationToken);
        }

        public bool TerminateProcessByName(string name)
        {
            try
            {
                var processes = Process.GetProcessesByName(name);
                foreach (var process in processes)
                {
                    process.Kill();
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool TerminateAllProcess()
        {
            try
            {
                var processes = Process.GetProcesses();
                foreach (var process in processes)
                {
                    process.Kill();
                }
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
            

        }

        public bool TerminateProcessById(int id)
        {
            try
            {
                var process = Process.GetProcessById(id);
                process.Kill();
                return true;
            }
            catch (Exception e)
            {
                return false;
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
