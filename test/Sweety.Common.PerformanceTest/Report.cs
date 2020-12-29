using System;

using MigraDocCore.DocumentObjectModel;
using MigraDocCore.DocumentObjectModel.Tables;
using MigraDocCore.DocumentObjectModel.Shapes;
using MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes;
using MigraDocCore.Rendering;
using PdfSharpCore.Utils;
using SixLabors.ImageSharp.PixelFormats;

using Sweety.Common.Extensions;

namespace Sweety.Common.PerformanceTest
{
    public static class Report
    {
        static string[] fontsName =
        {
            "Microsoft Yahei", //微软雅黑
            "SimHei",          //黑体
            "SimSun",          //宋体
            "FangSong",        //仿宋
            "KaiTi"            //楷体
        };

        static Report()
        {
            if (ImageSource.ImageSourceImpl == null) ImageSource.ImageSourceImpl = new ImageSharpImageSource<Rgba32>();
        }


        public static void GenReport1()
        {
            string imgSelected = "/Users/jess/myun/Upload/selected.png";
            string imgUnselected = "/Users/jess/myun/Upload/unselected.png";
            string title = "基于2,3,5,6-四(1H-1,2,4-三氮唑基)对苯二甲腈配体配位聚合物的合成及性质研究";
            title += "2,3,51H-1,-￥6546546.5454565546氮唑基)对苯二甲腈配";
            Document document = CreateDocument("（简洁）"
                , DateTimeOffset.Now
                , title
                , "秦明云");

            var section = document.LastSection;
            section.AddParagraph();


            var table = section.AddTable();
            table.Rows.Height = "1cm";
            table.Rows.HeightRule = RowHeightRule.AtLeast;
            table.Rows.VerticalAlignment = VerticalAlignment.Center;
            table.Borders.Width = "1pt";
            table.Borders.Color = new Color(0xffdddddd);
            table.AddColumn("5.8cm");
            table.AddColumn("6.7cm");
            table.AddColumn("3cm");
            table.AddColumn("4cm");


            var row = table.AddRow();
            //row.HeadingFormat = true;
            row.Height = "1.3cm";
            row.Format.Font.Bold = true;
            row.Format.Font.Size = 14;
            row.Format.Font.Color = Colors.DimGray;
            row.Shading.Color = new Color(0xffeeeeee);
            row.Cells[0].MergeRight = 3;
            row.Cells[0].AddParagraph("查询结果");


            row = table.AddRow();
            row.Cells[0].AddParagraph("去除本人已发表文献复制比：")
                .Format.Alignment = ParagraphAlignment.Right;
            row.Cells[1].AddParagraph("2.9%");
            row.Cells[2].AddParagraph("跨语言检测结果：")
                .Format.Alignment = ParagraphAlignment.Right;
            row.Cells[3].AddParagraph("0%");
            
            row = table.AddRow();
            row.Cells[0].AddParagraph("去除引用文献复制比：")
                .Format.Alignment = ParagraphAlignment.Right;
            row.Cells[1].AddParagraph()
                
                .AddText("2.9%");
            row.Cells[2].AddParagraph("总文字复制比：")
                .Format.Alignment = ParagraphAlignment.Right;
            row.Cells[3].AddParagraph("0%");

            row = table.AddRow();
            row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[0].AddParagraph("单篇最大文字复制比：");
            row.Cells[1].MergeRight = 2;
            var paragraph = row.Cells[1].AddParagraph("2.9% （");
            paragraph.AddHyperlink("https://www.qikan.com/article/lwdf20201002.html", HyperlinkType.Url)
                .AddFormattedText("城市有机更新", "href");
            paragraph.AddText("）");

            row = table.AddRow();
            row.Cells[0].AddParagraph().AddFormattedText("重复字数：", "table-item1");
            row.Cells[1].AddParagraph("[54546]");
            row.Cells[2].AddParagraph().AddFormattedText("总段落数：", "table-item1");
            row.Cells[3].AddParagraph("[5]");

            row = table.AddRow();
            row.Cells[0].AddParagraph().AddFormattedText("总字数：", "table-item1");
            row.Cells[1].AddParagraph("[54546]");
            row.Cells[2].AddParagraph().AddFormattedText("疑似段落数：", "table-item1");
            row.Cells[3].AddParagraph("[5]");

            row = table.AddRow();
            row.Cells[0].AddParagraph().AddFormattedText("单篇最大重复字数：", "table-item1");
            row.Cells[1].AddParagraph("[54546]");
            row.Cells[2].AddParagraph().AddFormattedText("前部重合数：", "table-item1");
            row.Cells[3].AddParagraph("[4155]");

            row = table.AddRow();
            row.Cells[0].AddParagraph().AddFormattedText("疑似段落最大重合字数：", "table-item1");
            row.Cells[1].AddParagraph("[54546]");
            row.Cells[2].AddParagraph().AddFormattedText("后部重合数：", "table-item1");
            row.Cells[3].AddParagraph("[4155]");

            row = table.AddRow();
            row.Cells[0].AddParagraph().AddFormattedText("疑似段落最小重合字数：", "table-item1");
            row.Cells[1].AddParagraph("[54546]");
            row.Cells[1].MergeRight = 2;


            row = table.AddRow();
            row.Height = "5cm";
            //饼图位置。

            row = table.AddRow();
            row.Cells[0].MergeRight = 3;
            paragraph = row.Cells[0].AddParagraph();
            const int spaceCount = 12;
            const int imgHeight = 10;
            var selectedImg = ImageSource.FromFile(imgSelected);
            var selectImg = paragraph.AddImage(selectedImg);
            selectImg.Height = imgHeight;
            paragraph.AddSpace(1);
            paragraph.AddFormattedText("疑似剽窃观点").Color = new Color(0xff258cf7);
            paragraph.AddSpace(spaceCount);
            var unselectedImg = ImageSource.FromFile(imgUnselected);
            selectImg = paragraph.AddImage(unselectedImg);
            selectImg.Height = imgHeight;
            selectImg.RelativeVertical = RelativeVertical.Paragraph;
            paragraph.AddSpace(1);
            paragraph.AddText("疑似剽窃文字表述");
            paragraph.AddSpace(spaceCount);
            selectImg = paragraph.AddImage(unselectedImg);
            selectImg.Height = imgHeight;
            paragraph.AddSpace(1);
            paragraph.AddText("疑似自我剽窃");
            paragraph.AddSpace(spaceCount);
            selectImg = paragraph.AddImage(unselectedImg);
            selectImg.Height = imgHeight;
            paragraph.AddSpace(1);
            paragraph.AddText("疑似整体剽窃");
            paragraph.AddSpace(spaceCount);
            selectImg = paragraph.AddImage(unselectedImg);
            selectImg.Height = imgHeight;
            paragraph.AddSpace(1);
            paragraph.AddText("过度引用");

            table.Columns[0].Format.LeftIndent = "0.3cm";
            table.Columns[0].Borders.Right.Width = 0;
            table.Columns[1].Borders.Left.Width = 0;
            table.Columns[1].Borders.Right.Width = 0;
            table.Columns[2].Borders.Left.Width = 0;
            table.Columns[2].Borders.Right.Width = 0;
            table.Columns[3].Borders.Left.Width = 0;


            //被检测文章被拆分后的列表。
            table = section.AddTable();
            table.Rows.Height = "1cm";
            table.Rows.HeightRule = RowHeightRule.AtLeast;
            table.Borders.Width = 0;
            table.Borders.Color = new Color(0xffdddddd);
            table.BottomPadding = 10;
            table.AddColumn("5cm");
            table.AddColumn("14.5cm");
            row = table.AddRow();
            row.Cells[0].AddParagraph("2.94%（323）");
            
            paragraph = row.Cells[1].AddParagraph();
            paragraph.Style = "href";
            paragraph.AddHyperlink("section-1", HyperlinkType.Bookmark)
                .AddText("第一部分的标题，第一部分的标题，第一部分的标题，第一部分的标题，第一部分的标题，（总 1232353 个字）");

            row = table.AddRow();
            row.Cells[0].AddParagraph("2.94%（323）");

            paragraph = row.Cells[1].AddParagraph();
            paragraph.Style = "href";
            paragraph.AddText("第一部分的标题，第一部分的标题，第一部分的标题，第一部分的标题，第一部分的标题，（总 1232353 个字）");

            row = table.AddRow();
            row.Cells[0].AddParagraph("2.94%（323）");
            row.Cells[1].AddParagraph()
                .AddFormattedText("第一部分的标题，第一部分的标题，第一部分的标题，第一部分的标题，第一部分的标题，", "href")
                .AddFormattedText("（总 1232353 个字）");

            row = table.AddRow();
            row.Cells[0].AddParagraph("2.94%（323）");
            row.Cells[1].AddParagraph()
                .AddFormattedText("第一部分的标题，第一部分的标题，第一部分的标题，第一部分的标题，第一部分的标题，", "href")
                .AddFormattedText("（总 1232353 个字）");

            row = table.AddRow();
            row.Cells[0].AddParagraph("2.94%（323）");
            row.Cells[1].AddParagraph()
                .AddFormattedText("第一部分的标题，第一部分的标题，第一部分的标题，第一部分的标题，第一部分的标题，", "href")
                .AddFormattedText("（总 1232353 个字）");

            table.SetEdge(0, 0, 1, table.Rows.Count, Edge.Left, BorderStyle.Single, 1);
            table.SetEdge(1, 0, 1, table.Rows.Count, Edge.Right, BorderStyle.Single, 1);
            table.SetEdge(0, table.Rows.Count - 1, table.Columns.Count, 1, Edge.Bottom, BorderStyle.Single, 1);
            table.Columns[0].Format.LeftIndent = 10;

            section.AddParagraph()
                .AddBookmark("section-1");
            
            //每个部分的相似文章列表。
            table = section.AddTable();
            table.Rows.VerticalAlignment = VerticalAlignment.Center;
            table.BottomPadding = "5pt";
            table.Borders.Top.Width = "1pt";
            table.Borders.Bottom.Width = "1pt";
            table.Borders.Color = new Color(0xffebeef5);
            table.AddColumn("1cm");
            table.AddColumn("15.5cm");
            table.AddColumn("3cm");

            row = table.AddRow();
            row.TopPadding = "0.5cm";
            row.BottomPadding = "0.5cm";
            row.Shading.Color = new Color(0xffe2f3d9);
            row.Cells[0].MergeRight = 2;
            paragraph = row.Cells[0].AddParagraph();
            paragraph.Style = "tableH1";
            paragraph.AddText("1. 第一部分的标题，其实就是文章名称加下划线_第1部分");
            paragraph.AddLineBreak();
            paragraph.AddFormattedText("总字数：0000", "tableH2");

            row = table.AddRow();
            row.Height = "1cm";
            row.Cells[0].MergeRight = 2;
            row.Cells[0].AddParagraph("相似文献列表")
                .Format.LeftIndent = 10;

            row = table.AddRow();
            row.Height = "1cm";
            row.Cells[0].MergeRight = 2;
            paragraph = row.Cells[0].AddParagraph();
            paragraph.Format.LeftIndent = 10;
            paragraph.AddFormattedText("去除本人已发表文献复制比：", "tableGrayText");
            paragraph.AddText("3.1%（234）\t");
            paragraph.AddFormattedText("| 文字复制比：", "tableGrayText");
            paragraph.AddText("3.1%（234）\t");
            paragraph.AddFormattedText("| 疑似剽窃观点：", "tableGrayText");
            paragraph.AddText("（0）");

            row = table.AddRow();
            row.Cells[0].AddParagraph("1").Format.Alignment = ParagraphAlignment.Center;
            paragraph = row.Cells[1].AddParagraph();
            paragraph.AddHyperlink("https://baidu.com", HyperlinkType.Url)
                .AddFormattedText("重复字数最多的一篇文章的标题，重复字数最多的一篇文章的标题", "href");
            paragraph.AddLineBreak();
            paragraph.AddText("作者姓名");
            paragraph.AddTab();
            paragraph.AddHyperlink("https://www.qikan.com", HyperlinkType.Url)
                .AddFormattedText("《期刊名称》2020年6期", "href");
            row.Cells[2].AddParagraph()
                .AddFormattedText("1.3%（130）", "tableRedText");
            row.Cells[2].AddParagraph("是否引证：否");

            row = table.AddRow();
            row.Cells[0].AddParagraph("1").Format.Alignment = ParagraphAlignment.Center;
            paragraph = row.Cells[1].AddParagraph();
            paragraph.AddHyperlink("https://baidu.com", HyperlinkType.Url)
                .AddFormattedText("重复字数第二多的一篇文章，", "href");
            paragraph.AddLineBreak();
            paragraph.AddText("作者姓名");
            paragraph.AddTab();
            paragraph.AddHyperlink("https://www.qikan.com", HyperlinkType.Url)
                .AddFormattedText("《期刊名称》2020年6期", "href");
            row.Cells[2].AddParagraph()
                .AddFormattedText("1.3%（130）", "tableRedText");
            row.Cells[2].AddParagraph("是否引证：否");


            section.AddPageBreak();
            paragraph = section.AddParagraph("看");
            paragraph.AddTab();
            paragraph.AddText("感较为老规矩");

            for (int i = 1; i < 19; i++)
            {
                paragraph = section.AddParagraph("看");
                paragraph.Format.AddTabStop(i + "cm");
                paragraph.AddTab();
                paragraph.AddText(i + "cm");
            }

            section.AddPageBreak();


            End(document);
        }

