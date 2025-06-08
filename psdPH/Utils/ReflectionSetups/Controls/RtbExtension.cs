using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace psdPH.Utils.ReflectionParameter
{
    public static class RtbExtension
    {
        public static void SetText(this RichTextBox rtb, string text)=>
            rtb.Document = convertStringToFlowDocument(text);
        public static string GetText(this RichTextBox rtb)=>getRtbText(rtb);
        static FlowDocument convertStringToFlowDocument(string text)
        {
            if (string.IsNullOrEmpty(text))
                return new FlowDocument();

            FlowDocument flowDoc = new FlowDocument();
            Paragraph paragraph = new Paragraph();

            string[] lines = text.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                paragraph.Inlines.Add(new Run(lines[i]));
                if (i < lines.Length - 1)
                    paragraph.Inlines.Add(new LineBreak());
            }
            flowDoc.Blocks.Add(paragraph);
            return flowDoc;
        }
        static string getRtbText(RichTextBox _rtb, string lineSep = "\n")
        {
            string _result = "";
            foreach (Paragraph item in (_rtb).Document.Blocks)
                foreach (var item1 in item.Inlines)
                    if (item1 is Run)
                    {
                        var run = (Run)item1;
                        if (_result != "")
                            _result += lineSep;
                        _result += run.Text;

                    }
            return _result;
        }
    }
}
