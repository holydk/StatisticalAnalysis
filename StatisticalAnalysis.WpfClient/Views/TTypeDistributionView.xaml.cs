using StatisticalAnalysis.WpfClient.Models;
using StatisticalAnalysis.WpfClient.ViewModels;
using StatisticalAnalysis.WpfClient.Helpers;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using StatisticalAnalysis.WpfClient.Commands;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using iTextSharp.text;
using StatisticalAnalysis.WpfClient.HypothesisTesting.Models;
using LiveCharts.Wpf;
using System.Linq;
using MaterialDesignThemes.Wpf;
using System.Windows.Documents;

namespace StatisticalAnalysis.WpfClient.Views
{
    /// <summary>
    /// Логика взаимодействия для TTypeDistributionView.xaml
    /// </summary>
    public partial class TTypeDistributionView : System.Windows.Controls.UserControl, IView<TTypeDistributionViewModel>
    {
        public TTypeDistributionViewModel ViewModel
        {
            get => DataContext as TTypeDistributionViewModel;
            set => DataContext = value;
        }
    
        public TTypeDistributionView(TTypeDistributionViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();

            if (ViewModel.ResultCommandItems != null && !ViewModel.ResultCommandItems.Any(i => i.Title != "Сохранить"))
            {
                ViewModel.ResultCommandItems.Add(new CommandItem(
                    "Сохранить",
                    PackIconKind.ContentSave, 
                    new RelayCommand((senser) =>
                    {
                        using (var saveFileDialog = new SaveFileDialog()
                        {
                            Filter = "Все файлы (*.*)|*.*|PDF|*.pdf|Microsoft Word (*.docx)|*.docx|Microsoft Excel (*.xlsx)|*.xlsx",
                            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                            FilterIndex = 1
                        })
                        {
                            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                            {
                                try
                                {
                                    SaveResult(saveFileDialog.FileName);
                                }
                                catch (InvalidOperationException invOpEx)
                                {
                                    // Внутреняя ошибка
                                    System.Windows.MessageBox.Show(invOpEx.Message);
                                }
                                catch (Exception ex)
                                {
                                    System.Windows.MessageBox.Show("Произошла непредвиденная ошибка.\n" + ex.Message);
                                }
                            }
                        }
                    })));
            }
            
            ViewModel.IsBusy = false;
            ViewModel.IsResult = false;
            ViewModel.VariationData = null;
            ViewModel.SelectedSignificanceLevel = null;
            ViewModel.SelectedDistributionType = null;
            ViewModel.SelectedDistributionSeriesInputType = null;

            PropertyChangedEventHandler eventHandler = null;
            eventHandler = async (obj, e) =>
            {
                if (e.PropertyName == "IsBusy" && ViewModel.IsBusy)
                {
                    if (!ViewModel.IsResult)
                    {
                        BeginAnimation(PART_inputParams, new Thickness(0, -PART_inputParams.ActualHeight, 0, 0), 0, null,
                            () => PART_inputParams.Visibility = Visibility.Collapsed);
                        BeginAnimation(PART_inputData, new Thickness(-PART_inputData.ActualWidth, 0, 0, 0), 0, 300); 
                    }
                    else
                    {
                        if (PART_resultContentControl.Content is Grid grid && ViewModel.IsResult && grid.Children.Count > 0)
                        {
                            if (grid.Children[1] is ContentControl contentControl &&
                                contentControl.Content is ScrollViewer scrollViewer)
                            {
                                scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                            }
                        }
                    }
                }
                else if (e.PropertyName == "IsResult")
                {
                    if (!ViewModel.IsResult)
                    {
                        PART_inputParams.Visibility = Visibility.Visible;
                        BeginAnimation(PART_inputParams, new Thickness(0), 1);

                        if (ViewModel.VariationData != null)
                        {
                            BeginAnimation(PART_inputData, new Thickness(0), 1, 300);
                        } 
                    }
                    else
                    {
                        if (PART_resultContentControl.Content is Grid grid && !ViewModel.IsBusy && grid.Children.Count > 0)
                        {                               
                            if (grid.Children[1] is ContentControl contentControl &&
                                contentControl.Content is ScrollViewer scrollViewer)
                            {
                                await Task.Delay(400);
                                scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                            }
                        }
                    }
                }
            };

            ViewModel.PropertyChanged -= eventHandler;
            ViewModel.PropertyChanged += eventHandler;           
        }

