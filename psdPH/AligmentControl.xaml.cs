using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static psdPH.Logic.PhotoshopDocumentExtension;

namespace psdPH
{
    /// <summary>
    /// Логика взаимодействия для AligmentControl.xaml
    /// </summary>
    public partial class AligmentControl : UserControl
    {
        Dictionary<Button, Alignment> btnAli=> new Dictionary<Button, Alignment>()
            {
                { upLeft,Alignment.Create("up","left")},
                { upCenter,Alignment.Create("up","center")},
                {  upRight,Alignment.Create("up","right")},
                {  centerLeft,Alignment.Create("center","left")},
                {  centerCenter,Alignment.Create("center","center")},
                {  centerRight,Alignment.Create("center","right")},
                {  downLeft,Alignment.Create("down","left")},
                {  downCenter,Alignment.Create("down","center")},
                {  downRight,Alignment.Create("down","right")}
            };
        Size _size = new Size(90, 90);
        Alignment _result=new Alignment(HorizontalAlignment.Center,VerticalAlignment.Center);
        public AligmentControl(Alignment alignment):this()
        {
            setAligment(alignment);
        }
        public AligmentControl(int size) : this()
        {
            Height = Width = size;
        }
        public AligmentControl()
        {
            InitializeComponent();
            foreach (Button button in btnAli.Keys)
            {
                button.Command = new RelayCommand(setAligment);
                button.CommandParameter = btnAli[button];
                button.ToolTip = btnAli[button].ToLocalizedString();
                ToolTipService.SetInitialShowDelay(button, 100);    // Задержка перед показом (0.5 сек)
                ToolTipService.SetBetweenShowDelay(button, 500);   // Время для мгновенного показа следующей подсказки (2 сек)
                ToolTipService.SetShowDuration(button, 1000);
            }
            var mainGrid = new Grid();
            setAligment(_result);
        }
        void clearColors()
        {
            foreach (var item in btnAli.Keys)
                item.SetResourceReference(Control.BackgroundProperty, SystemColors.ControlLightBrushKey);
        }
        void setAligment(object alignment_obj)
        {
            Alignment alignment = alignment_obj as Alignment;
            _result = alignment;
            clearColors();
            foreach (var item in btnAli)
                if (item.Value.Equals(alignment))
                    item.Key.SetResourceReference(Control.BackgroundProperty, SystemColors.ActiveCaptionBrushKey);
        }
        public Alignment GetResultAlignment()
        {
            return _result;
        }
    }
}
