using Photoshop;
using psdPH.Logic;
using psdPH.Views.WeekView.Logic;
using psdPH.Views.WeekView;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace psdPH.Views.SimpleView.Logic
{
    class SimpleRenderer
    {
        static void Render(SimpleData simpleData, Document doc)
        {
            var preparedBlob = simpleData.Prepare();
            MatchingResult match = preparedBlob.IsMatchingRouted(doc);
            if (!match)
            {
                MessageBox.Show($"Выявлено несоответствие структуры документа и шаблона. Не найдены подходящие части документа для" +
                    $"\n{match}");
                return;
            }
            preparedBlob.Apply(doc);
            var outputName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            var outputDirectory = SimpleView.Instance().OutputDirectory(outputName);
            SimpleView.Instance().CreateOutputDirectory(outputName);
            new OutputSaver(outputDirectory).Save(doc);
            doc.Rollback();
            Process.Start(outputDirectory);
        }
        public static void Render(SimpleData simpleData)
        {
            var projectName = PsdPhProject.Instance().ProjectName;
            var psdPath = PsdPhDirectories.ProjectPsd(projectName);
            var doc = PhotoshopWrapper.OpenDocument(psdPath, true);
            if (doc == null)
                return;
            Render(simpleData, doc);

        }
    }
}

