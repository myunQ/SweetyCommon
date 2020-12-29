using System;
using System.Collections.Generic;
using System.IO;

using PdfSharpCore;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.Content;
using PdfSharpCore.Pdf.Content.Objects;
using PdfSharpCore.Pdf.IO;
using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;
using PdfSharpCore.Fonts;


namespace Sweety.Common.PerformanceTest
{
    public class PdfSharpCoreUsage
    {
        public PdfSharpCoreUsage()
        {
            GenerateMigraDoc();

            string fontName = "Microsoft Yahei"; //"微软雅黑";
            //bool findFont = false;
            //System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();
            //using (var fonts = new System.Drawing.Text.InstalledFontCollection())
            //{
            //    foreach (var xfont in fonts.Families)
            //    {
            //        strBuilder.AppendLine($"{xfont.Name},\t{xfont.IsStyleAvailable(System.Drawing.FontStyle.Regular)}");
            //        if (findFont == false && xfont.Name == fontName)
            //        {
            //            findFont = true;
            //        }
            //    }
            //}


            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            
            
            
            


            XGraphics gfx = XGraphics.FromPdfPage(page);
            gfx.RotateAtTransform(45, new XPoint(page.Width / 2, page.Height / 2));
            gfx.DrawString("龙 源 查 重 宝", new XFont(fontName, 55, XFontStyle.Bold), XBrushes.LightGray, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);
            gfx.Dispose();

            //插入图片
            gfx = XGraphics.FromPdfPage(page);
            string imgFile = "/Users/jess/myun/Upload/logonew.png";
            XImage logo = XImage.FromFile(imgFile);
            gfx.ScaleTransform(0.3d);
            gfx.DrawImage(logo, 30, 30);
            gfx.Dispose();


            //
            gfx = XGraphics.FromPdfPage(page);
            gfx.DrawRectangle(XBrushes.AliceBlue, new XRect(30, 70, page.Width - 60, 16d));
            
            
            XFont font = new XFont(fontName, 18, XFontStyle.Bold);
            gfx.DrawString("文本复制检测报告单", font, XBrushes.Black, page.Width/2, 50, XStringFormats.BaseLineCenter);

            XSize size = gfx.MeasureString("文本复制检测报告单", font, XStringFormats.Center);
            gfx.DrawString("（简洁）", new XFont(fontName, 12, XFontStyle.Regular, XPdfFontOptions.UnicodeDefault), XBrushes.Black, page.Width / 2 + size.Width / 2, 50);


            //添加超链接
            XFont linkFont = new XFont(fontName, 12, XFontStyle.Underline);
            var linkTextSize = gfx.MeasureString("《瞭望东方》2020年10期", linkFont);
            //gfx.DrawRectangle(XBrushes.CadetBlue, new XRect(100, 300, linkTextSize.Width, linkTextSize.Height));
            gfx.DrawString("《瞭望东方》2020年10期", linkFont, XBrushes.Blue, 100, 300 + linkTextSize.Height);
            var rect = gfx.Transformer.WorldToDefaultPage(new XRect(100, 300, linkTextSize.Width, linkTextSize.Height));
            page.AddWebLink(new PdfRectangle(rect), "https://www.qikan.com/magdetails/8B13DB7E-11C6-4CDC-BAD9-531AF3D8E730/2020/10.html");



            
            
            string str = "中国素有“室无瓷不雅观，人无瓷难尊”一说，瓷器（china）对于中国人（China）而言，有着深化骨血般的情感。18世纪初，波兰国王奥古斯都二世同样怀着对瓷器的挚爱，修建了欧洲第一个瓷窑，命名“麦森”。于是，这个为国王打造生活方式的品牌应运而生，阅历了300多年风雨进程的麦森现在已然成为德国甚至全欧洲的第一瓷器品牌，那些高雅的器物被人们赋予“白色黄金”之美誉。令人不可思议的是，这个阅历了11场战役、6个不同政党的品牌，现在仍然秉承着最初的理念，为人们打造精致、高雅的生活方式。";
            
            gfx.MeasureString(str, font);
            gfx.DrawString(str
                , new XFont(fontName, 12), XBrushes.Black, new XRect(20, page.Height - 20, page.Width - 40, 0));
            gfx.Dispose();


            page = document.AddPage();
            gfx = XGraphics.FromPdfPage(page);

            gfx.DrawLines(new XPen(XColor.FromArgb(0xFF000000)),
                new XPoint[]
                {
                    new XPoint(20, 20),
                    new XPoint(60, 40),
                    new XPoint(160, 60),
                    new XPoint(30, 70)
                });


            XPen pen = new XPen(XColor.FromArgb(0xFF000000), 5d);
            gfx.DrawLine(pen, new XPoint(60, page.Height - 30), new XPoint(60, page.Height + 200));
            //画圆圈
            //startAngle：开始的角度，sweepAngle：重点角度。
            //gfx.DrawArc(pen, 100, 300, 200, 200, 0, 350);
            //圆角矩形
            //gfx.DrawRoundedRectangle(new XPen(XColor.FromArgb(0xff333333)), 100, 300, 200, 200,10,10);
            //饼图
            gfx.DrawPie(XBrushes.Aqua, 100, 300, 200, 200, 0, 300);
            gfx.DrawPie(XBrushes.Aquamarine, 100, 300, 200, 200, 300, 20);
            gfx.DrawPie(XBrushes.BlueViolet, 100, 300, 200, 200, 320, 40);

            

            gfx.Dispose();
            
            //gfx.DrawImage()


            //Hebing();

            if (document.FileSize > 0)
            {
                
            }

            /* 乱码
            document.Info.Title = "这个嘛....就是标题啦";
            document.Info.Creator = document.Info.Author = "龙源查重宝";
            document.Info.Subject = "子标题在哪里？";
            */
            document.Save("/Users/jess/myun/Upload/sss.pdf");

            
            /*
            string filename = AppDomain.CurrentDomain.BaseDirectory + "temp.pdf";
            filename = "/Users/jess/myun/aa.pdf";
            PdfDocument document = PdfReader.Open(filename, PdfDocumentOpenMode.Import);

            for(int i = 1; i <= document.PageCount; i++)
            {
                PdfDocument outDocument = new PdfDocument()
                {
                    Version = document.Version
                };
                outDocument.Info.Title = $"Page {i} of {document.Info.Title}";
                outDocument.Info.Author = document.Info.Author;
                outDocument.AddPage(document.Pages[i-1]);
                using FileStream file = new FileStream($"/Users/jess/myun/pdfs/aa-{i}.pdf", FileMode.CreateNew, FileAccess.Write);
                outDocument.Save(file);
            }


            var page = document.Pages[1];

            CObject content = ContentReader.ReadContent(page);

            var extractedText = ExtractText(content);


            var extractedText2 = page.ExtractText();

            if (extractedText == extractedText2)
            {

            }
            */
        }