        private static void BeginAnimation(FrameworkElement element, Thickness toThickness, double toOpacity, int? beginTime = null, Action onComplited = null)
        {
            var thicknessAnimation = new ThicknessAnimation(toThickness, new TimeSpan(0, 0, 0, 0, 400))
            {
                EasingFunction = new SineEase()
                {
                    EasingMode = EasingMode.EaseInOut
                }
            };
            thicknessAnimation.Completed += (sender, args) =>
            {
                thicknessAnimation.BeginAnimation(MarginProperty, null);
                onComplited?.Invoke();
            };

            var opacityAnimation = new DoubleAnimation(toOpacity, new TimeSpan(0, 0, 0, 0, 400))
            {
                EasingFunction = new SineEase()
                {
                    EasingMode = EasingMode.EaseInOut
                }
            };
            opacityAnimation.Completed += (sender, args) =>
            {
                opacityAnimation.BeginAnimation(OpacityProperty, null);
            };

            Storyboard.SetTarget(thicknessAnimation, element);
            Storyboard.SetTarget(opacityAnimation, element);

            Storyboard.SetTargetProperty(thicknessAnimation, new PropertyPath(MarginProperty));
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(OpacityProperty));

            var storyboard = new Storyboard();
            storyboard.Children.Add(thicknessAnimation);
            storyboard.Children.Add(opacityAnimation);

            if (beginTime.HasValue)
                storyboard.BeginTime = new TimeSpan(0, 0, 0, 0, beginTime.Value);

            storyboard.Begin();
        }

