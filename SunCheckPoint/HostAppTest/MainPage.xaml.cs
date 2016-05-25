using CommonLibrary;
using System;
using System.Collections.Generic;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HostAppTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        List<Goods> goodslist = new List<Goods>
        {
            new Goods() {Goodsname="Potato",Price=0.99 },
            new Goods() {Goodsname="Gabbage",Price=1.99 },
            new Goods() {Goodsname="Tomato",Price=2.99 },
            new Goods() {Goodsname="Onion",Price=3.99 },
        };
        public MainPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ListGood.ItemsSource = goodslist;
        }
        private async void BtnLaunchResult_Click(object sender, RoutedEventArgs e)
        {

            var testAppUri = new Uri("sun-targetapp:"); // The protocol handled by the launched app
            var options = new LauncherOptions();
            options.TargetApplicationPackageFamilyName = "8dd39492-3e3d-45cc-b67f-0f00fd3bbc99_75cr2b68sm664";
            List<Goods> goodselectlist = new List<Goods> { };
            for (int i = 0; i < goodslist.Count; i++)
            {
                if (goodslist[i].IsSelected)
                {
                    goodselectlist.Add(goodslist[i]);
                }

            }
            string items = JSONHelper.JsonSerializer(goodselectlist);
            var inputData = new ValueSet();
            inputData["Items"] = items;

            LaunchUriResult result = await Windows.System.Launcher.LaunchUriForResultsAsync(testAppUri, options, inputData);
            if (result.Status == LaunchUriStatus.Success &&
                result.Result != null &&
                result.Result.ContainsKey("ReturnData"))
            {
                ValueSet theValues = result.Result;
                TxtAmount.Text = string.Format("Total{0}，already paid successful", theValues["ReturnData"] as string);
                return;
            }
      
        }
    }
}
