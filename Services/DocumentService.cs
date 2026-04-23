using System;
using System.IO;
using System.Text;
using UglyToad.PdfPig;
using NPOI.XWPF.UserModel;

namespace EduCorePro.Services;

public class DocumentService
{
    public string ExtractTextFromPdf(string filePath, int maxPages = 30)
    {
        StringBuilder text = new StringBuilder();
        try
        {
            using (PdfDocument document = PdfDocument.Open(filePath))
            {
                int pagesToRead = Math.Min(document.NumberOfPages, maxPages);
                for (int i = 1; i <= pagesToRead; i++)
                {
                    var page = document.GetPage(i);
                    text.AppendLine(page.Text);
                }
            }
            return text.ToString();
        }
        catch (Exception ex)
        {
            return $"Ошибка чтения PDF: {ex.Message}";
        }
    }

public string CreateWordDocument(string content, string fileName)
    {
        try
        {
            string docsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string outputPath = Path.Combine(docsFolder, $"{fileName}_{DateTime.Now:HH-mm-ss}.docx");

            XWPFDocument doc = new XWPFDocument();
            string[] paragraphs = content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            
            foreach (var paraText in paragraphs)
            {
                if (string.IsNullOrWhiteSpace(paraText)) continue;
                
                XWPFParagraph p = doc.CreateParagraph();
                string text = paraText.Trim();

                if (text.StartsWith("#"))
                {
                    int headerLevel = text.TakeWhile(c => c == '#').Count();
                    text = text.Substring(headerLevel).Trim().Replace("**", "");
                    
                    var r = p.CreateRun();
                    r.SetText(text);
                    r.IsBold = true;
                    r.FontSize = headerLevel == 1 ? 18 : (headerLevel == 2 ? 16 : 14);
                    continue;
                }

                if (text.StartsWith("- "))
                {
                    p.IndentationLeft = 400;
                    text = "• " + text.Substring(2);
                }

                var parts = text.Split("**");
                for (int i = 0; i < parts.Length; i++)
                {
                    if (string.IsNullOrEmpty(parts[i])) continue;
                    
                    var r = p.CreateRun();
                    r.SetText(parts[i]);
                    r.FontSize = 12;
                    
                  
                    if (i % 2 != 0) 
                    {
                        r.IsBold = true;
                    }
                }
            }

            using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
            {
                doc.Write(fs);
            }

            return outputPath;
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка создания Word: {ex.Message}");
        }
    }
}