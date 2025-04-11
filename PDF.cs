using System.Drawing;
using PdfiumViewer;
using System;
using System.IO;
using Spire.Pdf;
using Spire.Doc;             
using Spire.Xls;            
using Spire.Presentation;
using System.ComponentModel;

namespace LMS
{
    public static class PDF
    {
        private static readonly string TempFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TempPDF");

        static PDF()
        {
            try
            {
                Directory.CreateDirectory(TempFolder);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create temporary directory", ex);
            }
        }

        public static Image FirstPage(string filePath)
        {
            try
            {
                using (var pdfDoc = PdfiumViewer.PdfDocument.Load(filePath))
                {
                    if (pdfDoc.PageCount > 0)
                    {
                        return pdfDoc.Render(0, 300, 300, PdfRenderFlags.None);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to render PDF preview", ex);
            }
            return null;
        }

        public static string ConvertWordToPdf(string inputPath)
        {
            string tempFilePath = GetTempFilePath(inputPath);

            try
            {
                Document document = new Document();
                document.LoadFromFile(inputPath);
                document.SaveToFile(tempFilePath, Spire.Doc.FileFormat.PDF);
            }
            catch (Exception ex)
            {
                if (File.Exists(tempFilePath))
                    File.Delete(tempFilePath);
                throw new Exception("Word to PDF conversion failed", ex);
            }

            return tempFilePath;
        }
        public static string ConvertPowerPointToPdf(string inputPath)
        {
            string tempFilePath = GetTempFilePath(inputPath);

            try
            {
                Presentation presentation = new Presentation();
                presentation.LoadFromFile(inputPath);
                presentation.SaveToFile(tempFilePath, Spire.Presentation.FileFormat.PDF);
            }
            catch (Exception ex)
            {
                if (File.Exists(tempFilePath))
                    File.Delete(tempFilePath);
                throw new Exception("PowerPoint to PDF conversion failed", ex);
            }

            return tempFilePath;
        }
        public static void ConvertWordToPdfAsync(string inputPath, Action<string> onSuccess, Action<Exception> onError)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += (s, e) =>
            {
                try
                {
                    // Call the existing static method
                    e.Result = ConvertWordToPdf(inputPath);
                }
                catch (Exception ex)
                {
                    e.Result = ex;
                }
            };
            worker.RunWorkerCompleted += (s, e) =>
            {
                if (e.Result is Exception ex)
                    onError?.Invoke(ex);
                else
                    onSuccess?.Invoke(e.Result as string);
            };
            worker.RunWorkerAsync();
        }

        // Async PowerPoint Conversion
        public static void ConvertPowerPointToPdfAsync(string inputPath, Action<string> onSuccess, Action<Exception> onError)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += (s, e) =>
            {
                try
                {
                    // Call the existing static method
                    e.Result = ConvertPowerPointToPdf(inputPath);
                }
                catch (Exception ex)
                {
                    e.Result = ex;
                }
            };
            worker.RunWorkerCompleted += (s, e) =>
            {
                if (e.Result is Exception ex)
                    onError?.Invoke(ex);
                else
                    onSuccess?.Invoke(e.Result as string);
            };
            worker.RunWorkerAsync();
        }
        private static string GetTempFilePath(string inputPath)
        {
            try
            {
                string fileName = $"{Path.GetFileNameWithoutExtension(inputPath)}_{Guid.NewGuid()}.pdf";
                return Path.Combine(TempFolder, fileName);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create temporary file path", ex);
            }
        }
    }
}
