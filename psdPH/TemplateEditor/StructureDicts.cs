using Photoshop;
using psdPH.Logic.Compositions;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using System;
using System.Collections.Generic;


namespace psdPH.TemplateEditor
{
    public static class StructureDicts
    {
        public delegate IBatchCompositionCreator CreateComposition(Document doc, Composition root);
        public delegate IBatchCompositionCreator EditComposition(Document doc, Composition composition);

        public static Dictionary<Type, CreateComposition>
            CreatorDict = new Dictionary<Type, CreateComposition>
            (){
        { typeof(Blob),(doc, root) =>new BlobCreator(doc,root as Blob)},
        { typeof(FlagLeaf), (doc, root) =>new FlagLeafCreator()},
        { typeof(PrototypeLeaf),(doc, root) =>new PrototypeCreator(doc, root) },
        { typeof(PlaceholderLeaf), (doc, root) =>new MultiPlaceholderLeafCreator(doc, root) },
        //{ typeof(ImageLeaf),(doc, root) => new ImageLeafCreator(doc) },
        { typeof(TextLeaf),(doc, root) => new MultiTextLeafCreator(doc,root)},
        { typeof(LayerLeaf),(doc, root) => new MultiLayerLeafCreator(doc,root)},
        { typeof(GroupLeaf),(doc, root) => new MultiGroupLeafCreator(doc,root)},
        { typeof(AreaLeaf),(doc, root) => new MultiAreaLeafCreator(doc, root)} };
        public static Dictionary<Type, EditComposition>
            EditorDict = new Dictionary<Type, EditComposition>
            ()
            {
                { typeof(Blob),(doc,composition)=>BlobEditorWindow.OpenInDocument(doc,composition as Blob) }
            };
    }




}
