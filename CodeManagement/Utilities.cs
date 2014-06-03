using System.IO;

namespace CodeManagement
{
    public class Utilities
    {
        public static string GetErrorFilePath(string codeFilePath)
        {
            var codeFileDirectory = Path.GetDirectoryName(codeFilePath);
            var codeFileName = Path.GetFileNameWithoutExtension(codeFilePath);

            return codeFileDirectory + "\\" + codeFileName + "_error.txt";
        }

        public static void WriteToFile(string filePath, string contents)
        {
            using (var fileWriter = new StreamWriter(filePath))
            {
                fileWriter.WriteLine(contents);
            }
        }

        public static string ReadFromFile(string filePath)
        {
            using (var fileReader = new StreamReader(filePath))
            {
                return fileReader.ReadToEnd();
            }
        }

        public static string GetFormattedCommandLineParameters(string[] parameters)
        {
            string arguments = "";

            foreach (var parameter in parameters)
            {
                arguments += parameter + " ";
            }

            return arguments.Trim();
        }
    }
}