namespace CodeManagement
{
    public class CodeExecutionService : ICodeExecutionService
    {
        public bool Compile(string codeFilePath)
        {
            var codeExecutor = CodeExecutorFactory.GetCodeExecutor(codeFilePath);

            return codeExecutor.Compile(codeFilePath);
        }

        public bool Run(string codeFilePath)
        {
            var codeExecutor = CodeExecutorFactory.GetCodeExecutor(codeFilePath);

            return codeExecutor.Run(codeFilePath);
        }
    }
}
