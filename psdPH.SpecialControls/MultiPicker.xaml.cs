using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace YourNamespace
{
    public partial class MultiPicker : UserControl
    {
        public MultiPicker()
        {
            InitializeComponent();
        }

        // Constructor that takes a list of objects
        public MultiPicker(IEnumerable<object> items) : this()
        {
            foreach (var item in items)
            {
                listbox1.Items.Add(item);
            }
        }

        // Event handler for double-click on listbox2
        private void ListBox2_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (listbox2.SelectedItem != null)
            {
                var selectedItem = listbox2.SelectedItem;
                listbox1.Items.Add(selectedItem);

                // Remove from listbox2 if it's an ObservableCollection you might want to handle differently
                    listbox2.Items.Remove(selectedItem);
            }
        }
        private void ListBox1_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (listbox1.SelectedItem != null)
            {
                var selectedItem = listbox1.SelectedItem;
                listbox2.Items.Add(selectedItem);

                // Remove from listbox2 if it's an ObservableCollection you might want to handle differently
                    listbox1.Items.Remove(selectedItem);
            }
        }

        // Method to get array of items in listbox1
        public object[] GetSelectedItems()
        {
            var items = new object[listbox2.Items.Count];
            listbox2.Items.CopyTo(items, 0);
            return items;
        }

        // DependencyProperty for ItemsSource (optional)
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable<object>), typeof(MultiPicker),
                new PropertyMetadata(null, OnItemsSourceChanged));

        public IEnumerable<object> ItemsSource
        {
            get { return (IEnumerable<object>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as MultiPicker;
            if (control != null)
            {
                control.listbox1.ItemsSource = e.NewValue as IEnumerable<object>;
            }
        }
    }
}
