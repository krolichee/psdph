using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace psdPH.Utils.CedStack
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