        private void GenerateMigraDoc()
        {
            string fontName = "Microsoft Yahei";
            string str = "中国素有“室无瓷不雅观，人无瓷难尊”一说，瓷器（china）对于中国人（China）而言，有着深化骨血般的情感。18世纪初，波兰国王奥古斯都二世同样怀着对瓷器的挚爱，修建了欧洲第一个瓷窑，命名“麦森”。于是，这个为国王打造生活方式的品牌应运而生，阅历了300多年风雨进程的麦森现在已然成为德国甚至全欧洲的第一瓷器品牌，那些高雅的器物被人们赋予“白色黄金”之美誉。令人不可思议的是，这个阅历了11场战役、6个不同政党的品牌，现在仍然秉承着最初的理念，为人们打造精致、高雅的生活方式。";

            MigraDocCore.DocumentObjectModel.Document document = new MigraDocCore.DocumentObjectModel.Document();
            document.Styles.Normal.Font.Name = fontName;
            document.Styles.Normal.ParagraphFormat.PageBreakBefore = true;
            document.UseCmykColor = true;

            var section = document.AddSection();
            MigraDocCore.DocumentObjectModel.Paragraph paragraph = section.AddParagraph();
            paragraph.AddFormattedText(str);
            paragraph.Format.Font.Color = MigraDocCore.DocumentObjectModel.Color.FromCmyk(100, 30, 20, 50);


            MigraDocCore.DocumentObjectModel.Shapes.TextFrame textFrame = section.AddTextFrame();
            //textFrame.Height = "3.0cm";
            textFrame.Width = "7.0cm";
            textFrame.Top = "5.0cm";
            textFrame.Left = MigraDocCore.DocumentObjectModel.Shapes.ShapePosition.Left;
            textFrame.RelativeHorizontal = MigraDocCore.DocumentObjectModel.Shapes.RelativeHorizontal.Margin;
            textFrame.RelativeVertical = MigraDocCore.DocumentObjectModel.Shapes.RelativeVertical.Page;
            //textFrame.FillFormat.Color = MigraDocCore.DocumentObjectModel.Color.FromCmyk(200, 200, 200, 200);
            textFrame.LineFormat.DashStyle = MigraDocCore.DocumentObjectModel.Shapes.DashStyle.Solid;
            paragraph = textFrame.AddParagraph();
            
            paragraph.Format.FirstLineIndent = new MigraDocCore.DocumentObjectModel.Unit(2, MigraDocCore.DocumentObjectModel.UnitType.Millimeter);
            //paragraph.Format.KeepTogether = true;
            //paragraph.Format.KeepWithNext = true;
            //paragraph.Format.WidowControl = true;
            paragraph.Format.LineSpacingRule = MigraDocCore.DocumentObjectModel.LineSpacingRule.Multiple;
            paragraph.Format.LineSpacing = new MigraDocCore.DocumentObjectModel.Unit(1.5, MigraDocCore.DocumentObjectModel.UnitType.Pica);
            paragraph.Format.PageBreakBefore = true;
            //paragraph.Format.Font.Size = 7;
            paragraph.Format.SpaceAfter = 3;
            paragraph.AddFormattedText(str, MigraDocCore.DocumentObjectModel.TextFormat.Underline);


            var table = section.AddTable();
            var column = table.AddColumn("7cm");
            column.Format.Alignment = MigraDocCore.DocumentObjectModel.ParagraphAlignment.Justify;

            var row = table.AddRow();
            row.Cells[0].AddParagraph(str);
            row.Cells[0].Format.Font.Bold = false;
            table.SetEdge(0, 0, 1, 1, MigraDocCore.DocumentObjectModel.Tables.Edge.Box, MigraDocCore.DocumentObjectModel.BorderStyle.Single, 1);

            MigraDocCore.Rendering.PdfDocumentRenderer pdfRenderer = new MigraDocCore.Rendering.PdfDocumentRenderer(true);
            pdfRenderer.Document = document;
            pdfRenderer.RenderDocument();
            string filename = "/Users/jess/myun/Upload/fromMigraDocCore.pdf";
            pdfRenderer.PdfDocument.Save(filename);
        }

