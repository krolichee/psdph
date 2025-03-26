using Photoshop;
using psdPH.Logic.Compositions;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using PsApp = Photoshop.Application;


namespace psdPH.TemplateEditor
{
    public static class StructureDicts
    {
        public delegate ICompositionShapitor CreateComposition(Document doc, Composition root);
        public delegate ICompositionShapitor EditComposition(Document doc, Composition composition);

        public static Dictionary<Type, CreateComposition>
            CreatorDict = new Dictionary<Type, CreateComposition>
            (){
        { typeof(Blob),(doc, root) =>
                 new BlobCreator(doc,root as Blob)
        },

        { typeof(FlagLeaf), (doc, root) =>
                new FlagLeafCreator()
        },
        { typeof(PrototypeLeaf),(doc, root) =>
            new PrototypeCreator(doc, root) },
        { typeof(PlaceholderLeaf), (doc, root) =>
           new PlaceholderLeafCreator(doc, root) },
        { typeof(ImageLeaf),(doc, root) =>
           new ImageLeafCreator(doc) },
        { typeof(TextLeaf),(doc, root) =>
            new TextLeafCreator(doc)},
        { typeof(LayerLeaf),(doc, root) =>
            new LayerLeafCreator(doc)},
        { typeof(GroupLeaf),(doc, root) =>
            new GroupLeafCreator(doc)},
        { typeof(TextAreaLeaf),(doc, root) =>
            new TextAreaLeafCreator(doc, root as Blob)}
    };
        public static Dictionary<Type, EditComposition>
            EditorDict = new Dictionary<Type, EditComposition>
            ()
            {
                { typeof(Blob),(doc,composition)=>BlobEditorWindow.OpenInDocument(doc,composition as Blob) }
            };
    }




}
