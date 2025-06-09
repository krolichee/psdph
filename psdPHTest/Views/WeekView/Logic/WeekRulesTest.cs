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

namespace psdPHTest.Views.WeekView.Logic
{
    [TestClass]
    public class WeekRules : WeekViewTest
    {
        [TestMethod]
        public void FromBlobTest()
        {
            Blob blob = GetBlob();
            var dayBlob = DowBlob.FromBlob(blob, 0, DayOfWeek.Monday);
        }

        EveryNDayCondition GetEveryNDayCondition(int interval, DateTime startDateTime, DowBlob blob)
        {
            return new EveryNDayCondition(blob) { Interval = interval, StartDateTime = startDateTime };
        }
        [DataTestMethod]
        [DataRow(2, DayOfWeek.Monday, true)]
        [DataRow(3, DayOfWeek.Tuesday, false)]
        [DataRow(2, DayOfWeek.Wednesday, true)]
        [DataRow(3, DayOfWeek.Thursday, true)]
        public void EveryNDayConditionTest(int interval, DayOfWeek dayOfWeek, bool result)
        {
            var blob = GetBlob();
            var weekConfig = GetWeekConfig();

            var currentWeek = 0;

            var startDateTime = WeekTime.GetDateByWeekAndDay(currentWeek, DayOfWeek.Monday);
            var dayBlob = new DowBlob() { Dow = dayOfWeek, Week = currentWeek };
            var everynCondition = GetEveryNDayCondition(interval, startDateTime, dayBlob);

            Assert.IsTrue(everynCondition.IsValid() == result);

        }
        [DataTestMethod]
        [DataRow(2, DayOfWeek.Monday, true)]
        [DataRow(3, DayOfWeek.Tuesday, false)]
        [DataRow(2, DayOfWeek.Wednesday, true)]
        [DataRow(3, DayOfWeek.Thursday, true)]
        public void WeekRulesTest(int interval, DayOfWeek dayOfWeek, bool result)
        {
            var blob = GetBlob();
            var weekConfig = GetWeekConfig();

            weekConfig.GetDayBlob(blob).ParameterSet.Add(new FlagParameter("uvu"));

            var weekListData = WeekListData.Create(weekConfig, new WeekRulesets(), blob);
            var currentWeek = 0;

            weekListData.NewWeek(currentWeek);
            var weekData = weekListData.Weeks[0];

            var startDateTime = WeekTime.GetDateByWeekAndDay(currentWeek, DayOfWeek.Monday);
            var everynCondition = new EveryNDayCondition() { Interval = interval, StartDateTime = startDateTime };
            var everynRule = new FlagRule() { FlagName = "uvu", Condition = everynCondition };

            weekListData.WeekRulesets.DayRules.AddRule(everynRule);
            var mainBlob = weekData.Prepare();

            var dayPh = mainBlob.GetChildren<PlaceholderLeaf>().First(ph => (ph.Replacement as DowBlob).Dow == dayOfWeek);
            var dayBlob = dayPh.Replacement as DowBlob;

            var resultFlagParameter = (dayBlob.ParameterSet.First(f => f.Name == "uvu") as FlagParameter);

            Assert.IsTrue(resultFlagParameter.Toggle == result);
        }
        [TestMethod]
        public void testInjectRules()
        {
            var blob = GetBlob();
            var weekConfig = GetWeekConfig();
            weekConfig.GetDayBlob(blob).ParameterSet.Add(new FlagParameter("test1"));

            blob.ParameterSet.Add(new FlagParameter("test"));

            var weekListData = WeekListData.Create(weekConfig, new WeekRulesets(), blob);
#pragma warning disable CS0612 // Тип или член устарел
            var weekCondition = new WeekCondition(blob) { Week = WeekTime.CurrentWeek };
#pragma warning restore CS0612 // Тип или член устарел
            var weekFlagRule = new FlagRule() { Condition = weekCondition, FlagName = "test" };

            var ndayCondition = new EveryNDayCondition(blob) { Interval = 2, StartDateTime = WeekTime.GetDateByWeekAndDay(WeekTime.CurrentWeek, DayOfWeek.Monday) };
            var dayFlagRule = new FlagRule() { Condition = weekCondition, FlagName = "test1" };


            weekListData.WeekRulesets.DayRules.AddRule(dayFlagRule);
            weekListData.WeekRulesets.WeekRules.AddRule(weekFlagRule);

            weekListData.NewWeek();

            var weekBlob = weekListData.Weeks[0].Prepare();
            weekBlob.CoreApply();

            Assert.IsTrue(weekBlob.ParameterSet.GetByType<FlagParameter>().First(f => f.Name == "test").Toggle);

            var mondayBlob = weekBlob.GetChildren<PlaceholderLeaf>().First(ph => (ph.Replacement as DowBlob).Dow == DayOfWeek.Monday).Replacement;
            Assert.IsTrue(mondayBlob.ParameterSet.GetByType<FlagParameter>().First(f => f.Name == "test1").Toggle);
        }
        [TestClass]
        public class ParameterTest
        {
            public HorizontalAlignment HA;
            [TestMethod]
            public void testEnumAuto()
            {
                var config = new SetupConfig(this, nameof(HA), "aaa");
                var parameter = Setup.EnumChoose(config, typeof(HorizontalAlignment));
                Console.WriteLine(parameter.Control as ComboBox);
            }
        }
    }
}
