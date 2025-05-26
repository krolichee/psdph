using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Logic;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows.Utils;
using psdPH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static psdPH.Logic.PhotoshopDocumentExtension;
using System.Windows.Controls;
using System.Windows;

namespace psdPHTest.UI
{
    [TestClass]
    public class ApplicationConnectionTest
    {
        [TestMethod]
        pub
    }
    [TestClass]
    public class MiscTest
    {
        static string s = string.Empty;
        public string m { get => MiscTest.s; set => MiscTest.s = value; }
        [TestMethod]
        public void ParameterWindowTest()
        {
            ParameterConfig config = new ParameterConfig(this, nameof(this.m), "Строка");
            Parameter[] parameters = new Parameter[] { Parameter.RichStringInput(config) };
            while (new ParametersInputWindow(parameters).ShowDialog() == true) ;
        }
        [TestMethod]
        public void CalendarTest()
        {
            var window = new Window();
            var calendar = new Calendar();
            window.Content = calendar;
            window.ShowDialog();
            // calendar.SelectedDate;
        }
        [TestMethod]
        public void AligmentContolUITest()
        {
            var window = new Window();
            window.SizeToContent = SizeToContent.WidthAndHeight;
            var aliControl = new AlignmentControl(30);
            aliControl.HorizontalAlignment = HorizontalAlignment.Stretch;
            aliControl.VerticalAlignment = VerticalAlignment.Stretch;
            aliControl.VerticalContentAlignment = VerticalAlignment.Stretch;
            aliControl.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            window.Content = aliControl;
            window.ShowDialog();
        }
        [TestMethod]
        public void AligmentContolValuesTest()
        {
            var window = new Window();
            window.SizeToContent = SizeToContent.WidthAndHeight;
            var aliControl = new AlignmentControl();
            aliControl.HorizontalAlignment = HorizontalAlignment.Stretch;
            aliControl.VerticalAlignment = VerticalAlignment.Stretch;
            aliControl.VerticalContentAlignment = VerticalAlignment.Stretch;
            aliControl.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            window.Content = aliControl;

            Dictionary<Button, Alignment> btnAli = new Dictionary<Button, Alignment>()
            {
                { aliControl.upLeft,Alignment.Create("up","left")},
                { aliControl.upCenter,Alignment.Create("up","center")},
                { aliControl.upRight,Alignment.Create("up","right")},
                { aliControl.centerLeft,Alignment.Create("center","left")},
                { aliControl.centerCenter,Alignment.Create("center","center")},
                { aliControl.centerRight,Alignment.Create("center","right")},
                { aliControl.downLeft,Alignment.Create("down","left")},
                { aliControl.downCenter,Alignment.Create("down","center")},
                { aliControl.downRight,Alignment.Create("down","right")}
            };

            foreach (Button button in btnAli.Keys)
            {
                button.Command.Execute(button.CommandParameter);
                Assert.IsTrue(aliControl.GetResultAlignment().Equals(btnAli[button]));
            }
        }
    }
}
