
using psdPH.Compositions;
using psdPH.Logic.Compositions;
using psdPH.Photoshop;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using System;
using System.Collections.Generic;


namespace psdPH.TemplateEditor.Structure
{
    public static class StructureDicts
    {
        public delegate IBatchCompositionCreator CreateComposition(DocumentWr doc, Composition root);
        public delegate IBatchCompositionCreator EditComposition(DocumentWr doc, Composition composition);

        public static Dictionary<Type, CreateComposition>
            CreatorDict = new Dictionary<Type, CreateComposition>
            (){
        { typeof(RootBlob),(doc, root) =>new LayerBlobCreator(doc)},
        { typeof(PrototypeBlob),(doc, root) =>new PrototypeBlobCreator(doc) },
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
                //{ typeof(RootBlob),(doc,composition)=>TemplateEditorWindow.OpenInDocument(doc,composition as LayerBlob) }
            };
    }




}
