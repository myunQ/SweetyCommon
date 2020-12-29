using System;
using System.Collections.Generic;
using System.IO;

using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Content;
using PdfSharp.Pdf.Content.Objects;
using PdfSharp.Pdf.IO;

namespace Sweety.Common.PerformanceTest
{
    public class PdfSharpCore_PdfSharpUsage
    {
        public PdfSharpCore_PdfSharpUsage()
        {
            PdfDocument document = null;
            string filename = AppDomain.CurrentDomain.BaseDirectory + "temp.pdf";
            
            try
            {
                document = PdfReader.Open(filename, PdfDocumentOpenMode.ReadOnly);
            }
            catch(Exception ex)
            {
                _ = ex.Message;
                using var fileStream = File.OpenRead(filename);
                try
                {
                    document = PdfReader.Open(fileStream, PdfDocumentOpenMode.Import);
                }
                catch(Exception eex)
                {
                    _ = eex.Message;
                }
                
            }
            

            var page = document.Pages[1];

            CObject content = ContentReader.ReadContent(page);

            var extractedText = ExtractText(content);


            var extractedText2 = page.ExtractText();

            if (extractedText == extractedText2)
            {

            }
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


    public static class PdfSharpExtensions2
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
