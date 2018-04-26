using StatisticalAnalysis.WpfClient.Models;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticalAnalysis.WpfClient.Helpers
{
    public static class FileHelper
    {
        public static FileExtension? GetFileExtension(string fileName) =>
            new FileInfo(fileName).Extension.TrimStart('.').GetFileExtensionByString();

        public static FileExtension? GetFileExtensionByString(this string fileExtension) =>
            (FileExtension?)typeof(FileExtension)
                .GetField(fileExtension, 
                          System.Reflection.BindingFlags.Public |
                          System.Reflection.BindingFlags.Static |
                          System.Reflection.BindingFlags.GetField | 
                          System.Reflection.BindingFlags.IgnoreCase)
                ?.GetValue(null);

        public async static Task<string> GetStringFromFileAsync(string fileName)
        {
            byte[] data = null;

            using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                data = new byte[fileStream.Length];

                await fileStream.ReadAsync(data, 0, data.Length);               
            }

            return Encoding.UTF8.GetString(data);
        }

        public async static Task<string[]> ReadCsvAsync(string fileName, char seperator)
        {
            var data = await GetStringFromFileAsync(fileName);
            var stringBuilder = new StringBuilder(data);

            return stringBuilder
                .Replace("\r", "")
                .Replace("\n", seperator.ToString())
                .ToString()
                .TrimEnd(seperator)
                .Split(seperator);
        }
    }
}