        private static Document CreateDocument(string subtitle, DateTimeOffset createdAtDateTime, string articleTitle, string author)
        {
            const string imgFile = "/Users/jess/myun/Upload/logo.png";
            Document result = new Document();
            DefineStyles(result);
            //result.DefaultPageSetup.DifferentFirstPageHeaderFooter = true; //首页的页眉和页脚不同于其它页。开启后 section.Headers.FirstPage.AddParagraph("首页才有的");
            result.DefaultPageSetup.HeaderDistance = result.DefaultPageSetup.FooterDistance = Unit.FromPoint(15d);
            //result.DefaultPageSetup.TopMargin = result.DefaultPageSetup.BottomMargin = Unit.FromPoint(5d);
            result.DefaultPageSetup.LeftMargin = result.DefaultPageSetup.RightMargin = Unit.FromPoint(25d);
            

            var section = result.AddSection();
            //基数页和偶数页的页眉和页脚不同。
            //开启后 section.Headers.Primary、section.Footers.Primary设置奇数页。section.Headers.EvenPage、section.Footers.EvenPage设置偶数页。
            //section.PageSetup.OddAndEvenPagesHeaderFooter = true;
            //HeaderFooter header = section.Headers.Primary;
            //header.AddParagraph("\tOdd Page Header");
            //header = section.Headers.EvenPage;
            //header.AddParagraph("Even Page Header");
            section.PageSetup.StartingNumber = 1;


            Paragraph paragraph = new Paragraph();
            paragraph.AddText("- ");
            paragraph.AddPageField();
            paragraph.AddText(" -");
            section.Footers.Primary.Add(paragraph);
            //section.Footers.EvenPage.Add(paragraph.Clone());

            var textFrame = section.AddTextFrame();
            //textFrame.LineFormat.Width = Unit.FromPoint(1d);
            textFrame.Width = result.DefaultPageSetup.PageWidth - Unit.FromPoint(50d);
            textFrame.MarginTop = Unit.FromPoint(10d);
            textFrame.RelativeVertical = RelativeVertical.Page;

            var logo = textFrame.AddImage(ImageSource.FromFile(imgFile));
            logo.Height = "1.5cm";
            logo.RelativeVertical = RelativeVertical.Margin;

            paragraph = textFrame.AddParagraph();
            paragraph.Style = StyleNames.Heading1;
            paragraph.AddText("文本复制检测报告单");

            paragraph = textFrame.AddParagraph();
            paragraph.Style = StyleNames.Heading2;
            paragraph.AddText(subtitle);

            //灰色通栏线
            textFrame = section.AddTextFrame();
            textFrame.Width = result.DefaultPageSetup.PageWidth;
            textFrame.Height = Unit.FromPoint(0d);
            textFrame.RelativeHorizontal = RelativeHorizontal.Page;
            textFrame.LineFormat.Width = Unit.FromPoint(0.5d);
            textFrame.LineFormat.Color = Colors.LightGray;
            textFrame.WrapFormat.DistanceBottom = Unit.FromPoint(10d);

            //报告编号 和 检查时间
            textFrame = section.AddTextFrame();
            textFrame.Width = result.DefaultPageSetup.PageWidth - Unit.FromPoint(50d);
            textFrame.Height = Unit.FromPoint(18d);
            textFrame.FillFormat.Color = Colors.LightGreen; //new Color(0xffe2f3d9)
            paragraph = textFrame.AddParagraph();
            paragraph.Format.AddTabStop("11.5cm");
            paragraph.AddSpace(4);
            paragraph.AddFormattedText($"№:DSCC{createdAtDateTime.ToUniversalTime():yyyyMMddHHmmssfffffff}", "GreenBlock");
            paragraph.AddTab();
            paragraph.AddFormattedText($"检测时间：{createdAtDateTime:yyyy年MM月dd日HH:mm:sszzz}", "GreenBlock");
            paragraph.Format.Font.Size = 9;

            //基本信息
            section.AddParagraph();

            var table = section.AddTable();
            table.TopPadding = "2pt";
            table.BottomPadding = "3pt";
            table.LeftPadding = "3pt";
            table.RightPadding = "3pt";
            //table.Borders.Width = "1pt";
            table.AddColumn("3cm").Format.Alignment = ParagraphAlignment.Right;
            table.AddColumn("16.5cm");
            var row = table.AddRow();
            row.Cells[0].AddParagraph("检查文献：");

            paragraph = row.Cells[1].AddParagraph();
            paragraph.Style = "DimGray";
            GetFontNameAndSize(result, "DimGray", out var familyName, out var fontSize);
            string[] texts = CutLongText(articleTitle, row.Cells[1].Column.Width.Point, familyName, fontSize);
            for (int i=0; i< texts.Length; i++)
            {
                if (i > 0) paragraph.AddLineBreak();

                paragraph.AddText(texts[i]);
            }

            row = table.AddRow();
            row.Cells[0].AddParagraph("作者：");
            row.Cells[1].AddParagraph().AddFormattedText(author, "DimGray");

            row = table.AddRow();
            row.Cells[0].AddParagraph("检测范围：");
            paragraph = row.Cells[1].AddParagraph("中国学术期刊网络出版总库");
            paragraph.Style = "DimGray";
            paragraph.AddLineBreak();
            paragraph.AddText("龙源期刊库");

            row = table.AddRow();
            row.Cells[0].AddParagraph("时间范围：");
            row.Cells[1].AddParagraph().AddFormattedText($"1900-01-01 ~ {DateTime.Today:yyyy-MM-dd}", "DimGray");

            return result;
        }


