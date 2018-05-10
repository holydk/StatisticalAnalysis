using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StatisticalAnalysis.WpfClient.Helpers
{
    public static partial class CommonHelpers
    {
        public static byte[] ToImageBytes(this FrameworkElement element, Thickness? margin = null, BitmapEncoder encoder = null)
        {
            byte[] data;

            using (var mStream = ToImageStream(element, margin, encoder))
            {
                data = mStream.GetBuffer();
            }

            return data;
        }

        public static MemoryStream ToImageStream(this FrameworkElement element, Thickness? margin = null, BitmapEncoder encoder = null)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));

            if (element.ActualWidth == 0 || element.ActualHeight == 0)
                return null;

            var pSource = PresentationSource.FromVisual(element);
            var matrix = pSource.CompositionTarget.TransformToDevice;
            var dpiX = matrix.M11 * 96;
            var dpiY = matrix.M22 * 96;

            var drawingVisual = new DrawingVisual();

            // undo element transformation
            using (var drawingContext = drawingVisual.RenderOpen())
            {
                var visualBrush = new VisualBrush(element);

                drawingContext.DrawRectangle(visualBrush, null, 
                    new Rect(new Point(margin?.Left ?? 0, margin?.Top ?? 0), 
                             new Size(element.ActualWidth, element.ActualHeight)));
            }

            var bitmap = new RenderTargetBitmap(
                (int)element.ActualWidth + (int)(margin == null ? 0 : margin.Value.Left + margin.Value.Right),
                (int)element.ActualHeight + (int)(margin == null ? 0 : margin.Value.Top + margin.Value.Bottom),
                dpiX,
                dpiY,
                PixelFormats.Default);

            bitmap.Render(drawingVisual);

            if (encoder == null)
                encoder = new PngBitmapEncoder();

            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            var mStream = new MemoryStream();            
            encoder.Save(mStream);

            return mStream;
        }
    }
}
