using psdPH.Logic;
using psdPH.Logic.Ruleset.Rules;
using psdPH.RuleEditor;
using System;
using System.Collections.Generic;
using static psdPH.TemplateEditor.StructureRulesetDefinition;

namespace psdPH.TemplateEditor
{
    public class StructureRulesDicts
    {
        public static Dictionary<Type, CreateRule>
            CreatorDict = new Dictionary<Type, CreateRule>
            (){
        {
              typeof(Rule),(doc, composition) =>
                 new RuleEditorWindow(new StructureRulesetDefinition(composition))
        } };

        public static Dictionary<Type, EditRule>
            EditorDict = new Dictionary<Type, EditRule>
            ()
            {
                //{ typeof(Rule),(doc,rule)=>new RuleControlWindow(rule.Composition) }
            };
    }

}
