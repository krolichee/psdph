using System;
using System.Windows;
using System.Windows.Controls;

namespace psdPH.Utils.CedStack
{
    abstract public class CEDStackHandler
    {

        public StackPanel Stack;
        protected abstract UIElement createControl(object item);
        protected abstract object[] getElements();
        public void Refresh()
        {
            Stack.Children.Clear();
            object[] elements = getElements();
            foreach (object item in elements)
                Stack.Children.Add(createControl(item));
        }
        protected virtual void InitializeAddDropDownMenu(Button button) { }
        protected virtual void AddButtonAction() { }
        protected void AddButton_Click(object _, object __) { AddButtonAction(); }
        protected virtual MenuItem CreateAddMenuItem(Type type) { throw new NotImplementedException(); }
        public void Initialize(CEDStackUI cEDStackUI)
        {
            InitializeAddDropDownMenu(cEDStackUI.AddButton);
            cEDStackUI.AddButton.Click += AddButton_Click;
            Stack = cEDStackUI.StackPanel;
        }

    }
}
