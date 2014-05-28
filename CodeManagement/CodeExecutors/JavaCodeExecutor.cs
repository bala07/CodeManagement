using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace CodeManagement.CodeExecutors
{
    public class JavaCodeExecutor : ICodeExecutor
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int SetErrorMode(int wMode);

        public bool Compile(string codeFilePath)
        {
            var process = new Process { StartInfo = this.GetProcessStartInfoForCompilation(codeFilePath) };

            try
            {
                process.Start();
                process.WaitForExit();
            }
            catch (Exception e)
            {
                this.WriteSystemExceptionToFile(e.Message);
                return false;
            }

            if (process.ExitCode != 0)
            {
                this.WriteCompilationErrorsToFile(codeFilePath, process.StandardError.ReadToEnd());
                return false;
            }

            return true;
        }

        public bool Run(string classFilePath)
        {
            var process = new Process { StartInfo = this.GetProcessStartInfoForExecution(classFilePath) };

            try
            {
                process.Start();
                process.WaitForExit();
            }
            catch(Exception e)
            {
                this.WriteSystemExceptionToFile(e.Message);

                return false;
            }

            if (process.ExitCode != 0)
            {
                this.WriteCodeExceptionToFile(classFilePath, process.StandardError.ReadToEnd());

                return false;
            }

            return true;
        }

        private void WriteCodeExceptionToFile(string codeFilePath, string readToEnd)
        {
            // TODO : Write the exception to file
        }

        private void WriteCompilationErrorsToFile(string codeFilePath, string compilationErrors)
        {
            //TODO: Write the compilation error details to file
        }

        private void WriteSystemExceptionToFile(string message)
        {
            //TODO: Write the exception to file
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