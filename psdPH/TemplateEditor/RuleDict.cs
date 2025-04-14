using Photoshop;
using psdPH.Logic;
using psdPH.Logic.Compositions;
using psdPH.RuleEditor;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static psdPH.TemplateEditor.RuleDict;

namespace psdPH.TemplateEditor
{
    public static class RuleDict
    {
        public delegate IRuleEditor CreateRule(Document doc, Composition composition);
        public delegate IRuleEditor EditRule(Document doc, Rule rule);

        public static Dictionary<Type, CreateRule>
            CreatorDict = new Dictionary<Type, CreateRule>
            (){
        { typeof(Blob),(doc, composition) =>
                 new RuleControl(composition)
        } };
    
        public static Dictionary<Type, EditRule>
            EditorDict = new Dictionary<Type, EditRule>
            ()
            {
                { typeof(Blob),(doc,rule)=>new RuleControl(rule) }
            };
    }

}
