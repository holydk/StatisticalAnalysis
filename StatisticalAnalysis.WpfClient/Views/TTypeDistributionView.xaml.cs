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

            if (ViewModel.ResultCommandItems != null && !ViewModel.ResultCommandItems.Any(i => i.Title == "Сохранить"))
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

            var titleResult = CommonHelpers.FindChild<TextBlock>(PART_resultContentControl, "TitleResult");
            var captionResult = CommonHelpers.FindChild<TextBlock>(PART_resultContentControl, "CaptionResult");
            var titleVarSeries = CommonHelpers.FindChild<TextBlock>(PART_resultContentControl, "TitleVarSeries");
            var captionVarSeries = CommonHelpers.FindChild<TextBlock>(PART_resultContentControl, "CaptionVarSeries");
            var critEmpResultTb = CommonHelpers.FindChild<TextBlock>(PART_resultContentControl, "CritEmpResultTb");
            var critEmpConclusionTb = CommonHelpers.FindChild<TextBlock>(PART_resultContentControl, "CritEmpConclusionTb");
            var titleChartTb = CommonHelpers.FindChild<TextBlock>(PART_resultContentControl, "TitleChartTb");
            var captionChartTb = CommonHelpers.FindChild<TextBlock>(PART_resultContentControl, "CaptionChartTb");
            var chart = CommonHelpers.FindChild<CartesianChart>(PART_resultContentControl, "chartResult");

            switch (fileExt.Value)
            {
                case FileExtension.Docx:

                    var wApp = new Word.Application();

                    if (wApp == null)
                        throw new InvalidOperationException(
                            "Ошибка сохранения в формат *.docx. Возможно у вас не установлен Microsoft Word.");

                    Word.Document wDoc = null;
                    Word.Table table = null;
                    Word.Range wCurRange = null;
                    Word.Paragraph wParagraph = null;             

                    try
                    {
                        wDoc = wApp.Documents.Add();

                        if (titleResult != null)
                        {
                            wParagraph = wDoc.Content.Paragraphs.Add();
                            wParagraph.Range.Font.Size = 16;
                            wParagraph.Range.Text = titleResult.Text;
                            wParagraph.Range.InsertParagraphAfter(); 
                        }

                        if (captionResult != null)
                        {
                            wParagraph = wDoc.Content.Paragraphs.Add();
                            wParagraph.Range.Font.Size = 14;

                            var text = string.Empty;

                            foreach (var item in captionResult.Inlines)
                            {
                                text += (item as System.Windows.Documents.Run).Text;
                            }

                            wParagraph.Range.Text = text + "\n";
                            wParagraph.Range.InsertParagraphAfter();
                        }

                        if (titleVarSeries != null)
                        {
                            wParagraph = wDoc.Content.Paragraphs.Add();
                            wParagraph.Range.Font.Size = 16;
                            wParagraph.Range.Text = titleVarSeries.Text;
                            wParagraph.Range.InsertParagraphAfter();
                        }

                        if (captionVarSeries != null)
                        {
                            wParagraph = wDoc.Content.Paragraphs.Add();
                            wParagraph.Range.Font.Size = 14;
                            wParagraph.Range.Text = captionVarSeries.Text + "\n";
                            wParagraph.Range.InsertParagraphAfter();
                        }

                        //xlWorkSheet.Cells[++curRowsCount, 1] = "№";
                        //xlWorkSheet.Cells[curRowsCount, 2] = "Интервал";
                        //xlWorkSheet.Cells[curRowsCount, 3] = "Эмп. частоты\nni";
                        //xlWorkSheet.Cells[curRowsCount, 4] = "Вероятности\npi";
                        //xlWorkSheet.Cells[curRowsCount, 5] = "Теор. частоты\nn*pi";
                        //xlWorkSheet.Cells[curRowsCount, 6] = "(ni - n*pi)^2";
                        //xlWorkSheet.Cells[curRowsCount, 7] = "(ni - n*pi)^2/(n*pi)";

                        //xlCurRange = xlWorkSheet.Cells[curRowsCount, 7];
                        //xlCurRange.WrapText = true;

                        //var properties = typeof(THypothesisResult).GetProperties();
                        //var results = ViewModel.THypothesis.Results;

                        //for (int i = 0; i < results.Count; i++, curRowsCount++)
                        //{
                        //    for (int j = 0; j < properties.Length; j++)
                        //    {
                        //        xlWorkSheet.Cells[curRowsCount + 1, j + 1] = properties[j].GetValue(results[i]);
                        //    }
                        //}

                        var properties = typeof(THypothesisResult).GetProperties();
                        var results = ViewModel.THypothesis.Results;
                        table = wDoc.Tables.Add(wParagraph.Range, results.Count + 1, properties.Length);

                        table.Rows[1].Cells[1].Range.Text = "№";
                        table.Rows[1].Cells[2].Range.Text = "Интервал";
                        table.Rows[1].Cells[3].Range.Text = "Эмп. частоты\nni";
                        table.Rows[1].Cells[4].Range.Text = "Вероятности\npi";
                        table.Rows[1].Cells[5].Range.Text = "Теор. частоты\nn*pi";
                        table.Rows[1].Cells[6].Range.Text = "(ni - n*pi)^2";
                        table.Rows[1].Cells[7].Range.Text = "(ni - n*pi)^2/(n*pi)";

                        for (int i = 1; i < results.Count; i++)
                        {
                            for (int j = 0; j < properties.Length; j++)
                            {
                                table.Rows[i + 1].Cells[j + 1].Range.Text = properties[j].GetValue(results[i]).ToString();
                            }
                        }

                        if (critEmpResultTb != null)
                        {
                            wParagraph = wDoc.Content.Paragraphs.Add();
                            wParagraph.Range.Font.Size = 16;

                            var text = string.Empty;

                            foreach (var item in critEmpResultTb.Inlines)
                            {
                                text += (item as System.Windows.Documents.Run).Text;
                            }

                            wParagraph.Range.Text = text + "\n";
                            wParagraph.Range.InsertParagraphAfter();
                        }

                        if (critEmpConclusionTb != null)
                        {
                            wParagraph = wDoc.Content.Paragraphs.Add();
                            wParagraph.Range.Font.Size = 14;

                            var text = string.Empty;

                            foreach (var item in critEmpConclusionTb.Inlines)
                            {
                                text += (item as System.Windows.Documents.Run).Text;
                            }

                            wParagraph.Range.Text = text + "\n";
                            wParagraph.Range.InsertParagraphAfter();
                        }

                        if (titleChartTb != null)
                        {
                            wParagraph = wDoc.Content.Paragraphs.Add();
                            wParagraph.Range.Font.Size = 16;
                            wParagraph.Range.Text = titleChartTb.Text;
                            wParagraph.Range.InsertParagraphAfter();
                        }

                        if (captionChartTb != null)
                        {
                            wParagraph = wDoc.Content.Paragraphs.Add();
                            wParagraph.Range.Font.Size = 14;
                            wParagraph.Range.Text = captionChartTb.Text + "\n";
                            wParagraph.Range.InsertParagraphAfter();
                        }

                        if (chart != null)
                        {
                            using (var mStream = chart.ToImageStream(new Thickness(12)))
                            {
                                var imgChart = System.Drawing.Image.FromStream(mStream);

                                System.Windows.Forms.Clipboard.SetDataObject(imgChart);
                                wParagraph.Range.Paste();
                            }
                        }

                        wCurRange = wDoc.Range(0, 0);                        

                        wDoc.SaveAs2(fileName);
                        wDoc.Close();
                    }
                    finally
                    {
                        wApp.Quit();

                        Marshal.ReleaseComObject(wParagraph);
                        Marshal.ReleaseComObject(wCurRange);
                        Marshal.ReleaseComObject(table);
                        Marshal.ReleaseComObject(wDoc);
                        Marshal.ReleaseComObject(wApp);

                        wParagraph = null;
                        wCurRange = null;
                        table = null;
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

                        var curRowsCount = 1;

                        if (titleResult != null)
                        {
                            xlCurRange = xlWorkSheet.Cells[curRowsCount, 1];
                            xlCurRange.Font.Size = 16;
                            xlCurRange.Value2 = titleResult.Text;

                        }

                        if (captionResult != null)
                        {
                            xlCurRange = xlWorkSheet.Cells[++curRowsCount, 1];
                            xlCurRange.Font.Size = 14;

                            foreach (var item in captionResult.Inlines)
                            {
                                xlCurRange.Value2 += (item as System.Windows.Documents.Run).Text;
                            }
                        }

                        curRowsCount++;

                        if (titleVarSeries != null)
                        {
                            xlCurRange = xlWorkSheet.Cells[++curRowsCount, 1];
                            xlCurRange.Font.Size = 16;
                            xlCurRange.Value2 = titleVarSeries.Text;
                        }

                        if (captionVarSeries != null)
                        {
                            xlCurRange = xlWorkSheet.Cells[++curRowsCount, 1];
                            xlCurRange.Font.Size = 14;
                            xlCurRange.Value2 = captionVarSeries.Text;
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

                        if (critEmpResultTb != null)
                        {
                            xlCurRange = xlWorkSheet.Cells[++curRowsCount, 1];
                            xlCurRange.Font.Size = 16;

                            foreach (var item in critEmpResultTb.Inlines)
                            {
                                xlCurRange.Value2 += (item as System.Windows.Documents.Run).Text;
                            }
                        }

                        if (critEmpConclusionTb != null)
                        {
                            xlCurRange = xlWorkSheet.Cells[++curRowsCount, 1];
                            xlCurRange.Font.Size = 14;

                            foreach (var item in critEmpConclusionTb.Inlines)
                            {
                                xlCurRange.Value2 += (item as System.Windows.Documents.Run).Text;
                            }
                        }

                        curRowsCount++;

                        if (titleChartTb != null)
                        {
                            xlCurRange = xlWorkSheet.Cells[++curRowsCount, 1];
                            xlCurRange.Font.Size = 16;
                            xlCurRange.Value2 = titleChartTb.Text;
                        }

                        if (captionChartTb != null)
                        {
                            xlCurRange = xlWorkSheet.Cells[++curRowsCount, 1];
                            xlCurRange.Font.Size = 14;
                            xlCurRange.Value2 = captionChartTb.Text;
                        }

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
                        xlWorkBook.Close();
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
                        var fontHeader = new Font(baseFont, 16, Font.NORMAL);
                        var fontNormal = new Font(baseFont, 14, Font.NORMAL);

                        if (titleResult != null)
                        {
                            doc.Add(new Paragraph(titleResult.Text, fontHeader));
                        }

                        if (captionResult != null)
                        {
                            var data = string.Empty;

                            foreach (var item in captionResult.Inlines)
                            {
                                data += (item as System.Windows.Documents.Run).Text;
                            }

                            var captRes = new Paragraph(data, fontNormal)
                            {
                                SpacingAfter = 20
                            };

                            doc.Add(captRes);
                        }

                        if (titleVarSeries != null)
                        {
                            doc.Add(new Paragraph(titleVarSeries.Text, fontHeader));
                        }

                        if (captionVarSeries != null)
                        {
                            doc.Add(new Paragraph(captionVarSeries.Text, fontNormal));
                        }

                        var properties = typeof(THypothesisResult).GetProperties();
                        var results = ViewModel.THypothesis.Results;
                        var tableResult = new PdfPTable(properties.Length)
                        {
                            SpacingBefore = 20,
                            SpacingAfter = 20
                        };

                        tableResult.AddCell("№");
                        tableResult.AddCell("Интервал");
                        tableResult.AddCell("Эмп. частоты\nni");
                        tableResult.AddCell("Вероятности\npi");
                        tableResult.AddCell("Теор. частоты\nn*pi");
                        tableResult.AddCell("(ni - n*pi)^2");
                        tableResult.AddCell("(ni - n*pi)^2/(n*pi)");

                        for (int i = 0; i < results.Count; i++)
                        {
                            for (int j = 0; j < properties.Length; j++)
                            {
                                tableResult.AddCell(properties[j].GetValue(results[i]).ToString());
                            }
                        }

                        doc.Add(tableResult);

                        if (critEmpResultTb != null)
                        {
                            var data = string.Empty;

                            foreach (var item in critEmpResultTb.Inlines)
                            {
                                data += (item as System.Windows.Documents.Run).Text;
                            }

                            doc.Add(new Paragraph(data, fontHeader));
                        }

                        if (critEmpConclusionTb != null)
                        { 

                            var data = string.Empty;

                            foreach (var item in critEmpConclusionTb.Inlines)
                            {
                                data += (item as System.Windows.Documents.Run).Text;
                            }

                            var critEmpConcl = new Paragraph(data, fontNormal)
                            {
                                SpacingBefore = 20,
                                SpacingAfter = 20
                            };

                            doc.Add(critEmpConcl);
                        }

                        if (titleChartTb != null)
                        {
                            doc.Add(new Paragraph(titleChartTb.Text, fontHeader));
                        }

                        if (captionChartTb != null)
                        { 
                            doc.Add(new Paragraph(captionChartTb.Text, fontNormal));
                        }

                        if (chart != null)
                        {
                            var imgData = chart.ToImageBytes();
                            var imgResult = iTextSharp.text.Image.GetInstance(imgData);
                            imgResult.Alignment = Element.ALIGN_CENTER;
                            imgResult.SpacingBefore = 20;

                            imgResult.ScalePercent(70);

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
