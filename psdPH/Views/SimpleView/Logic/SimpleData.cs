using psdPH.Logic.Compositions;
using psdPH.Logic.Parameters;
using psdPH.Views.WeekView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace psdPH.Views.SimpleView.Logic
{
    public class SimpleData:ViewData
    {
        SimpleListData SimpleListData;
        public override Blob RootBlob => SimpleListData.RootBlob;
        public ParameterSet ParameterSet = new ParameterSet();
        public SimpleData(SimpleListData simpleListData)
        {
            SimpleListData = simpleListData;
            ParameterSet = RootBlob.ParameterSet.Clone();
        }

        public Blob Prepare()
        {
            //Присваивание заглушкам заменителей
            var mainBlob = RootBlob.Clone();
            mainBlob.ParameterSet = ParameterSet.Clone();
            return mainBlob;
        }
        public void Restore(SimpleListData simpleListData)
        {
            SimpleListData = simpleListData;
            var blobParameterSet = RootBlob.ParameterSet.Clone();
            blobParameterSet.Import(ParameterSet);
            ParameterSet = blobParameterSet;
        }
        public SimpleData() { }
    }
}