        private static void DefineStyles(Document document)
        {
            Style style = document.Styles[StyleNames.Normal];
            style.Font.Name = fontsName[0];
            style.Font.Size = 10;
            //style.Font.Color = new Color(0xff686a6e);

            //标题1
            style = document.Styles[StyleNames.Heading1];
            style.Font.Name = fontsName[1];
            style.Font.Size = 22;
            style.Font.Bold = true;
            //style.Font.Color = Colors.DarkBlue; //深蓝色
            //style.ParagraphFormat.PageBreakBefore = true; //true 换页符
            style.ParagraphFormat.Alignment = ParagraphAlignment.Center;

            //标题2
            style = document.Styles[StyleNames.Heading2];
            style.Font.Name = fontsName[1];
            style.Font.Size = 13;
            style.Font.Color = Colors.Gray; 


            //绿色块基础样式
            style = document.AddStyle("GreenBlock", StyleNames.Normal);
            style.Font.Color = Colors.Green; //new Color(0xff6bc145)

            //暗灰色
            style = document.AddStyle("DimGray", StyleNames.Normal);
            style.Font.Color = Colors.DimGray;

            //超链接
            style = document.AddStyle("href", StyleNames.Normal);
            style.Font.Color = new Color(0xff469bf5);
            style.Font.Italic = true;
            style.Font.Bold = true;

            //红色字
            style = document.AddStyle("red-text", StyleNames.Normal);
            style.Font.Color = Colors.Red;

            //绿色字
            style = document.AddStyle("green-text", StyleNames.Normal);
            style.Font.Color = Colors.Green;

            //重复字数、总段落数
            style = document.AddStyle("table-item1", StyleNames.Normal);
            style.Font.Color = Colors.Gray;



            //被检测文章的每个部分表格的表头
            style = document.AddStyle("tableH1", StyleNames.Normal);
            style.Font.Size = 12;
            style.Font.Color = new Color(0xff6ab344);//Colors.DarkOliveGreen;
            style.ParagraphFormat.LeftIndent = 10;

            style = document.AddStyle("tableH2", "tableH1");
            style.Font.Color = new Color(0xff606266);

            style = document.AddStyle("tableRedText", StyleNames.Normal);
            style.Font.Color = new Color(0xfff36d6f);

            style = document.AddStyle("tableGrayText", StyleNames.Normal);
            style.Font.Color = new Color(0xff909399);
            

            //页脚
            style = document.Styles[StyleNames.Footer];
            style.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            //style.ParagraphFormat.AddTabStop("10.5cm", TabAlignment.Center);
        }


