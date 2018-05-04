using System.Threading.Tasks;

namespace StatisticalAnalysis.WpfClient.Models
{
    public interface IConvertFromFile
    {
        void FromFile(string fileName);

        Task FromFileAsync(string fileName);
    }
}
