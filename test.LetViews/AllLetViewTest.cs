using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Alignments;
using psdPH.Lets;
using psdPH.Lets.Core;
using psdPH.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace test.LetViews
{
    [TestClass]
    public abstract class AllLetViewTest
    {
        [TestMethod]
        public void Use_AllTest()
        {
            LetView view = GetLetView();
            Let let = view.Let;
            SetupLet(let);
            var control = view.Control;
            var window = new Window() { Content = control };
            window.ShowDialog();
            MessageBox.Show(let.Value.ToString());
        }
        protected abstract LetView GetLetView();
        protected Let GetLet()
        {
            return new Let(GetConfig());
        }
        protected abstract ReflectionConfig GetConfig();
        protected abstract void SetupLet(Let let);
   
    }
}