        private static void End(Document document)
        {
            var table = document.LastSection.AddTable();
            table.Rows.Height = "1cm";
            table.Rows.VerticalAlignment = VerticalAlignment.Center;
            table.Rows.HeightRule = RowHeightRule.AtLeast;
            table.AddColumn("1.5cm");
            table.AddColumn("17cm");

            var row = table.AddRow();
            row.Borders.Top.Width = "1pt";
            row.Cells[0].AddParagraph("说明：");
            row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[1].AddParagraph("1. 总文字复制比：被检测论文总重合字数在总字数中所占的比例。");
            row = table.AddRow();
            row.Cells[1].AddParagraph("2. 去除引用文献复制比：去除系统识别为引用的文献后，计算出来的重合字数在总字数中所占的比例。");
            row = table.AddRow();
            row.Cells[1].AddParagraph("3. 去除本人已发表文献复制比：去除作者本人已发表文献后，计算出来的重合字数在总字数中所占的比例。");
            row = table.AddRow();
            row.Cells[1].AddParagraph("4. 单篇最大文字复制比：被检测文献与所有相似文献比对后，重合字数占总字数的比例最大的那一篇文献的");
            var paragraph = row.Cells[1].AddParagraph();
            paragraph.AddSpace(4);
            paragraph.AddText("文字复制比。");
            row = table.AddRow();
            row.Cells[1].AddParagraph("5. 指标是由系统根据《学术论文不端行为的界定标准》自动生成的。");
            row = table.AddRow();
            paragraph = row.Cells[1].AddParagraph("6. ");
            paragraph.AddFormattedText("红色", "red-text");
            paragraph.AddText("文字表示文字复制部分；");
            paragraph.AddFormattedText("绿色", "green-text");
            paragraph.AddText("文字表示引用部分；棕灰色文字表示作者本人已发表文献部分。");
            row = table.AddRow();
            row.Cells[1].AddParagraph("7. 本报告单仅对您所选择比对资源范围内检测结果负责。");


            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = document;
            renderer.RenderDocument();

            string filename = "/Users/jess/myun/Upload/HelloMigraDoc.pdf";

            renderer.PdfDocument.Save(filename);
        }

