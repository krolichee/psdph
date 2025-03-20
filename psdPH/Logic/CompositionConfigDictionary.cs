using psdPH.Logic.Compositions;
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
        public static readonly Dictionary<Type, Type> StoC = new Dictionary<Type, Type>
        {
            {typeof(Blob),typeof(BlobEditorCfg) },

            {typeof(FlagLeaf),typeof(FlagLeafEditorCfg) },
            {typeof(PrototypeLeaf),typeof(PrototypeLeafEditorCfg) },
            {typeof(PlaceholderLeaf),typeof(PlaceholderEditorCfg) },

            {typeof(ImageLeaf),typeof(ImageLeafEditorCfg) },
            {typeof(TextLeaf),typeof(TextLeafEditorCfg) },
            {typeof(LayerLeaf),typeof(LayerLeafEditorCfg) },
            {typeof(GroupLeaf),typeof(GroupLeafEditorCfg) },
            {typeof(TextAreaLeaf),typeof(TextAreaLeafEditorCfg) },

        };
        public static Type GetConfigType(Type type)
        {
                return StoC[type];
            
        }
    }
}
