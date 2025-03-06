using Photoshop;
using psdPH.Views.WeekView;
using psdPH.Views.WeekView.Logic;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static psdPH.WeekViewWindow;

namespace psdPH
{
    /// <summary>
    /// Логика взаимодействия для WeekGalery.xaml
    /// </summary>
    public partial class WeekViewWindow : Window
    {
        public WeekConfig WeekConfig;
        public WeekListData WeekListData;
        Document doc;
        public WeekViewWindow(Blob root, WeekConfig weekDowsConfig = null, WeekListData weekListData = null)
        {

            var psApp = PhotoshopWrapper.GetPhotoshopApplication();
            doc = PhotoshopWrapper.OpenDocument(psApp, root.Path);

            if (weekDowsConfig == null)
            {
                WeekConfigEditor wce_w = new WeekConfigEditor(root);
                wce_w.ShowDialog();
                weekDowsConfig = wce_w.GetResultConfig();
            }
            if (weekListData == null)
            {

                weekListData = new WeekListData() { RootBlob = root };
            }
            WeekConfig = weekDowsConfig;
            WeekListData = weekListData;
            InitializeComponent();
            refreshWeekStack();
        }
        void renderWeek(WeekData weekData)
        {
            PlaceholderLeaf[] prototypes = weekData.MainBlob.getChildren<PlaceholderLeaf>();
            Dictionary<DayOfWeek, PlaceholderLeaf> dowPlaceholderList = prototypes.ToDictionary(p=>WeekConfig.DowPrototypeLayernameList.First(dp=>dp.Layername==p.LayerName).Dow,p=>p);
            foreach (var dowBlob in weekData.DowBlobList)
            {
                var ph = dowPlaceholderList[dowBlob.Dow];
                ph.ReplaceWithFiller(doc,dowBlob.Blob);
            }
            weekData.MainBlob.apply(doc);
        }

        void refreshWeekStack()
        {
            weeksStack.Children.Clear();
            //var rowStack = new StackPanel();
            //rowStack.Children.Add(new Button() { Command = });
            //weeksStack.Children.Add();
            long unixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            int currentWeek = WeekTime.GetCurrentWeekFromUnixTime(unixTime);
            foreach (var weekData in WeekListData.Weeks)
                if (weekData.Week >= currentWeek)
                    weeksStack.Children.Add(new WeekRow(WeekConfig, weekData));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WeekListData.NewWeek(WeekConfig, WeekListData.RootBlob);
            refreshWeekStack();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            renderWeek(WeekListData.Weeks[0]);
        }
    }
}
