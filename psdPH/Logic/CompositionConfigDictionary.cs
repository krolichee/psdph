using psdPH.TemplateEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Logic
{
    class CompositionConfigDictionary
    {
        static Dictionary<Type, Type> StoC = new Dictionary<Type, Type>
        {{typeof(Blob),typeof(BlobEditorConfig) },
            {typeof(TextLeaf),typeof(TextLeafEditorConfig) },

            {typeof(PlaceholderLeaf),typeof(PlaceholderEditorConfig) },
            {typeof(PrototypeLeaf),typeof(ProtoEditorConfig) }
            //{typeof(PlaceholderLeaf),typeof(PlaceholderLeafEditorConfig) },
        };
        public static Type GetConfigType(Type type)
        {
            return StoC[type];
        }
    }
}
