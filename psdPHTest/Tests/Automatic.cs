using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Logic.Compositions;
using psdPH.Logic.Rules;
using psdPH.Logic;
using psdPH.Views.WeekView.Logic;
using psdPH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static psdPH.Logic.PhotoshopDocumentExtension;
using System.Windows.Controls;
using System.Windows;
using Condition = psdPH.Logic.Rules.Condition;
using HAli = System.Windows.HorizontalAlignment;
using VAli = System.Windows.VerticalAlignment;
using psdPH.Views.WeekView;
using psdPHTest.Tests;
using psdPH.Utils;
using Photoshop;
using psdPH.Logic.Parameters;

namespace psdPHTest.Tests.Automatic
{
    [TestClass]
    public class XMLSerializationTest
    {
        [TestMethod]
        public void testSave()
        {
            var blob = Blob.PathBlob("очень сильно заболел хуй");
            Assert.IsTrue(DiskOperations.SaveXml("test.xml", blob).Serialized);
        }
        [TestMethod]
        public void testOpen()
        {
            DiskOperations.OpenXml<Blob>("test.xml");
        }
        [TestMethod]
        public void testKnownTypes()
        {
            var blob = Blob.PathBlob("test.xml");
            var cond = new DummyCondition(null);
            blob.RuleSet.AddRule(new AlignRule(blob) { Alignment = Alignment.Create("up", "left"), AreaLayerName = "2", LayerName = "2" });
            Assert.IsTrue(DiskOperations.SaveXml("test.xml", blob).Serialized);
        }
    }
    [TestClass]
    public class AligmentRuleTest
    {
        Blob MainBlob;
        FlagParameter flagParam;
        void _(object sender, RoutedEventArgs e)
        {
            var doc = PhotoshopWrapper.GetPhotoshopApplication().ActiveDocument;
            flagParam.Toggle = (sender as CheckBox).IsChecked == true;
            MainBlob.Apply(doc);
        }
        Blob GetBlob()
        {
            var blob = Blob.PathBlob("");
            flagParam = new FlagParameter("sadism") { Toggle = true};
            var on_area = new AreaLeaf() { LayerName = "on_area" };
            var off_area = new AreaLeaf() { LayerName = "off_area" };
            var layer1Leaf = new TextLeaf() { LayerName = "lorem", Text = "Lorem Ipsum" };
            var objLayer = new TextLeaf() { LayerName = "obj" };
            var controlLayer = new LayerLeaf() { LayerName = "control" };
            blob.ParameterSet.Add(flagParam);
            blob.AddChild(on_area);
            blob.AddChild(off_area);
            blob.AddChild(layer1Leaf);
            Condition true_condition = new FlagCondition(blob) { FlagParameter = flagParam, Value = true };
            Condition false_condition = new FlagCondition(blob) { FlagParameter = flagParam, Value = false };
            blob.RuleSet.Rules.Add(
                new AlignRule(blob)
                {
                    LayerComposition = layer1Leaf,
                    AreaLeaf = on_area,
                    Alignment = Alignment.Create("center", "left"),
                    Condition = true_condition
                }
                );
            blob.RuleSet.Rules.Add(
                new AlignRule(blob)
                {
                    LayerComposition = layer1Leaf,
                    AreaLeaf = off_area,
                    Alignment = Alignment.Create("center", "left"),
                    Condition = false_condition
                }
                );
            blob.RuleSet.Rules.Add(
                new VisibleRule(blob) { LayerComposition = objLayer, Condition = true_condition }
                );
            return blob;
        }
        [TestInitialize]
        public void Init()
        {
            MainBlob = GetBlob();
            var doc = PhotoshopWrapper.GetPhotoshopApplication().ActiveDocument;
            var window = new Window();
            var chb = new CheckBox();
            window.Content = chb;
            chb.HorizontalAlignment = HAli.Center;
            chb.VerticalAlignment = VAli.Center;
            chb.Click += _;
            window.Height = window.MinHeight = 100;
            window.Width = window.MinWidth = 70;
            window.WindowStyle = WindowStyle.ToolWindow;
            window.ShowDialog();
        }
        [TestMethod]
        public void AlignRuleTest()
        {

        }
    }
    [TestClass]
    public class ParameterTest
    {
        public HorizontalAlignment HA = HorizontalAlignment.Stretch;
        [TestMethod]
        public void testEnumAuto()
        {
            var initVal = HA;
            var config = new SetupConfig(this, nameof(HA), "aaa");
            var parameter = Setup.EnumChoose(config, typeof(HorizontalAlignment));
            HorizontalAlignment comboboxValue = (HorizontalAlignment)((parameter.Control as ComboBox).SelectedValue as EnumWrapper).Value ;
            Assert.IsNotNull(comboboxValue);
            Assert.IsTrue(comboboxValue  == initVal);
            Console.WriteLine(comboboxValue);
        }
    }

}



