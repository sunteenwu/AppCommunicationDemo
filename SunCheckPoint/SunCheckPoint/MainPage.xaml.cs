using SunCheckPoint;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SunCheckPoint
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        List<Scenario> scenarios = new List<Scenario>
        {     new Scenario() {Title="Scenario 6 -Communication with wpf client ", ClassType=typeof(Scenario6) },
            new Scenario() { Title="Scenario 1 -URI and Protocol Activation", ClassType=typeof(Scenario1)},
            new Scenario() { Title="Scenario 2 - Launch for result", ClassType=typeof(Scenario2)},
            new Scenario() { Title="Scenario 3 - Share Contract", ClassType=typeof(Scenario3)},
             new Scenario() { Title="Scenario 4 - App Services", ClassType=typeof(Scenario4)},
             new Scenario() {Title="Scenario 5 -Public cache share folder ", ClassType=typeof(Scenario5) }


        };
        public static MainPage Current;
        public MainPage()
        {
            Current = this;
            this.InitializeComponent();
        }



        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            ScenarioControl.ItemsSource = scenarios;
            if (Window.Current.Bounds.Width < 640)
            {
                ScenarioControl.SelectedIndex = -1;
            }
            else
            {
                ScenarioControl.SelectedIndex = 0;
            }
        }
        private void ScenarioControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox scenarioListBox = sender as ListBox;
            Scenario s = scenarioListBox.SelectedItem as Scenario;
            if (s != null)
            {
                ScenarioFrame.Navigate(s.ClassType);
                if (Window.Current.Bounds.Width < 640)
                {
                    Splitter.IsPaneOpen = false;
                    StatusBorder.Visibility = Visibility.Collapsed;
                }
            }
        }

        public List<Scenario> Scenarios
        {
            get { return this.scenarios; }
        }


        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            Splitter.IsPaneOpen = (Splitter.IsPaneOpen == true) ? false : true;
            StatusBorder.Visibility = Visibility.Collapsed;
        }

        public void NotifyUser(string strMessage, NotifyType type)
        {
            switch (type)
            {
                case NotifyType.StatusMessage:
                    StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Green);
                    break;
                case NotifyType.ErrorMessage:
                    StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Red);
                    break;
            }
            StatusBlock.Text = strMessage;

            // Collapse the StatusBlock if it has no text to conserve real estate.
            StatusBorder.Visibility = (StatusBlock.Text != String.Empty) ? Visibility.Visible : Visibility.Collapsed;
            if (StatusBlock.Text != String.Empty)
            {
                StatusBorder.Visibility = Visibility.Visible;
                StatusPanel.Visibility = Visibility.Visible;
            }
            else
            {
                StatusBorder.Visibility = Visibility.Collapsed;
                StatusPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnSetOtherpage_Click(object sender, RoutedEventArgs e)
        {
            //Scenario6 page6 = new Scenario6();
            ScenarioFrame.Navigate(typeof(Scenario6),"hello");
           // page6.UpdateTextblock = "hello";
            
        }


    }



    public class ScenarioBindingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Scenario s = value as Scenario;
            return (MainPage.Current.Scenarios.IndexOf(s) + 1) + ") " + s.Title;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return true;
        }
    }
    public class Scenario
    {
        public string Title { get; set; }
        public Type ClassType { get; set; }
    }
    public enum NotifyType
    {
        StatusMessage,
        ErrorMessage
    };
}


