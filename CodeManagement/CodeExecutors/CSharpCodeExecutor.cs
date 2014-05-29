using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

using Microsoft.CSharp;

namespace CodeManagement.CodeExecutors
{
    class CSharpCodeExecutor : BaseCodeExecutor
    {

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int SetErrorMode(int wMode);

        public override bool Compile(string codeFilePath)
        {
            var compilerParameters = this.GetCompilerParameters(codeFilePath);

            var codeProvider = new CSharpCodeProvider();
            var compilerResults = codeProvider.CompileAssemblyFromFile(compilerParameters, new[] { codeFilePath });

            if (!compilerResults.Errors.HasErrors)
            {
                return true;
            }

            this.HandleCompilationErrors(compilerResults, codeFilePath);

            return false;
        }

        public override bool Run(string codeFilePath)
        {
            var oldMode = this.SuppressWindowsErrorDialogMode();

            var process = new Process { StartInfo = this.GetProcessStartInfo(codeFilePath) };

            try
            {
                process.Start();
                process.WaitForExit();
            }
            catch (Exception e)
            {
                throw new Exception("Process start caused an exception");
            }
            finally
            {
                this.ResetWindowsErrorDialogMode(oldMode);
            }

            if (process.ExitCode == 0)
            {
                return true;
            }

            this.WriteErrorToFile(process.StandardError.ReadToEnd(), codeFilePath);

            return false;
        }

        private CompilerParameters GetCompilerParameters(string codeFilePath)
        {
            return new CompilerParameters
            {
                GenerateInMemory = true,
                TreatWarningsAsErrors = false,
                GenerateExecutable = true,
                CompilerOptions = "/optimize",
                OutputAssembly = this.GetAssemblyPath(codeFilePath)
            };
        }

        private ProcessStartInfo GetProcessStartInfo(string codeFilePath)
        {
            var codeFileDirectoryPath = Path.GetDirectoryName(codeFilePath);

            return new ProcessStartInfo
            {
                FileName = this.GetAssemblyPath(codeFilePath),
                WorkingDirectory = codeFileDirectoryPath,
                UseShellExecute = false,
                ErrorDialog = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };
        }

        private string GetAssemblyPath(string codeFilePath)
        {
            return Path.GetDirectoryName(codeFilePath) + "\\" + Path.GetFileNameWithoutExtension(codeFilePath) + ".exe";
        }

        private void HandleCompilationErrors(CompilerResults compilerResults, string codeFilePath)
        {
            var compilationErrors = "";

            foreach (var error in compilerResults.Errors)
            {
                compilationErrors += error + "\n";
            }

            this.WriteErrorToFile(codeFilePath, compilationErrors);

        }

        private int SuppressWindowsErrorDialogMode()
        {
            return SetErrorMode(3);
        }

        private void ResetWindowsErrorDialogMode(int oldMode)
        {
            SetErrorMode(oldMode);
        }
    }
}