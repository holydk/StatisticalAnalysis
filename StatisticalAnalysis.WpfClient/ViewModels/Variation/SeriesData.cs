using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using StatisticalAnalysis.WpfClient.Helpers;
using StatisticalAnalysis.WpfClient.HypothesisTesting.Models;
using StatisticalAnalysis.WpfClient.Models;

namespace StatisticalAnalysis.WpfClient.ViewModels.Variation
{
    public abstract class SeriesData<TDatum> : ViewModelBase, ISeriesData
        where TDatum : class, new()
    {
        public ObservableCollection<TDatum> Data
        {
            get => Get(() => Data);
            set => Set(() => Data, value);
        }

        public SeriesData()
        {
            Data = new ObservableCollection<TDatum>();
        }

        public virtual void FromFile(string fileName)
        {
            throw new NotImplementedException();
        }

        public async Task FromFileAsync(string fileName)
        {
            var fileExt = FileHelper.GetFileExtension(fileName);

            if (!fileExt.HasValue || fileExt != FileExtension.Csv)
                throw new NotSupportedException("Данный тип файла не поддерживается.");

            var data = await FileHelper.ReadCsvAsync(fileName, ';');

            FixDefaultErrors(data);

            var parsedData = ParseData(data);

            FixParsedData(parsedData);
            ClearData();

            foreach (var item in parsedData)
            {
                Data.Add(item);
            }
        }

        protected virtual ICollection<TDatum> ParseData(IEnumerable<string> data)
        {
            var parsedData = new List<TDatum>();
            TDatum datum = null;

            foreach (var item in data)
            {
                if (string.IsNullOrWhiteSpace(item))
                    datum = new TDatum();
                else                
                    datum = Parse(item);

                parsedData.Add(datum);
            }

            return parsedData;         
        }

        protected virtual void FixParsedData(ICollection<TDatum> data) { }

        private void FixDefaultErrors(string[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                // Eng
                if (data[i].Contains('o'))
                    data[i] = data[i].Replace('o', '0');

                // Rus
                if (data[i].Contains('о'))
                    data[i] = data[i].Replace('о', '0');
            }
        }

        public void ClearData() => Data?.Clear();

        protected abstract TDatum Parse(string item);

        public abstract ICollection<IVariationPair<object>> ToVariationPairs();
    }
}