        private void SaveResult(string fileName)
        {
            var fileExt = CommonHelpers.GetFileExtension(fileName);

            if (!fileExt.HasValue) return;

            switch (fileExt.Value)
            {
                case FileExtension.Docx:

                    var wApp = new Word.Application();

                    if (wApp == null)
                        throw new InvalidOperationException(
                            "Ошибка сохранения в формат *.docx. Возможно у вас не установлен Microsoft Word.");

                    Word.Document wDoc = null;

                    try
                    {
                        wDoc = wApp.Documents.Add();






                        wDoc.Close(true);
                    }
                    finally
                    {
                        Marshal.ReleaseComObject(wDoc);
                        Marshal.ReleaseComObject(wApp);

                        wDoc = null;
                        wApp = null;

                        GC.Collect();
                    }

                    break;

                case FileExtension.Xlsx:

                    var xlApp = new Excel.Application();

                    if (xlApp == null)
                        throw new InvalidOperationException(
                            "Ошибка сохранения в формат *.xlsx. Возможно у вас не установлен Microsoft Excel.");

                    Excel.Workbook xlWorkBook = null;
                    Excel.Worksheet xlWorkSheet = null;
                    Excel.Range xlCurRange = null;

                    try
                    {
                        xlWorkBook = xlApp.Workbooks.Add();
                        xlWorkSheet = (Excel.Worksheet)xlWorkBook.Sheets[1];

                        var titleResult = CommonHelpers.FindChild<TextBlock>(PART_resultContentControl, "TitleResult");
                        var captionResult = CommonHelpers.FindChild<TextBlock>(PART_resultContentControl, "CaptionResult");
                        var curRowsCount = 1;

                        xlCurRange = xlWorkSheet.Cells[curRowsCount, 1];
                        xlCurRange.Font.Size = 16;
                        xlCurRange.Value2 = titleResult?.Text ?? "";

                        if (captionResult != null)
                        {
                            xlCurRange = xlWorkSheet.Cells[++curRowsCount, 1];
                            xlCurRange.Font.Size = 14;

                            foreach (var item in captionResult.Inlines)
                            {
                                xlCurRange.Value2 += (item as Run).Text;
                            }
                        }

                        curRowsCount++;

                        var titleVarSeries = CommonHelpers.FindChild<TextBlock>(PART_resultContentControl, "TitleVarSeries");
                        var captionVarSeries = CommonHelpers.FindChild<TextBlock>(PART_resultContentControl, "CaptionVarSeries");

                        if (titleVarSeries != null)
                        {
                            xlCurRange = xlWorkSheet.Cells[++curRowsCount, 1];
                            xlCurRange.Font.Size = 16;
                            xlCurRange.Value2 = titleVarSeries?.Text ?? "";
                        }

                        if (captionVarSeries != null)
                        {
                            xlCurRange = xlWorkSheet.Cells[++curRowsCount, 1];
                            xlCurRange.Font.Size = 14;
                            xlCurRange.Value2 = captionVarSeries?.Text ?? "";
                        }

                        xlWorkSheet.Cells[++curRowsCount, 1] = "№";
                        xlWorkSheet.Cells[curRowsCount, 2] = "Интервал";
                        xlWorkSheet.Cells[curRowsCount, 3] = "Эмп. частоты\nni";
                        xlWorkSheet.Cells[curRowsCount, 4] = "Вероятности\npi";
                        xlWorkSheet.Cells[curRowsCount, 5] = "Теор. частоты\nn*pi";
                        xlWorkSheet.Cells[curRowsCount, 6] = "(ni - n*pi)^2";
                        xlWorkSheet.Cells[curRowsCount, 7] = "(ni - n*pi)^2/(n*pi)";

                        xlCurRange = xlWorkSheet.Cells[curRowsCount, 7];
                        xlCurRange.WrapText = true;

                        var properties = typeof(THypothesisResult).GetProperties();
                        var results = ViewModel.THypothesis.Results;

                        for (int i = 0; i < results.Count; i++, curRowsCount++)
                        {
                            for (int j = 0; j < properties.Length; j++)
                            {
                                xlWorkSheet.Cells[curRowsCount + 1, j + 1] = properties[j].GetValue(results[i]);
                            }
                        }

                        curRowsCount++;

                        var critEmpResultTb = CommonHelpers.FindChild<TextBlock>(PART_resultContentControl, "CritEmpResultTb");
                        var critEmpConclusionTb = CommonHelpers.FindChild<TextBlock>(PART_resultContentControl, "CritEmpConclusionTb");

                        if (critEmpResultTb != null)
                        {
                            xlCurRange = xlWorkSheet.Cells[++curRowsCount, 1];
                            xlCurRange.Font.Size = 16;

                            foreach (var item in critEmpResultTb.Inlines)
                            {
                                xlCurRange.Value2 += (item as Run).Text;
                            }
                        }

                        if (critEmpConclusionTb != null)
                        {
                            xlCurRange = xlWorkSheet.Cells[++curRowsCount, 1];
                            xlCurRange.Font.Size = 14;

                            foreach (var item in critEmpConclusionTb.Inlines)
                            {
                                xlCurRange.Value2 += (item as Run).Text;
                            }
                        }

                        curRowsCount++;

                        var titleChartTb = CommonHelpers.FindChild<TextBlock>(PART_resultContentControl, "TitleChartTb");
                        var captionChartTb = CommonHelpers.FindChild<TextBlock>(PART_resultContentControl, "CaptionChartTb");

                        if (titleChartTb != null)
                        {
                            xlCurRange = xlWorkSheet.Cells[++curRowsCount, 1];
                            xlCurRange.Font.Size = 16;
                            xlCurRange.Value2 = titleChartTb?.Text ?? "";
                        }

                        if (captionChartTb != null)
                        {
                            xlCurRange = xlWorkSheet.Cells[++curRowsCount, 1];
                            xlCurRange.Font.Size = 14;
                            xlCurRange.Value2 = captionChartTb?.Text ?? "";
                        }

                        var chart = CommonHelpers.FindChild<CartesianChart>(PART_resultContentControl, "chartResult");

                        if (chart != null)
                        {
                            using (var mStream = chart.ToImageStream(new Thickness(12)))
                            {
                                var imgChart = System.Drawing.Image.FromStream(mStream);
                                xlCurRange = (Excel.Range)xlWorkSheet.Cells[++curRowsCount, 1];                                

                                System.Windows.Forms.Clipboard.SetDataObject(imgChart);
                                xlWorkSheet.Paste(xlCurRange, imgChart);
                            }
                        }

                        xlWorkBook.SaveAs(fileName);
                        xlWorkBook.Close(true);
                    }
                    finally
                    {
                        xlApp.Quit();

                        Marshal.ReleaseComObject(xlCurRange);
                        Marshal.ReleaseComObject(xlWorkSheet);
                        Marshal.ReleaseComObject(xlWorkBook);
                        Marshal.ReleaseComObject(xlApp);

                        xlCurRange = null;
                        xlWorkSheet = null;
                        xlWorkBook = null;
                        xlApp = null;

                        GC.Collect();
                    }

                    break;

                case FileExtension.Pdf:

                    PdfWriter writer = null;
                    var doc = new Document();

                    try
                    {
                        writer = PdfWriter.GetInstance(doc, new FileStream(fileName, FileMode.Create));

                        doc.Open();

                        var baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                        var fontHeader = new Font(baseFont, 14, Font.BOLD);
                        var title = new PdfPCell()
                        {
                            Phrase = new Phrase("Проверка гипотезы"),
                            Border = 0,
                            PaddingLeft = 5,
                            PaddingRight = 5,
                            PaddingTop = 10,
                            PaddingBottom = 10
                        };

                        doc.Add(new Phrase("Проверка гипотезы"));

                        var contentResult = CommonHelpers.FindChild<StackPanel>(PART_resultContentControl, "PART_stackPanelResult");

                        if (contentResult != null)
                        {
                            var percentScale = 100;
                            var imgData = contentResult.ToImageBytes();
                            var imgResult = iTextSharp.text.Image.GetInstance(imgData);
                            imgResult.Alignment = Element.ALIGN_CENTER;
                            imgResult.SpacingBefore = 50;

                            while (imgResult.ScaledHeight > writer.PageSize.Height - 50 ||
                                   imgResult.ScaledWidth > writer.PageSize.Width)
                            {
                                imgResult.ScalePercent(percentScale--);
                            }

                            doc.Add(imgResult); 
                        }
                    }
                    finally
                    {
                        writer?.Flush();
                        doc.Close();
                        
                        writer = null;
                        doc = null;
                    }

                    break;

                default:
                    break;
            }
        }
    }
}
