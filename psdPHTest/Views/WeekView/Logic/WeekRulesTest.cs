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

namespace psdPHTest.Views.WeekView.Logic
{
    [TestClass]
    [TestCategory(TestCatagories.Automatic)]
    public class WeekRules : WeekViewTest
    {
        EveryNDayCondition GetEveryNDayCondition(int interval, DateTime startDateTime, DayParameterSet parset)
        {
            return new EveryNDayCondition(parset) { Interval = interval, StartDateTime = startDateTime };
        }
        [DataTestMethod]
        [DataRow(2, DayOfWeek.Monday, true)]
        [DataRow(3, DayOfWeek.Tuesday, false)]
        [DataRow(2, DayOfWeek.Wednesday, true)]
        [DataRow(3, DayOfWeek.Thursday, true)]
        public void EveryNDayConditionTest(int interval, DayOfWeek dayOfWeek, bool result)
        {
            var weekConfig = GetWeekConfig();
            var currentWeek = WeekTime.CurrentWeek;
            var startDateTime = WeekTime.GetDateByWeekAndDay(currentWeek, DayOfWeek.Monday);
            var dayParset = new DayParameterSet(dayOfWeek, currentWeek);
            var everynCondition = GetEveryNDayCondition(interval, startDateTime, dayParset);

            Assert.IsTrue(everynCondition.IsValid() == result);

        }
        [DataTestMethod]
        [DataRow(2, DayOfWeek.Monday, true)]
        [DataRow(2, DayOfWeek.Tuesday, false)]
        [DataRow(3, DayOfWeek.Monday, true)]
        [DataRow(3, DayOfWeek.Tuesday, false)]
        [DataRow(3, DayOfWeek.Wednesday, false)]
        public void WeekRulesTest(int interval, DayOfWeek dayOfWeek, bool result)
        {
            var blob = GetWeekBlob();
            
            var weekConfig = GetWeekConfig();

            var dayBlob = weekConfig.GetDayBlob(blob);
            var flagParameter = new FlagParameter("uvu");
            dayBlob.ParameterSet.AsCollection().Add(flagParameter);

            var currentWeek = WeekTime.CurrentWeek;
            var startDateTime = WeekTime.GetDateByWeekAndDay(currentWeek, DayOfWeek.Monday);
            var weekListData = WeekListData.Create(weekConfig, blob);
            var everyNDayCondition = new EveryNDayCondition() { Interval = interval, StartDateTime = startDateTime };
            var flagRule = new FlagRule() { FlagParameter = flagParameter, Condition = everyNDayCondition,Value = true};

            weekListData.WeekRulesets.DayRules.AddRule(flagRule);
            weekListData.NewWeek();
            dayBlob.ParameterSet = null;
            var weekData = weekListData.Weeks[0];

            Console.WriteLine($"При interval = {interval}, DayOfWeek = {dayOfWeek} и ожидаемо {result}");
            foreach (var dayParset in weekData.DayParsetsList)
            {
                Console.WriteLine($"{dayParset.Dow}:{dayParset.AsCollection().First(p=>p.Name == flagParameter.Name).Value}");
            }

            Assert.IsTrue(weekData.DowParsetDict[dayOfWeek].GetByType<FlagParameter>().First(p => p.Name == flagParameter.Name).Toggle == result);
        }
        //[TestMethod]
        public void testInjectRules()
        {
            var blob = GetWeekBlob();
            var weekConfig = GetWeekConfig();
            weekConfig.GetDayBlob(blob).ParameterSet.Add(new FlagParameter("test1"));

            blob.ParameterSet.Add(new FlagParameter("test"));

            var weekListData = WeekListData.Create(weekConfig, new WeekRulesets(), blob);
#pragma warning disable CS0612 // Тип или член устарел
            var weekCondition = new WeekCondition(blob) { Week = WeekTime.CurrentWeek };
#pragma warning restore CS0612 // Тип или член устарел
            var weekFlagRule = new FlagRule() { Condition = weekCondition, FlagName = "test" };
            Assert.Fail();

            //var ndayCondition = new EveryNDayCondition(blob) { Interval = 2, StartDateTime = WeekTime.GetDateByWeekAndDay(WeekTime.CurrentWeek, DayOfWeek.Monday) };
            //var dayFlagRule = new FlagRule() { Condition = weekCondition, FlagName = "test1" };


            //weekListData.WeekRulesets.DayRules.AddRule(dayFlagRule);
            //weekListData.WeekRulesets.WeekRules.AddRule(weekFlagRule);

            //weekListData.NewWeek();

            //var weekBlob = weekListData.Weeks[0].Prepare();
            //weekBlob.CoreApply();

            //Assert.IsTrue(weekBlob.ParameterSet.GetByType<FlagParameter>().First(f => f.Name == "test").Toggle);

            //var mondayBlob = weekBlob.GetChildren<PlaceholderLeaf>().First(ph => (ph.Replacement as DowBlob).Dow == DayOfWeek.Monday).Replacement;
            //Assert.IsTrue(mondayBlob.ParameterSet.GetByType<FlagParameter>().First(f => f.Name == "test1").Toggle);
        }
        [TestCategory(TestCatagories.Automatic)]
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
