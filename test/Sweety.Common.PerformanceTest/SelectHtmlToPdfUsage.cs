using System;
using System.IO;

using SelectPdf;

namespace Sweety.Common.PerformanceTest
{
    public class SelectHtmlToPdfUsage
    {
        public SelectHtmlToPdfUsage()
        {
            UrlToPdf();
        }

        private void UrlToPdf()
        {
            StreamReader reader = new StreamReader("/Users/jess/myun/龙源期刊网-你喜欢的所有名刊大刊数字版都在这里了!.htm");
            string html = reader.ReadToEnd();

            HtmlToPdf converter = new HtmlToPdf();

            // convert the url to pdf
            PdfDocument doc = converter.ConvertHtmlString(html);

            using FileStream fileStream = new FileStream($"/Users/jess/myun/pdfs/www.qikan.com.pdf", FileMode.CreateNew, FileAccess.Write);
            // save pdf document
            doc.Save(fileStream);

            // close pdf document
            doc.Close();
        }
    }
}
