using System;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf;
using System.Reflection;
using static iText.Forms.PdfSigFieldLock;
using System.Net.WebSockets;
using System.Text;


namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPdf = "C:/Users/avnee/Downloads/dummy_pdf_2.pdf";
            string targetString = "SIGNATURE : ";

            using (PdfReader reader = new PdfReader(inputPdf))
            {
                var doc = new PdfDocument(reader);
                int pageNo = doc.GetNumberOfPages();

                ITextExtractionStrategy strategy = new LocationTextExtractionStrategy();
                var page = doc.GetPage(pageNo);

                string text = PdfTextExtractor.GetTextFromPage(page, strategy);
                //string[] words = text.Split(new[] {  '\n' }, StringSplitOptions.RemoveEmptyEntries);
                //int targetWordIndex = Array.IndexOf(words, targetString);

                FieldInfo locationResultField = typeof(LocationTextExtractionStrategy).GetField("locationalResult", BindingFlags.NonPublic | BindingFlags.Instance);

                var locationResult = locationResultField.GetValue(strategy) as List<TextChunk>;
                var chunk = locationResult[locationResult.Count - 1];
                int gropugs = targetString.Length;
                int count = (int)Math.Ceiling((double)locationResult.Count / targetString.Length);
                var arr = targetString.ToCharArray();
                Array.Reverse(arr);
                var aaa = new string(arr);
                var tempString = string.Empty;

                int lastIndex = locationResult.Count - 1;
                StringBuilder reversedString = new StringBuilder();
                for (int i = locationResult.Count - 1; i >= 0; i--)
                {
                    reversedString.Append(locationResult[i].GetText());
                    if (reversedString.ToString().EndsWith(aaa))
                    {
                        //var aa = locationResult[i + aaa.Length-2];
                        var location = locationResult[i+ aaa.Length-1].GetLocation();

                        // Console.WriteLine(location.GetStartLocation());
                        var EndLocation = location.GetEndLocation();
                        Console.WriteLine("X:{0}, Y:{1}, Z:{2}", EndLocation.Get(0), EndLocation.Get(1), EndLocation.Get(2));
                        Console.WriteLine("CharSpaceWidth:{0}", location.GetCharSpaceWidth());
                        Console.WriteLine("DistPerpendicular:{0}", location.DistPerpendicular());
                        Console.WriteLine("DistParallelEnd:{0}", location.DistParallelEnd());
                        
                        break;
                    }
                }
                
            }
        }

    }
}