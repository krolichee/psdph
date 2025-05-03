using Photoshop;
using psdPH.Logic;
using psdPH.RuleEditor;
using System;
using System.Collections.Generic;

namespace psdPH.TemplateEditor
{
    public static class RuleDicts
    {
        public delegate IRuleEditor CreateRule(Document doc, Composition composition);
        public delegate IRuleEditor EditRule(Document doc, Rule rule);

        public static Dictionary<Type, CreateRule>
            CreatorDict = new Dictionary<Type, CreateRule>
            (){
        { typeof(Rule),(doc, composition) =>
                 new RuleControlWindow(composition)
        } };

        public static Dictionary<Type, EditRule>
            EditorDict = new Dictionary<Type, EditRule>
            ()
            {
                { typeof(Rule),(doc,rule)=>new RuleControlWindow(rule.Composition) }
            };
    }

}
