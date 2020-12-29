using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

using Spire.Pdf;
namespace Sweety.Common.PerformanceTest
{
    public class FreeSpirePdfUsage
    {
        public FreeSpirePdfUsage()
        {
            string filename = "/Users/jess/myun/aa.pdf";
            filename = AppDomain.CurrentDomain.BaseDirectory + "temp.pdf";

            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile(filename);

            //IList<Image> images = new List<Image>();
            StringBuilder buffer = new StringBuilder();
            //遍历文档，提取文本
            foreach (PdfPageBase page in doc.Pages)
            {
                /*
                if (page.ExtractImages() != null)
                {
                    foreach (Image image in page.ExtractImages())
                    {
                        images.Add(image);
                    }
                }
                */
                buffer.Append(page.ExtractText());
            }
            doc.Close();


            //遍历提取的图片，保存并命名图片
            /*
            int index = 0;
            foreach (Image image in images)
            {
                String imageFileName = String.Format("Image------{0}.png", index++);
                image.Save(imageFileName, ImageFormat.Png);
            }
            */
        }

        private void Html2PdfByURL()
        {
            string input = @"<strong>This is a test for converting HTML string to PDF </strong>
      <ul><li>Spire.PDF supports to convert HTML in URL into PDF</li>
      <li>Spire.PDF supports to convert HTML string into PDF</li>
      <li>With the new plugin</li></ul>";
            input = "http://www.qikan.com";
            Spire.Pdf.HtmlConverter.Qt.HtmlConverter.Convert(input, "www.qikan.com.pdf",
                 //启用javascript
                 true,
                 //加载超时
                 10 * 1000,
                 //页面大小
                 new SizeF(612, 792),
                 //页边距
                 new Spire.Pdf.Graphics.PdfMargins(0),
                 //加载类型
                 Spire.Pdf.HtmlConverter.LoadHtmlType.URL);


        }

        private void Html2PdfByHTMLText()
        {
            string input = @"<strong>This is a test for converting HTML string to PDF </strong>
      <ul><li>Spire.PDF supports to convert HTML in URL into PDF</li>
      <li>Spire.PDF supports to convert HTML string into PDF</li>
      <li>With the new plugin</li></ul>";
            
            Spire.Pdf.HtmlConverter.Qt.HtmlConverter.Convert(input, "html.pdf",
                 //启用javascript
                 true,
                 //加载超时
                 10 * 1000,
                 //页面大小
                 new SizeF(612, 792),
                 //页边距
                 new Spire.Pdf.Graphics.PdfMargins(0),
                 //加载类型
                 Spire.Pdf.HtmlConverter.LoadHtmlType.SourceCode);
        }
    }
}
