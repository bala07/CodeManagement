namespace CodeManagement.CodeExecutors
{
    public interface ICodeExecutor
    {
        bool Compile(string codeFilePath);

        bool Run(string codeFilePath);
    }
}
