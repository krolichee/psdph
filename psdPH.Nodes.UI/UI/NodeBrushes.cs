using psdPH.Nodes.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace psdPH.Nodes.UI.UI
{
    public static class NodeBrushes
    {
        static Dictionary<Type, Brush> brushes = new Dictionary<Type, Brush>()
        {
            {typeof(ObjectNode),Brushes.Aquamarine},
            {typeof(Node),Brushes.Gray},
            {typeof(RuleNode),Brushes.DarkOrange},
            {typeof(ConditionNode),Brushes.YellowGreen},
            {typeof(MuxNode),Brushes.YellowGreen},

        };
        public static Brush GetBrush(Type type)
        {
            if (!typeof(Node).IsAssignableFrom(type))
                throw new ArgumentException();
            if (brushes.TryGetValue(type, out var brush))
                return brush;
            else
                return GetBrush(type.BaseType);
        }
    }
}
