using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Yahoo.Yui.Compressor;
using System.Reflection;
using System.Diagnostics;
using Microsoft.Ajax.Utilities;
using System.Text.RegularExpressions;

namespace BNE.Compress.Console
{
    public class ErrorEventArgs : EventArgs
    {
        public string Details { get; set; }
        public Exception Exception { get; set; }
    }
    public class Program
    {
        private static event EventHandler<ErrorEventArgs> ErrorOccurs;
        private const int TimeoutCompressGif = 60000;

        static Program()
        {
            ErrorOccurs += Program_ErrorOccurs;
        }

        private static void OnErrorOccurs(Exception ex, string details)
        {
            var ev = ErrorOccurs;
            if (ev != null)
            {
                ev(null, new ErrorEventArgs { Exception = ex, Details = details });
            }
        }

        private static bool _initiliazed;
        static void Program_ErrorOccurs(object sender, ErrorEventArgs e)
        {
            try
            {
                var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                if (!directory.EndsWith(@"\"))
                {
                    directory += @"\errorCompressLog.txt";
                }
                else
                {
                    directory += @"errorCompressLog.txt";
                }

                using (StreamWriter writer = new StreamWriter(directory, true))
                {
                    if (!_initiliazed)
                    {
                        _initiliazed = true;
                        writer.WriteLine("--------------------------------------");
                        writer.WriteLine(string.Format("------------{0}--------------", DateTime.Now.ToString("dd/MM/yy HH:mm")));
                        writer.WriteLine("--------------------------------------");
                    }

                    if (string.IsNullOrEmpty(e.Details))
                    {
                        if (e.Exception == null)
                        {
                            writer.WriteLine("Unknown ERROR");
                            return;
                        }

                        var text = RemoveBreakLink(e.Exception.Message);
                        writer.WriteLine("Generic ERROR ### " + text);
                        return;
                    }

                    if (e.Exception == null)
                    {
                        writer.WriteLine(RemoveBreakLink(e.Details) + " ### " + "(ErrorMessage is Missing)");
                        return;
                    }

                    var value = RemoveBreakLink(e.Exception.Message);
                    writer.WriteLine(RemoveBreakLink(e.Details) + " ### " + value);

                }
            }
            catch
            {

            }

        }

        private static string RemoveBreakLink(string text)
        {
            return Regex.Replace(text, @"\r\n?|\n", " ");
        }

        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += SolveLibrary;

            if (args == null || args.Length < 1)
                throw new ArgumentOutOfRangeException("args");

            var input = args[0] ?? string.Empty;

            string output;
            if (args.Length > 1)
                output = args[1] ?? string.Empty;
            else
                output = input ?? string.Empty;

            if (!File.Exists(output))
            {
                if (!Directory.Exists(output))
                {
                    var folderPath = output.Split('\\').Reverse().Skip(1).Reverse().Aggregate((a, b) => a + @"\" + b);
                    if (!Directory.Exists(folderPath))
                    {
                        AppDomain.CurrentDomain.AssemblyResolve -= SolveLibrary;
                        var ex = new ArgumentException(string.Format("The output directory ‘{0}’ doesn’t exist", output));
                        OnErrorOccurs(ex, "On Before Start Process (output problem)");
                        throw ex;
                    }
                }
            }

            if (!Directory.Exists(input) && !File.Exists(input))
            {
                AppDomain.CurrentDomain.AssemblyResolve -= SolveLibrary;
                var ex = new ArgumentException(string.Format("{0} isn’t a known directory or file", input));
                OnErrorOccurs(ex, "On Before Start Process (input problem)");
                throw ex;
            }

            if (Directory.Exists(input))
            {
                DoProcessRecursive(input, output);
                return;
            }

            if (!File.Exists(output) && Directory.Exists(output))
            {
                if (!output.EndsWith(@"\"))
                {
                    output = output + @"\";
                }

                output = output + input.Split('\\').LastOrDefault() ?? input;
            }

            try
            {
                DoProcessInOneFIle(input, output);
            }
            catch (Exception ex)
            {
                OnErrorOccurs(ex, "On Doing the Process (Main Part)");
            }
            finally
            {
                AppDomain.CurrentDomain.AssemblyResolve -= SolveLibrary;
            }

        }

        private static void DoProcessInOneFIle(string input, string output)
        {
            if (input.EndsWith("js", StringComparison.OrdinalIgnoreCase))
            {
                CompressFileJs(input, output);
            }
            else if (input.EndsWith("css", StringComparison.OrdinalIgnoreCase))
            {
                CompressFileCSS(input, output);
            }
            else if (input.EndsWith("png", StringComparison.OrdinalIgnoreCase))
            {
                CompressFilePng(input, output);
            }
            else if (input.EndsWith("jpg", StringComparison.OrdinalIgnoreCase)
                || input.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase))
            {
                CompressFileJpg(input, output);
            }
            else if (input.EndsWith("gif", StringComparison.OrdinalIgnoreCase))
            {
                CompressFileGIF(input, output);
            }
            else
            {
                throw new ArgumentException(string.Format("{0} isn’t a known a file", input));
            }
        }

        private static void DoProcessRecursive(string input, string output)
        {
            var ag = new List<Exception>();
            int count = 1;
            foreach (var item in GetCompressions(input, output))
            {
                try
                {
                    item();
                    count++;
                }
                catch (Exception ex)
                {
                    OnErrorOccurs(ex, "On Doing the Process (Part " + count + ")");
                    System.Console.WriteLine("ERROR | " + ex.Message);
                    ag.Add(ex);
                }
            }

            AppDomain.CurrentDomain.AssemblyResolve -= SolveLibrary;

            if (ag.Count > 0)
            {
                throw new AggregateException(ag);
            }
            return;
        }

        private static IEnumerable<Action> GetCompressions(string input, string output)
        {
            yield return () => CompressDirectory(input, output, ".js", CompressFileJs);
            yield return () => CompressDirectory(input, output, ".css", CompressFileCSS);
            yield return () => CompressDirectory(input, output, ".png", CompressFilePng);
            yield return () => CompressDirectory(input, output, ".jpg", CompressFileJpg);
            yield return () => CompressDirectory(input, output, ".jpeg", CompressFileJpg);
            yield return () => CompressDirectory(input, output, ".gif", CompressFileGIF);
        }


        private static System.Reflection.Assembly SolveLibrary(object sender, ResolveEventArgs args)
        {
            string assemblyName = new AssemblyName(args.Name).Name;
            if (assemblyName.EndsWith(".resources"))
                return null;

            string dllName = assemblyName + ".dll";
            string dllFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dllName);

            var assembly = typeof(Program).Assembly;
            var resource = assembly.GetManifestResourceNames().FirstOrDefault(a => a.Equals(typeof(Program).Namespace + ".Resources." + dllName, StringComparison.OrdinalIgnoreCase)
                                                            || a.Equals(typeof(Program).Namespace + "." + dllName, StringComparison.OrdinalIgnoreCase));

            if (resource == null)
                return null;

            using (Stream s = assembly.GetManifestResourceStream(resource))
            {
                byte[] data = new byte[s.Length];
                s.Read(data, 0, data.Length);

                return Assembly.Load(data);
            }

        }

        public static void CompressDirectory(string root, string outputDirectory, string extension, Action<string, string> actionToExecute)
        {
            if (!extension.StartsWith(".", StringComparison.OrdinalIgnoreCase))
                extension = "." + extension;

            System.Console.WriteLine("Compressing all " + extension + " files within: " + root);

            if (!outputDirectory.EndsWith(@"\"))
                outputDirectory = outputDirectory + @"\";

            foreach (var file in Directory.GetFiles(root, "*" + extension))
            {
                actionToExecute(file, outputDirectory);
            }

            foreach (var directory in Directory.GetDirectories(root))
            {
                //skip .svn folders and what not
                if ((new DirectoryInfo(directory).Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                {
                    var newOuputDirectory = outputDirectory + directory.Substring(root.Length) + @"\";
                    CompressDirectory(directory, newOuputDirectory, extension, actionToExecute);
                }
            }
        }

        private static void CompressFileCSS(string input, string outputDirectory)
        {
            var name = Path.GetFileName(input);
            System.Console.WriteLine("Compressing file: " + name);

            var realName = PrepareAndGetOutputPath(outputDirectory, name);

            string content;
            using (var sr = new StreamReader(input))
            {
                content = sr.ReadToEnd();
            }

            string compressed;
            try
            {
                compressed = new CssCompressor().Compress(content);
            }
            catch (Exception ex)
            {
                OnErrorOccurs(ex, "On CompressFileCSS : " + name);
                System.Console.WriteLine("ERROR > " + name + " > (" + ex.Message + ")");

                try
                {
                    compressed = (new Minifier()).MinifyStyleSheet(content);
                }
                catch
                {
                    OnErrorOccurs(ex, "On CompressFileCSS : " + name);
                    System.Console.WriteLine("ERROR > " + name + " > (" + ex.Message + ")");
                }
                return;
            }

            if (content.Length < compressed.Length)
                return;

            using (var sw = new StreamWriter(realName, false))
            {
                sw.Write(compressed);
                sw.Flush();
            }
        }

        public static void CompressFileJs(string file, string outputDirectory)
        {
            var name = Path.GetFileName(file);
            System.Console.WriteLine("Compressing file: " + name);

            var realName = PrepareAndGetOutputPath(outputDirectory, name);

            string content;
            using (var sr = new StreamReader(file))
            {
                content = sr.ReadToEnd();
            }

            string compressed;
            try
            {
                compressed = new JavaScriptCompressor().Compress(content);
            }
            catch (Exception ex)
            {
                OnErrorOccurs(ex, "On CompressFileJS : " + name);
                System.Console.WriteLine("ERROR > " + name + " > (" + ex.Message + ")");
                return;
            }

            if (content.Length < compressed.Length)
                return;

            using (var sw = new StreamWriter(realName, false))
            {
                sw.Write(compressed);
                sw.Flush();
            }
        }

        private static string PrepareAndGetOutputPath(string targetOutput, string inputFileName)
        {
            string totalString;
            if (targetOutput.EndsWith(@"\", StringComparison.OrdinalIgnoreCase)
                || targetOutput.Split('\\').Reverse().First().IndexOf(".", StringComparison.OrdinalIgnoreCase) == -1)
            {
                if (!System.IO.Directory.Exists(targetOutput))
                {
                    System.IO.Directory.CreateDirectory(targetOutput);
                }
                totalString = targetOutput + inputFileName;
            }
            else
            {
                var dir = targetOutput.Split('\\').Reverse().Skip(1).Reverse().Aggregate((a, b) => a + @"\" + b);
                if (!System.IO.Directory.Exists(dir))
                {
                    System.IO.Directory.CreateDirectory(dir);
                }

                totalString = targetOutput;

            }
            return totalString;
        }

        private static string executableTempPng;
        private static string GetPngProcess()
        {
            if (File.Exists(executableTempPng))
                return executableTempPng;

            var pngFile = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".exe";

            var assembly = typeof(Program).Assembly;
            var exe = assembly.GetManifestResourceNames()
                           .FirstOrDefault(a => a.IndexOf("optipng.exe", StringComparison.OrdinalIgnoreCase) > -1);

            if (exe == null)
                throw new NullReferenceException("pngFile");

            using (Stream s = assembly.GetManifestResourceStream(exe))
            {
                byte[] data = new byte[s.Length];
                s.Read(data, 0, data.Length);
                File.WriteAllBytes(pngFile, data);
            }
            executableTempPng = pngFile;
            return pngFile;
        }

        private static void CompressFilePng(string input, string output)
        {
            var name = Path.GetFileName(input);
            System.Console.WriteLine("Compressing file: " + name);

            var outProcessFile = PrepareAndGetOutputPath(output, name);

            var exe = GetPngProcess();

            var psInfo = new ProcessStartInfo(exe, string.Format("-o5 -quiet -out \"{0}\" \"{1}\"", outProcessFile, input));
            psInfo.RedirectStandardOutput = true;
            psInfo.RedirectStandardError = true;
            psInfo.UseShellExecute = false;
            psInfo.CreateNoWindow = true;

            using (var pRes = new Process())
            {
                pRes.StartInfo = psInfo;

                pRes.Start();
                pRes.WaitForExit();

                if (pRes.ExitCode == 0)
                    return;

                try
                {
                    var err = pRes.StandardError.ReadToEnd();
                    OnErrorOccurs(new Exception("External Error in optipng.exe"), "On CompressFilePNG : " + err);
                    System.Console.WriteLine("ERROR > " + name + " > (" + err + ")");
                }
                catch (Exception ex)
                {
                    OnErrorOccurs(new Exception("Error in log the External Error in optipng.exe"), "On CompressFilePNG : " + ex.Message);
                    System.Console.WriteLine("Error in ERROR > " + name + " > (" + ex.Message + ")");
                }
            }
        }


        private static string executableTempGif;
        private static string GetGifProcess()
        {
            if (File.Exists(executableTempGif))
                return executableTempGif;

            var gifFile = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".exe";

            var assembly = typeof(Program).Assembly;
            var exe = assembly.GetManifestResourceNames()
                           .FirstOrDefault(a => a.IndexOf("gifsicle.exe", StringComparison.OrdinalIgnoreCase) > -1);

            if (exe == null)
                throw new NullReferenceException("gifFile");

            using (Stream s = assembly.GetManifestResourceStream(exe))
            {
                byte[] data = new byte[s.Length];
                s.Read(data, 0, data.Length);
                File.WriteAllBytes(gifFile, data);
            }
            executableTempGif = gifFile;
            return gifFile;
        }

        private static void CompressFileGIF(string input, string output)
        {
            var name = Path.GetFileName(input);
            System.Console.WriteLine("Compressing file: " + name);

            var outProcessFile = PrepareAndGetOutputPath(output, name);

            var exe = GetGifProcess();

            var psInfo = new ProcessStartInfo(exe, string.Format(@"-O3 --output ""{0}"" ""{1}""", outProcessFile, input));
            psInfo.RedirectStandardOutput = true;
            psInfo.RedirectStandardError = true;
            psInfo.UseShellExecute = false;
            psInfo.CreateNoWindow = true;

            using (var pRes = new Process())
            {
                pRes.StartInfo = psInfo;

                pRes.Start();
                if (!pRes.WaitForExit(TimeoutCompressGif))
                {
                    OnErrorOccurs(new Exception("External error | TIMEOUT | in gifsicle.exe"), "On CompressFileGIF " + ": TIMEOUT");
                    System.Console.WriteLine("ERROR | TIMEOUT | > " + name + " > (Timeout)");
                    return;
                }

                if (pRes.ExitCode == 0)
                    return;

                try
                {
                    var err = pRes.StandardError.ReadToEnd();
                    OnErrorOccurs(new Exception("External error in gifsicle.exe"), "On CompressFileGIF " + err.Replace(exe, ""));
                    System.Console.WriteLine("ERROR > " + name + " > (" + err + ")");
                }
                catch (Exception ex)
                {
                    OnErrorOccurs(new Exception("Error in log the External Error in optipng.exe"), "On CompressFileGIF² " + ex.Message.Replace(exe, ""));
                    System.Console.WriteLine("Error in ERROR > " + name + " > (" + ex.Message + ")");
                }
            }
        }

        private static string executableTempJpg;
        private static string GetJpgProcess()
        {
            if (File.Exists(executableTempJpg))
                return executableTempJpg;

            var jpgFile = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".exe";

            var assembly = typeof(Program).Assembly;
            var exe = assembly.GetManifestResourceNames()
                           .FirstOrDefault(a => a.IndexOf("jpegtran.exe", StringComparison.OrdinalIgnoreCase) > -1);

            if (exe == null)
                throw new NullReferenceException("jpegtran");

            using (Stream s = assembly.GetManifestResourceStream(exe))
            {
                byte[] data = new byte[s.Length];
                s.Read(data, 0, data.Length);
                File.WriteAllBytes(jpgFile, data);
            }
            executableTempJpg = jpgFile;
            return jpgFile;
        }

        private static void CompressFileJpg(string input, string output)
        {
            var name = Path.GetFileName(input);
            System.Console.WriteLine("Compressing file: " + name);

            var outProcessFile = PrepareAndGetOutputPath(output, name);

            var exe = GetJpgProcess();

            var psInfo = new ProcessStartInfo(exe, string.Format("-outfile \"{0}\" -optimize \"{1}\"", outProcessFile, input));
            psInfo.RedirectStandardOutput = true;
            psInfo.RedirectStandardError = true;
            psInfo.UseShellExecute = false;
            psInfo.CreateNoWindow = true;

            using (var pRes = new Process())
            {
                pRes.StartInfo = psInfo;

                pRes.Start();
                pRes.WaitForExit();

                if (pRes.ExitCode == 0)
                    return;

                try
                {
                    var err = pRes.StandardError.ReadToEnd();
                    OnErrorOccurs(new Exception("External error in jpegtran.exe"), "On CompressFileJPG : " + name + err);
                    System.Console.WriteLine("ERROR > " + name + " > (" + err + ")");
                }
                catch (Exception ex)
                {
                    OnErrorOccurs(new Exception("Error in log the External Error in jpegtran.exe"), "On CompressFileJPG² : " + name + ex.Message);
                    System.Console.WriteLine("Error in ERROR > " + name + " > (" + ex.Message + ")");
                }
            }
        }

    }


}
