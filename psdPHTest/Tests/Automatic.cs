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
using CompositionCondition = psdPH.Logic.Rules.CompositionCondition;
using HAli = System.Windows.HorizontalAlignment;
using VAli = System.Windows.VerticalAlignment;
using psdPH.Views.WeekView;
using psdPHTest.Tests;
using psdPH.Utils;
using Photoshop;
using psdPH.Logic.Parameters;
using psdPH.Logic.Ruleset.Rules;
using psdPH.Utils.Setups;

namespace psdPHTest.Tests.Automatic
{
    
    [TestClass]
    public class ParameterTest
    {
        public HorizontalAlignment HA = HorizontalAlignment.Stretch;
        [TestCategory(TestCatagories.Automatic)]
        [TestMethod]
        public void testEnumAuto()
        {
            var initVal = HA;
            var config = new SetupConfig(this, nameof(HA), "aaa");
            var parameter = EnumChooseSetup.EnumChoose(config, typeof(HorizontalAlignment));
            HorizontalAlignment comboboxValue = (HorizontalAlignment)((parameter.Control as ComboBox).SelectedValue as EnumWrapper).Value ;
            Assert.IsNotNull(comboboxValue);
            Assert.IsTrue(comboboxValue  == initVal);
            Console.WriteLine(comboboxValue);
        }
    }

}



