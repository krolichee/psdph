using psdPH.Logic.Compositions;
using psdPH.Utils.CedStack;
using psdPH.Views.SimpleView.Logic;
using psdPH.Views.SimpleView.SimpleViewCedStack;
using psdPH.Views.WeekView;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace psdPH.Views.SimpleView.Windows.SimpleViewCedStack
{
    class SimpleViewHandler : CEDStackHandler
    {
        SimpleListData SimpleViewList;
        public SimpleViewHandler(SimpleListData simpleViewList)
        {
            SimpleViewList = simpleViewList;
            simpleViewList.Variants.CollectionChanged += (_, __) => Refresh();
        }
        protected override FrameworkElement createControl(object item)=>new CompositionTreeControl((Blob)item);
        protected override object[] getElements()=>SimpleViewList.Variants.ToArray();
        protected override void AddButtonAction()=>new SimpleViewCommand(SimpleViewList).CreateCommand.Execute(SimpleViewList);
        protected override IList Items => this.SimpleViewList.Variants as IList;
    }
}