        private static bool GetFontNameAndSize(Document document, string styleName, out string name, out double size)
        {
            var style = document.Styles[styleName];
            if (style == null) style = document.Styles[StyleNames.Normal];

            name = String.Empty;
            size = 0d;

            do
            {
                if (name.Length == 0 && !String.IsNullOrEmpty(style.Font.Name))
                {
                    name = style.Font.Name;
                }

                if (size == 0d && style.Font.Size > 0d)
                {
                    size = style.Font.Size;
                }

                style = style.GetBaseStyle();
            }
            while (style != null && (name.Length == 0 || size == 0d));

            return name.Length > 0 && size > 0d;
        }

        /// <summary>
        /// 如果 <paramref name="text"/> 写入 <c>PDF</c> 文档超出指定宽度则将 <paramref name="text"/> 切分成适合的几段文本。
        /// </summary>
        /// <param name="text"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        private static string[] CutLongText(string text, double width, string fontFamilyName, double fontEmSize)
        {
            var font = new PdfSharpCore.Drawing.XFont(fontFamilyName, fontEmSize);

            using (PdfSharpCore.Pdf.PdfDocument sgfegew = new PdfSharpCore.Pdf.PdfDocument())
            {
                using (PdfSharpCore.Drawing.XGraphics g = PdfSharpCore.Drawing.XGraphics.FromPdfPage(sgfegew.AddPage()))
                {
                    var size = g.MeasureString(text, font);

                    if (size.Width > width)
                    {
                        var arrCount = (int)Math.DivRem((long)size.Width, (long)width, out var remainder);
                        if (remainder > 0L) arrCount++;

                        string[] result = new string[arrCount];
                        double minWdith = width - 10d;
                        var len = (int)Math.Floor(width / size.Width * text.Length);
                        int start = 0;
                        

                        for(int i = 0, j = arrCount - 1; i < j; i++)
                        {
                            int tmpLen = len;
                            int lastLen = tmpLen;
                            string str = String.Empty;

                            while (true)
                            {
                                str = text.Substring(start, tmpLen);
                                size = g.MeasureString(str, font);
                                if (size.Width < minWdith)
                                {
                                    if ((tmpLen + 1) == lastLen) break;

                                    tmpLen++;
                                    continue;
                                }

                                if (size.Width > width)
                                {
                                    lastLen = tmpLen;
                                    tmpLen--;
                                    continue;
                                }
                                break;
                            }

                            
                            for (int x = str.Length - 1, y = (int)(str.Length * 0.7); x > y; x--)
                            {
                                if (str[x].IsSBCLetterOrDigit()
                                    || ((str[x] == '.' || str[x] == ',') && Char.IsDigit(str[x - 1]) && Char.IsDigit(text[start + x + 1])))
                                {
                                    continue;
                                }

                                if (str[x] == '-' && Char.IsDigit(text[start + x + 1]))
                                {
                                    if (str[x - 1].IsCurrencySymbol())
                                    {
                                        tmpLen = x - 1;
                                    }
                                    else
                                    {
                                        tmpLen = x;
                                    }
                                }
                                else if (str[x].IsCurrencySymbol() && Char.IsDigit(text[start + x + 1]))
                                {
                                    if (str[x - 1] == '-')
                                    {
                                        tmpLen = x - 1;
                                    }
                                    else
                                    {
                                        tmpLen = x;
                                    }
                                }
                                else
                                {
                                    tmpLen = x + 1;
                                }

                                break;
                            }

                            if (str.Length > tmpLen)
                            {
                                str = str.Substring(0, tmpLen);
                            }

                            start += tmpLen;
                            result[i] = str;
                        }
                        result[^1] = text.Substring(start);
                        return result;
                    }
                    else
                    {
                        return new string[] { text };
                    }
                }
            }
        }
    }
}
