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
    abstract public class CEDStackHandler
    {
        public StackPanel Stack;
        protected abstract IList Items { get; }
        protected virtual void move(int from, int to)
        {
            var arr = Items;
            var from_obj = arr[from];
            arr.RemoveAt(from);
            arr.Insert(to,from_obj);
        }
        //protected virtual void swap(int index1, int index2)
        //{
        //    var e = Items;
        //    var item1 = e[index1];
        //    e[index1] = e[index2];
        //    e[index2] = item1;
        //}
        protected abstract FrameworkElement createControl(object item);
        protected abstract object[] getElements();
        public void Refresh()
        {
            Stack.Children.Clear();
            object[] elements = getElements();
            foreach (object item in elements)
                Stack.Children.Add(a(createControl(item)));
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
        public UIElement a(FrameworkElement element)
        {
            var wrapper = new Grid()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch, // Растягиваем Grid на всю доступную ширину
                ColumnDefinitions =
    {
        new ColumnDefinition { Width = new GridLength(20) }, // Фиксированная ширина слева
        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) } // Оставшееся место для вашего элемента
    }
            };
            var border = new Border
            {
                //Background = Brushes.White,
                BorderBrush = Brushes.Black,
                //BorderThickness = new Thickness(1),
                // Margin = new Thickness(5),
                //Width = 200,
                //Height = 50,
                Child = wrapper,
                RenderTransform = new TranslateTransform(),
                HorizontalAlignment = HorizontalAlignment.Stretch,

            };

            // Создаем левый элемент (например, Border для демонстрации)
            var leftElement = new DragRect
            {
                Dragged = border,
                Background = Brushes.LightGray,
                Width = 20, // Фиксированная ширина
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Stretch,
                Height = element.Height
            };
            // Ваш пользовательский элемент (который должен растягиваться)
            var yourUserControl = element; // Замените на ваш реальный элемент
           yourUserControl.HorizontalAlignment = HorizontalAlignment.Stretch;

            // Добавляем элементы в Grid
            Grid.SetColumn(leftElement, 0);
            Grid.SetColumn(yourUserControl, 1);

            wrapper.Children.Add(leftElement);
            wrapper.Children.Add(yourUserControl);
            
            leftElement.AddHandler(UIElement.MouseEnterEvent, new MouseEventHandler(dragElementEnter), true);
            //            leftElement.AddHandler(UIElement.MouseLeaveEvent, new MouseEventHandler(dragElementLeave), true);

            mlbd_h = mlbd;
            mlbu_h = mlbu; 
            mm_h = mm; 

            return border;
        }
        private MouseButtonEventHandler mlbd_h;
        private MouseButtonEventHandler mlbu_h;
        private MouseEventHandler mm_h ;
        private void removeBorderHandlers(FrameworkElement border)
        {
            border.MouseLeftButtonDown -= mlbd_h;
            border.MouseLeftButtonUp -= mlbu_h;
            border.MouseMove -= mm_h;
        }
        private void dragElementEnter(object sender, MouseEventArgs e)
        {
            var border = (sender as DragRect).Dragged;
            border.MouseLeftButtonDown += mlbd_h;
            border.MouseLeftButtonUp += mlbu_h;
            border.MouseMove += mm_h;
        }

        private UIElement draggedItem;
        private Point startPoint;
        private int originalIndex;
        private TranslateTransform transform;
        private double offsetY;
        private bool dragged = false;
        private StackPanel itemsContainer=>Stack;
        private void mlbu(object sender, MouseButtonEventArgs e)
        {
            
            var border = (sender as Border).Child as FrameworkElement;
            if(!dragged)
                draggedItem?.ReleaseMouseCapture();

            if (draggedItem == null || e.ChangedButton != MouseButton.Left) return;


            // Сбрасываем визуальные эффекты
            draggedItem.Opacity = 1;
            draggedItem.Effect = null;
            Panel.SetZIndex(draggedItem, 0);

            // Определяем новую позицию
            Point endPoint = e.GetPosition(itemsContainer);
            int newIndex = CalculateNewIndex(endPoint.Y);

            if (newIndex != originalIndex && newIndex != -1)
            {
                // Анимация перемещения
                var animation = new DoubleAnimation(0, TimeSpan.FromMilliseconds(200));
                transform.BeginAnimation(TranslateTransform.YProperty, animation);
                // Фактическое перемещение элемента в коллекции
                move(originalIndex, newIndex);
                //itemsContainer.Children.Remove(draggedItem);
                //itemsContainer.Children.Insert(newIndex, draggedItem);
                //doRefresh = true;
                Refresh();
            }
            else
            {
                // Анимация возврата на место
                var animation = new DoubleAnimation(0, TimeSpan.FromMilliseconds(200));
                transform.BeginAnimation(TranslateTransform.YProperty, animation);
            }


            draggedItem.ReleaseMouseCapture();
            draggedItem.RenderTransform = null;
            Console.WriteLine("отпущено");
            draggedItem = null;
            ClearBorders();
            e.Handled = false;
            dragged = false;
            removeBorderHandlers(border);
        }
        private void mm(object sender, MouseEventArgs e)
        {
            if (draggedItem == null || !draggedItem.IsMouseCaptured) return;
            Console.WriteLine("перетаскивание");
            
            // Текущая позиция курсора относительно StackPanel
            Point currentPoint = e.GetPosition(itemsContainer);

            // Вычисляем смещение
            double deltaY = currentPoint.Y - startPoint.Y+offsetY;

            // Применяем трансформацию
            transform.Y = deltaY;
            if (deltaY!=0) 
                dragged = true;
            // (sender as FrameworkElement).Margin = new Thickness(0, deltaY, 0, 0);

            // Обновляем индикатор позиции вставки
            UpdateInsertionIndicator(currentPoint.Y);
        }
        private void mlbd(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;

            draggedItem = sender as FrameworkElement;
            Console.WriteLine("захвачено");
            startPoint = e.GetPosition(null);
            originalIndex = itemsContainer.Children.IndexOf(draggedItem);

            // Получаем начальную позицию элемента относительно StackPanel
            startPoint = draggedItem.TransformToAncestor(itemsContainer)
                .Transform(new Point(0, 0));
            offsetY = startPoint.Y - e.GetPosition((FrameworkElement)itemsContainer).Y;
            // Инициализируем трансформацию
            transform = draggedItem.RenderTransform as TranslateTransform ?? new TranslateTransform();
            draggedItem.RenderTransform = transform;

            // Поднимаем элемент над другими
            Panel.SetZIndex(draggedItem, 100);

            // Визуальные эффекты
            draggedItem.Opacity = 0.8;
            draggedItem.Effect = new DropShadowEffect
            {
                Color = Colors.Black,
                ShadowDepth = 5,
                Opacity = 0.5
            };

            // Захватываем мышь
            draggedItem.CaptureMouse();
            e.Handled = false;
            dragged = false;
        }
        private int FindNewIndex(double yPosition)
        {
            for (int i = 0; i < itemsContainer.Children.Count; i++)
            {
                if (itemsContainer.Children[i] == draggedItem) continue;

                UIElement child = itemsContainer.Children[i];
                Point childPosition = child.TransformToAncestor(itemsContainer).Transform(new Point(0, 0));
                double childTop = childPosition.Y;
                double childBottom = childTop + child.RenderSize.Height;

                // Если курсор находится в верхней половине элемента
                if (yPosition < childTop + (childBottom - childTop) / 2)
                {
                    return i > originalIndex ? i - 1 : i;
                }
            }

            // Если курсор ниже всех элементов - добавляем в конец
            return itemsContainer.Children.Count - 1;
        }
        private int CalculateNewIndex(double yPosition)
        {
            for (int i = 0; i < itemsContainer.Children.Count; i++)
            {
                if (itemsContainer.Children[i] == draggedItem) continue;

                var child = itemsContainer.Children[i] as FrameworkElement;
                if (child == null) continue;

                Point childPos = child.TransformToAncestor(itemsContainer).Transform(new Point(0, 0));
                double childCenter = childPos.Y + child.ActualHeight / 2;

                if (yPosition < childCenter)
                {
                    return i > originalIndex ? i - 1 : i;
                }
            }

            return itemsContainer.Children.Count - 1;
        }
        private void ClearBorders() {
            foreach (UIElement child in itemsContainer.Children)
            {
                if (child is Border border)
                {
                    //border.BorderBrush = Brushes.Black;
                    border.BorderThickness = new Thickness(0);
                }
            }
        }
        private void UpdateInsertionIndicator(double yPosition)
        {
            // Сбрасываем подсветку для всех элементов
            foreach (UIElement child in itemsContainer.Children)
            {
                if (child is Border border)
                {
                    //border.BorderBrush = Brushes.Black;
                    border.BorderThickness = new Thickness(0);
                }
            }
            if (draggedItem == null) return;

            // Находим потенциальную позицию вставки
            for (int i = 0; i < itemsContainer.Children.Count; i++)
            {
                if (itemsContainer.Children[i] == draggedItem) continue;

                UIElement child = itemsContainer.Children[i];
                Point childPosition = child.TransformToAncestor(itemsContainer).Transform(new Point(0, 0));
                double childTop = childPosition.Y;
                double childBottom = childTop + child.RenderSize.Height;

                // Подсвечиваем границу элемента, если курсор рядом
                if (yPosition >= childTop - 10 && yPosition <= childBottom + 10)
                {
                    if (child is Border border)
                    {
                        border.BorderBrush = Brushes.AliceBlue;
                        border.BorderThickness = new Thickness(2);
                    }
                    break;
                }
            }
        }

    }
}
