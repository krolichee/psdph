using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Logic.Compositions;
using psdPH.Logic.Parameters;
using psdPH.Logic;
using psdPH.Views.WeekView.Logic;
using psdPH;
using psdPHTest.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using psdPH.Views.WeekView;
using psdPH.Logic.Ruleset.Rules;
using psdPH.Utils.Setups;

namespace psdPHTest.Views.WeekView.Logic
{
    [TestCategory(TestCategories.Automatic)]
    [TestClass]
    public class ParameterTest
    {
        public HorizontalAlignment HA;
        [TestMethod]
        public void testEnumAuto()
        {
            var config = new SetupConfig(this, nameof(HA), "aaa");
            var parameter = EnumChooseSetup.EnumChoose(config, typeof(HorizontalAlignment));
            Console.WriteLine(parameter.Control as ComboBox);
        }
    }
}