        private void Chaifen()
        {
            string filename = "/Users/jess/myun/aa.pdf";
            PdfDocument document = PdfReader.Open(filename, PdfDocumentOpenMode.Import);

            for (int i = 1; i <= document.PageCount; i++)
            {
                PdfDocument outDocument = new PdfDocument()
                {
                    Version = document.Version
                };
                outDocument.Info.Title = $"Page {i} of {document.Info.Title}";
                outDocument.Info.Author = document.Info.Author;
                outDocument.AddPage(document.Pages[i - 1]);
                using FileStream file = new FileStream($"/Users/jess/myun/pdfs/aa-{i}.pdf", FileMode.CreateNew, FileAccess.Write);
                outDocument.Save(file);
                outDocument.Close();
            }
            document.Close();
        }

        private void Hebing()
        {
            using FileStream fileStream = new FileStream($"/Users/jess/myun/pdfs/big.pdf", FileMode.CreateNew, FileAccess.Write);
            string path = "/Users/jess/myun/pdfs/";

            PdfDocument outDocument = new PdfDocument();
            
            outDocument.Info.Title = "合并后的文件";
            foreach (var file in Directory.GetFiles(path, "*.pdf"))
            {
                PdfDocument document = PdfReader.Open(file, PdfDocumentOpenMode.Import);
                if (outDocument.Version == 0) outDocument.Version = document.Version;

                for (int i = 0; i < document.PageCount; i++)
                {
                    outDocument.AddPage(document.Pages[i]);   
                }
                document.Close();
            }

            outDocument.Save(fileStream);
            outDocument.Close();
        }

        private void Html2PDF()
        {
            
            
        }

        private IEnumerable<string> ExtractText(CObject cObject)
        {
            var textList = new List<string>();
            if (cObject is COperator)
            {
                var cOperator = cObject as COperator;
                if (cOperator.OpCode.Name == OpCodeName.Tj.ToString() ||
                    cOperator.OpCode.Name == OpCodeName.TJ.ToString())
                {
                    foreach (var cOperand in cOperator.Operands)
                    {
                        textList.AddRange(ExtractText(cOperand));
                    }
                }
            }
            else if (cObject is CSequence)
            {
                var cSequence = cObject as CSequence;
                foreach (var element in cSequence)
                {
                    textList.AddRange(ExtractText(element));
                }
            }
            else if (cObject is CString)
            {
                var cString = cObject as CString;
                textList.Add(cString.Value);
            }
            return textList;
        }
    }



    public static class PdfSharpExtensions
    {
        public static IEnumerable<string> ExtractText(this PdfPage page)
        {
            var content = ContentReader.ReadContent(page);
            var text = content.ExtractText();
            return text;
        }

        public static IEnumerable<string> ExtractText(this CObject cObject)
        {
            if (cObject is COperator)
            {
                var cOperator = cObject as COperator;
                if (cOperator.OpCode.Name == OpCodeName.Tj.ToString() ||
                    cOperator.OpCode.Name == OpCodeName.TJ.ToString())
                {
                    foreach (var cOperand in cOperator.Operands)
                        foreach (var txt in ExtractText(cOperand))
                            yield return txt;
                }
            }
            else if (cObject is CSequence)
            {
                var cSequence = cObject as CSequence;
                foreach (var element in cSequence)
                    foreach (var txt in ExtractText(element))
                        yield return txt;
            }
            else if (cObject is CString)
            {
                var cString = cObject as CString;
                yield return cString.Value;
            }
        }
    }
}
