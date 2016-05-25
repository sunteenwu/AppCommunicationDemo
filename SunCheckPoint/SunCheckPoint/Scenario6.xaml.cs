using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SunCheckPoint
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Scenario6 : Page
    {
        private string currentText;

        public Scenario6()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if(e.Parameter!=null)
            { 
            UpdateTextblock = e.Parameter.ToString();
            }
        }

        private async void BtnLaunchwpf_Click(object sender, RoutedEventArgs e)
        {
            var launchUri = new Uri(UriToLaunch.Text);
            await Launcher.LaunchUriAsync(launchUri);

        }
       
        public string UpdateTextblock
        {
            get
            {
                return currentText;
            }

            set
            {
                currentText = value;
                Debug.WriteLine("new Text = " + currentText);
                // Shows "Hello World" fine

                updateText();
            }
        }


        private void updateText()
        {
            myTextblock.Text = currentText.ToString();
            // DOesn't seems to update
        }

        private void Btntextcus_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
