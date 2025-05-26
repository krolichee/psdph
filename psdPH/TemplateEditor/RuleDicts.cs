using Photoshop;
using psdPH.Logic;
using psdPH.Logic.Rules;
using psdPH.RuleEditor;
using System;
using System.Collections.Generic;
using System.Windows;
using static psdPH.WeekConfig;
using Condition = psdPH.Logic.Rules.Condition;

namespace psdPH.TemplateEditor
{
    public static class RuleDicts
    {
        public static Condition[] Conditions(Composition root) => new Condition[]
            {
                new FlagCondition(root)
            };
        public static Rule[] Rules(Composition root) => new ConditionRule[]
            {
                new TextFontSizeRule(root),
                new TextAnchorRule(root),
                new TranslateRule(root),
                new OpacityRule(root),
                new VisibleRule(root),
                new AlignRule(root),
                new FitRule(root),
            };
        public delegate IRuleEditor CreateRule(Document doc, Composition composition);
        public delegate IRuleEditor EditRule(Document doc, Rule rule);

        public static Dictionary<Type, CreateRule>
            CreatorDict = new Dictionary<Type, CreateRule>
            (){
        {
              typeof(Rule),(doc, composition) =>
                 new RuleControlWindow(Rules(composition),Conditions(composition))
        } };

        public static Dictionary<Type, EditRule>
            EditorDict = new Dictionary<Type, EditRule>
            ()
            {
                //{ typeof(Rule),(doc,rule)=>new RuleControlWindow(rule.Composition) }
            };
    }

}
