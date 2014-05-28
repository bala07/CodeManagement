using System.IO;

using CodeManagement.CodeExecutors;

namespace CodeManagement
{
    public class CodeExecutorFactory
    {
        public static ICodeExecutor GetCodeExecutor(string codeFilePath)
        {
            var fileExtension = Path.GetExtension(codeFilePath);

            switch (fileExtension)
            {
                case ".cs":
                case ".exe":
                    return new CSharpCodeExecutor();
                case ".java":
                case ".class":
                    return new JavaCodeExecutor();
                default:
                    return null;
            }
        }
    }
}