using psdPH.Alignments;
using psdPH.Lets;
using psdPH.Lets.Core;
using psdPH.LetViews;
using psdPH.LetViews.Check;
using psdPH.LetViews.Choose;
using psdPH.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static test.LetViews.AlignmentLetViewTests;
using static test.LetViews.CheckLetViewTests;
using static test.LetViews.ChooseLetViewTests;
using static test.LetViews.DateLetViewTests;
using static test.LetViews.EnumLetViewTests;

namespace test.LetViews
{
    class AllLetViewTests
    {
        Let let;
        public void AlignmentTest()
        {
            var obj = new AlignmentTestObj();
            var config = new ReflectionConfig(obj, nameof(AlignmentTestObj.Alignment));
            let = new Let(config);
            let.Value = true;
            let.Value = new Alignment(HAilgnment.Left, VAilgnment.Top);
            var view = new AlignmentLetView(let);
            var control = view.Control;
            var window = new Window() { Content = control };
            window.ShowDialog();
            MessageBox.Show(let.Value.ToString());
        }
        public void CheckLetTest()
        {
            var obj = new CheckTestObj();
            var config = new ReflectionConfig(obj, nameof(CheckTestObj.Bool));
            let = new Let(config);
            let.Value = true;
            var view = new CheckLetView(let);
            var control = view.Control;
            var window = new Window() { Content = control };
            window.ShowDialog();
            MessageBox.Show(let.Value.ToString());
        }
        public void ChooseTest()
        {
            var obj = new ChooseTestObj();
            var config = new ReflectionConfig(obj, nameof(ChooseTestObj.Selected));
            let = new Let(config);
            obj.Selected = 5;
            var view = new ChooseLetView(let, obj.Options.Cast<object>().ToArray());
            var control = view.Control;
            var window = new Window() { Content = control };
            window.ShowDialog();
            MessageBox.Show(let.Value.ToString());
        }
        public void DateTest()
        {
            var obj = new DateTestObj();
            var config = new ReflectionConfig(obj, nameof(DateTestObj.Date));
            let = new Let(config);
            var view = new DateLetView(let);
            var control = view.Control;
            var window = new Window() { Content = control };
            window.ShowDialog();
            MessageBox.Show(let.Value.ToString());
        }
        public void EnumTest()
        {
            var obj = new EnumTestObj();
            var config = new ReflectionConfig(obj, nameof(EnumTestObj.Scope));
            let = new Let(config);
            var view = new EnumLetView(let);
            var control = view.Control;
            var window = new Window() { Content = control };
            window.ShowDialog();
            MessageBox.Show(let.Value.ToString());
        }
        public void AllTest(ReflectionConfig config, LetView view)
        {
            let = new Let(config);
            var control = view.Control;
            var window = new Window() { Content = control };
            window.ShowDialog();
            MessageBox.Show(let.Value.ToString());
        }
    }
}
