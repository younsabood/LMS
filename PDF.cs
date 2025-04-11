using System.Drawing;
using PdfiumViewer;
using System;
using System.IO;
using Spire.Pdf;
using Spire.Doc;             
using Spire.Xls;            
using Spire.Presentation;   

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
