using System.Windows;
using System.Windows.Input;

namespace psdPH.Utils.CedStack
{
    public interface IStackPanelManipulation
    {
        event StackPanelManipulation.SwapHandler Swapped;

        UIElement NewElement(FrameworkElement element);
        int CalculateNewIndex(double yPosition);
        void ClearBorders();
        void mlbd(object sender, MouseButtonEventArgs e);
        void mlbu(object sender, MouseButtonEventArgs e);
        void mm(object sender, MouseEventArgs e);
        void UpdateInsertionIndicator(double yPosition);
    }
}