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
    public class SimpleData:ISerializable
    {
        [XmlIgnore]
        public Blob RootBlob => SimpleListData.RootBlob;
        [XmlIgnore]
        public SimpleListData SimpleListData;
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
            this.SimpleListData = simpleListData;
            ParameterSet = RootBlob.ParameterSet.FillWith(ParameterSet);
        }
        public SimpleData() { }
    }
}
