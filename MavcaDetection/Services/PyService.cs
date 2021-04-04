using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MavcaDetection.Services
{
    public class PyService
    {
        private readonly ScriptEngine _Engine;
        private readonly ScriptSource _ScriptSource;
        public string Source { get; set; }
        public PyService()
        {
            if (_Engine != null)
            {
                _Engine = Python.CreateEngine();
                ICollection<string> searchPaths = _Engine.GetSearchPaths();
                searchPaths.Add(@"C:\Users\PC\Desktop\Capstone\mavca-detect\mavca-venv");
                searchPaths.Add(@"C:\Users\PC\Desktop\Capstone\mavca-detect\mavca-venv\Lib\site-packages");
                searchPaths.Add(@"C:\Users\PC\Desktop\Capstone\mavca-detect\mavca-venv\Lib");
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

        public void RunScript()
        {
            var info = new ProcessStartInfo(@"C:/Users/PC/Desktop/Capstone/mavca-detect/mavca-venv/Scripts/python.exe");
            info.FileName = "cmd.exe";
            info.CreateNoWindow = false;
            info.RedirectStandardInput = true;
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;
            using (var process = new Process())
            {
                process.StartInfo = info;
                process.Start();
                process.StandardInput.WriteLine(@"c:/Users/PC/Desktop/Capstone/mavca-detect/mavca-venv/Scripts/activate.bat");
                process.StandardInput.WriteLine(@"cd c:/Users/PC/Desktop/Capstone/mavca-detect");
                process.StandardInput.WriteLine("python main.py");
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
