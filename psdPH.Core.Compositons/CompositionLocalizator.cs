using psdPH.Compositions;
using psdPH.Localization;
using psdPH.Logic.Compositions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Core.Compositons
{
    [Localizator]
    public static class CompositionLocalizator
    {
        public static void RegisterLocalizations()
        {
            TypeLocalization.RegisterLocalization(
            new Dictionary<Type, string>
        {
                {typeof(RootBlob), "Поддокумент" },

                {typeof(PrototypeBlob), "Прототип" },
                { typeof(PlaceholderLeaf), "Заглушка" },

                //{typeof(ImageLeaf), "Изображение" },
                { typeof(TextLeaf), "Текст" },
                { typeof(LayerLeaf), "Слой" },
                { typeof(GroupLeaf), "Группа" },
                { typeof(AreaLeaf),"Зона" }

        });
        }
    }
}
