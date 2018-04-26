using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using StatisticalAnalysis.HypothesisTesting.Models;
using StatisticalAnalysis.WpfClient.Helpers;
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

            ParseData(data);
        }

        protected virtual void ParseData(string[] data)
        {
            var parsedData = new List<TDatum>();

            foreach (var item in data)
            {
                TDatum datum = null;

                if (string.IsNullOrWhiteSpace(item))
                    datum = new TDatum();
                else                
                    datum = Parse(item);

                parsedData.Add(datum);
            }

            foreach (var item in parsedData)
            {
                Data.Add(item);
            }
        }

        protected abstract TDatum Parse(string item);

        public void ClearData() => Data?.Clear();

        public abstract ICollection<IVariationPair<object>> ToVariationPairs();
    }
}
