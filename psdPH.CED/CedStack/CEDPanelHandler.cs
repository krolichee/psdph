using psdPH.Utils.CedStack;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace psdPH.CED
{
    abstract public class CEDPanelHandler
    {
        public Panel Panel;
        PanelManipulation PanelManipulation;
        protected abstract IList Items { get; }
        protected virtual void move(int from, int to)
        {
            var arr = Items;
            var from_obj = arr[from];
            arr.RemoveAt(from);
            arr.Insert(to, from_obj);
            Refresh();
        }
        protected abstract FrameworkElement createControl(object item);
        protected abstract object[] getElements();
        public void Refresh()
        {
            Panel.Children.Clear();
            object[] elements = getElements();
            foreach (object item in elements)
                Panel.Children.Add(a(createControl(item)));
        }
        protected virtual void InitializeAddDropDownMenu(Button button) { }
        protected virtual void AddButtonAction() { }
        protected void AddButton_Click(object _, object __) { AddButtonAction(); }
        protected virtual MenuItem CreateAddMenuItem(Type type) { throw new NotImplementedException(); }
        public void Initialize(CEDStackUI cEDStackUI)
        {
            InitializeAddDropDownMenu(cEDStackUI.AddButton);
            cEDStackUI.AddButton.Click += AddButton_Click;
            Panel = cEDStackUI.Panel;
            PanelManipulation = new StackPanelManipulation(Panel);
            PanelManipulation.Swapped += move;
        }
        public UIElement a(FrameworkElement element)
        {
            return PanelManipulation.NewElement(element);
        }

        public CEDPanelHandler()
        {
            new StackPanelManipulation(Panel);
        }





    }
}
