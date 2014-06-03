using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace CodeManagement.CodeExecutors
{
    public class JavaCodeExecutor : BaseCodeExecutor
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int SetErrorMode(int wMode);

        public override bool Compile(string codeFilePath)
        {
            var process = new Process { StartInfo = this.GetProcessStartInfoForCompilation(codeFilePath) };

            try
            {
                process.Start();
                process.WaitForExit();
            }
            catch (Exception e)
            {
                throw new Exception("Process start caused an exception while compiling");
            }

            if (process.ExitCode != 0)
            {
                this.WriteErrorToFile(codeFilePath, process.StandardError.ReadToEnd());
                return false;
            }

            return true;
        }

        public override bool Run(string classFilePath, string[] commandLineParameters)
        {
            var process = new Process { StartInfo = this.GetProcessStartInfoForExecution(classFilePath) };

            try
            {
                process.Start();
                process.WaitForExit();
            }
            catch(Exception e)
            {
                throw new Exception("Process start caused an exception while running the code");
            }

            if (process.ExitCode != 0)
            {
                this.WriteErrorToFile(classFilePath, process.StandardError.ReadToEnd());

                return false;
            }

            return true;
        }

        private ProcessStartInfo GetProcessStartInfoForCompilation(string codeFilePath)
        {
            const string JavaHome = "C:\\Program Files\\Java\\jdk1.7.0_40\\bin\\javac.exe";

            string codeFileDirectory = Path.GetDirectoryName(codeFilePath);

            return new ProcessStartInfo
                       {
                           FileName = JavaHome,
                           RedirectStandardError = true,
                           RedirectStandardOutput = true,
                           ErrorDialog = false,
                           UseShellExecute = false,
                           Arguments = "-sourcepath " + codeFileDirectory + " -d " + codeFileDirectory + " " + codeFilePath
                        };
        }

        private ProcessStartInfo GetProcessStartInfoForExecution(string classFilePath)
        {
            const string JavaHome = "C:\\Program Files\\Java\\jdk1.7.0_40\\bin\\java.exe";

            string classFileDirectory = Path.GetDirectoryName(classFilePath);
            string classFileName = Path.GetFileNameWithoutExtension(classFilePath);

            return new ProcessStartInfo
                       {
                           FileName = JavaHome,
                           RedirectStandardError = true,
                           RedirectStandardOutput = true,
                           ErrorDialog = false,
                           UseShellExecute = false,
                           Arguments = "-cp " + classFileDirectory + " " + classFileName,
                           WorkingDirectory = classFileDirectory
                       };
        }
    }
}