﻿using StatisticalAnalysis.WpfClient.Models;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace StatisticalAnalysis.WpfClient.Helpers
{
    public static partial class CommonHelpers
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

        public async static Task<string> ReadFileAsync(string filename, Encoding encoding)
        {
            var data = string.Empty;

            using (var reader = new StreamReader(filename, encoding))
            {
                data = await reader.ReadToEndAsync();
            }

            return data;
        }

        public async static Task<string[]> ReadCsvAsync(string fileName, char seperator)
        {
            var data = await ReadFileAsync(fileName, Encoding.Default);
            var stringBuilder = new StringBuilder(data);

            return stringBuilder
                .Replace("\r", string.Empty)
                .Replace("\n", seperator.ToString())
                .ToString()
                .TrimEnd(seperator)
                .Split(seperator);
        }
    }
}
