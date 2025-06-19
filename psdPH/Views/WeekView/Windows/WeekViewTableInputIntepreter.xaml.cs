using psdPH.Logic;
using psdPH.Logic.Parameters;
using psdPH.Utils.Setups;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace psdPH.Views.WeekView.Windows
{
    /// <summary>
    /// Логика взаимодействия для WeekViewTableInputIntepreter.xaml
    /// </summary>
    public partial class WeekViewTableInputIntepreter : Window
    {
        List<Setup> _setups = new List<Setup>();
        WeekData WeekData;
        void setRowColumn(UIElement element,int i, int j)
        {
            Grid.SetRow(element, i);
            Grid.SetColumn(element, j);
        }
        void insertParameterPair(Parameter par, int i, int j)
        {
            var setup = par.Setups[0];
            _setups.Add(setup);
            var nameLabel = new TextBlock() { Text = par.Name };
            var setupControl = setup.Control;
            setRowColumn(nameLabel, i, j);
            nameLabel.Margin = new Thickness(10, 4, 2, 4);
            mainGrid.Children.Add(nameLabel);
            setRowColumn(setupControl, i, j + 1);

            if (setup.Stack != setupControl.Parent)
                throw new Exception();
            setup.Stack.Children.Remove(setupControl);
            mainGrid.Children.Add(setupControl);
        }
        public WeekViewTableInputIntepreter(WeekData weekData)
        {
            InitializeComponent();
            SizeToContent = SizeToContent.WidthAndHeight;
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            int weekPars = 0;
            for (int i = 0; i < weekData.ParameterSet.Parameters.Count; i++)
            {
                mainGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                psdPH.Logic.Parameters.Parameter par = weekData.ParameterSet.Parameters[i];
                insertParameterPair(par,i,0);
                weekPars++;
            }
            
            mainGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            for (int i = 0; i < weekData.DayParsetsList[0].Parameters.Count; i++)
                mainGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            

            for (int parset_i = 0; parset_i < 7; parset_i++)
            {
                var dayParameterSet = weekData.DayParsetsList[parset_i];
                var dow = Localization.Localize(dayParameterSet.Dow);

                var dowLabel = new TextBlock() { Text = dow };
                setRowColumn(dowLabel, weekPars, parset_i * 2 + 2);
                mainGrid.Children.Add(dowLabel);
                dowLabel.Margin = new Thickness(10, 4, 2, 4);


                mainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                mainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                for (int par_i = 0; par_i < dayParameterSet.Parameters.Count; par_i++)
                {
                    
                    psdPH.Logic.Parameters.Parameter par = dayParameterSet.Parameters[par_i];
                    insertParameterPair(par, par_i + weekPars+1, parset_i * 2 + 2);
                }

            }
            
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            foreach (var setup in _setups)
                setup.Accept();
            Close();
        }

    }
}
