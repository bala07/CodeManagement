using System.ServiceModel;

namespace CodeManagement
{
    [ServiceContract]
    public interface ICodeExecutionService
    {
        [OperationContract]
        bool Compile(string codeFilePath);

        [OperationContract]
        bool Run(string codeFilePath);
    }
}
