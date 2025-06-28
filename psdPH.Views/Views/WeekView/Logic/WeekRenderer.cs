using Photoshop;
using psdPH.Logic;
using psdPH.Logic.Compositions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace psdPH.Views.WeekView.Logic
{
    class WeekRenderer
    {
        static void RenderWeek(WeekData weekData, Document doc)
        {
            var preparedBlob = weekData.Prepare();
            MatchingResult match = preparedBlob.IsMatchingRouted(doc);
            if (!match)
            {
                MessageBox.Show($"Выявлено несоответствие структуры документа и шаблона. Не найдены подходящие части документа для" +
                    $"\n{match}");
                return;
            }
            preparedBlob.Apply(doc);
            var outputName = weekData.WeekConfig.GetWeekDatesString(weekData.Week);
            var outputDirectory = WeekView.Instance().OutputDirectory(outputName);
            WeekView.Instance().CreateOutputDirectory(outputName);
            new OutputSaver(outputDirectory).Save(doc);
            doc.Rollback();
            Process.Start(outputDirectory);
        }
        public static void RenderWeek(WeekData weekData)
        {
            var projectName = PsdPhProject.Instance().ProjectName;
            var psdPath = PsdPhDirectories.ProjectPsd(projectName);
            var doc = PhotoshopWrapper.OpenDocument(psdPath,true);
            if (doc == null)
                return;
            RenderWeek(weekData, doc);
            
        }
    }
}
