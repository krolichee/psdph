using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace psdPH.Utils.CedStack
{
    public abstract class PanelManipulation : IStackPanelManipulation
    {
        public delegate void SwapHandler(int originalIndex, int newIndex);
        public abstract event SwapHandler Swapped;
        public abstract UIElement NewElement(FrameworkElement element);
        public abstract int CalculateNewIndex(double yPosition);
        public abstract void ClearBorders();
        public abstract void mlbd(object sender, MouseButtonEventArgs e);
        public abstract void mlbu(object sender, MouseButtonEventArgs e);
        public abstract void mm(object sender, MouseEventArgs e);
        public abstract void UpdateInsertionIndicator(double yPosition);
    }
}
