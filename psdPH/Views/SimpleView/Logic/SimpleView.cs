using psdPH.Logic.Compositions;
using psdPH.Utils;
using psdPH.Views.SimpleView.Windows;
using psdPH.Views.WeekView;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace psdPH.Views.SimpleView.Logic
{
    public class SimpleView:View<SimpleListData>
    {
        public static SimpleView MakeSimpleView()
        {
            return (_instance = new SimpleView()) as SimpleView;
        }
        public override string ViewName=>"SimpleView";

        public override Window ShowWindow()
        {
            if (ListData == null)
                return null;
            var window = new SimpleViewWindow(ListData);
            window.Show();
            return window;
        }
        protected override SimpleListData openOrCreateData()
        {
            var project = PsdPhProject.Instance();
            RootBlob blob = project.openOrCreateMainBlob();
            SimpleListData simpleListData;
            simpleListData = OpenData();
            if (simpleListData == null)
                simpleListData = new SimpleListData(blob);
            else
                simpleListData.Restore(blob);
            return simpleListData;
        }

        protected override void SaveListData(SimpleListData listData)
        {
            DiskOperations.SaveXml(ListDataPath, listData);
        }
    }
}
