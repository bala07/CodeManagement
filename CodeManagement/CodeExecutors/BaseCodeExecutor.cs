namespace CodeManagement.CodeExecutors
{
    public class BaseCodeExecutor : ICodeExecutor
    {
        public virtual bool Compile(string codeFilePath)
        {
            return false;
        }

        public virtual bool Run(string codeFilePath, string[] commandLineParameters)
        {
            return false;
        }

        protected void WriteErrorToFile(string codeFilePath, string error)
        {
            var errorFilePath = Utilities.GetErrorFilePath(codeFilePath);

            Utilities.WriteToFile(errorFilePath, error);
        }
    }
}