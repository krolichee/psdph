using psdPH.Logic.Compositions;
using psdPH.Logic.Parameters;
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
    class SimpleViewHandler : CEDPanelHandler
    {
        SimpleListData SimpleListData;
        public SimpleViewHandler(SimpleListData simpleViewList)
        {
            SimpleListData = simpleViewList;
            simpleViewList.Variants.CollectionChanged += (_, __) => Refresh();
        }
        protected override FrameworkElement createControl(object item)=>new SimpleControl(item as SimpleData, SimpleListData);
        protected override object[] getElements()=>SimpleListData.Variants.ToArray();
        protected override void AddButtonAction()=>new SimpleViewCommand(SimpleListData).CreateCommand.Execute(SimpleListData);
        protected override IList Items => this.SimpleListData.Variants as IList;
    }
}
