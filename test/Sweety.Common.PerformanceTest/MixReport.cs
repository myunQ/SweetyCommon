using System;
using System.Linq;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using MigraDocCore.DocumentObjectModel;
using MigraDocCore.DocumentObjectModel.Tables;
using MigraDocCore.DocumentObjectModel.Shapes.Charts;
using MigraDocCore.DocumentObjectModel.Shapes;
using MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes;
using MigraDocCore.Rendering;
using PdfSharpCore.Utils;
using SixLabors.ImageSharp.PixelFormats;


namespace Sweety.Common.PerformanceTest
{
    public static class MixReport
    {
        static string[] fontsName =
        {
            "Microsoft Yahei", //微软雅黑
            "SimHei",          //黑体
            "SimSun",          //宋体
            "FangSong",        //仿宋
            "KaiTi"            //楷体
        };

        static MixReport()
        {
            if (ImageSource.ImageSourceImpl == null) ImageSource.ImageSourceImpl = new ImageSharpImageSource<Rgba32>();
        }


        public static void GenReport1()
        {
            //PdfDocument doc = new PdfDocument();
            PdfDocument doc = PdfSharpCore.Pdf.IO.PdfReader.Open("/Users/jess/myun/Upload/HelloMigraDoc.pdf", PdfSharpCore.Pdf.IO.PdfDocumentOpenMode.Modify);
            //PdfPage page = doc.AddPage();
            PdfPage page = doc.Pages[0];

            XGraphics gfx = XGraphics.FromPdfPage(page);

            int[] data =
            {
                34205,
                6247,
                8427
            };

            double total = data.Sum();

            XBrush[] brushes = {
                new XSolidBrush(XColor.FromArgb(0xffe4e7ed)),
                new XSolidBrush(XColor.FromArgb(0xfff36d6f)),
                new XSolidBrush(XColor.FromArgb(0xff258cf7)),
            };

            string[] text =
            {
                "无问题部分",
                "文字复制部分",
                "引用部分"
            };

            XFont font = new XFont(fontsName[0], 10);
            double startAngle = 45d;
            double sweepAngle;
            double y = 500d;
            for(int i=0;i<data.Length;i++)
            {
                double p = data[i] / total;
                sweepAngle = p * 360d;
                gfx.DrawPie(brushes[i], 100, y, 100, 100, startAngle, sweepAngle);

                gfx.DrawRectangle(brushes[i], 220, y + 20 + 17 * i, 12, 12);

                gfx.DrawString($"{text[i]} {(p * 100):f2}%", font, XBrushes.Black, 235, y+30 + 17 * i);

                startAngle += sweepAngle;
            }

            string filename = "/Users/jess/myun/Upload/MixHelloMigraDoc.pdf";

            doc.Save(filename);
        }
    }
}
