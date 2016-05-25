using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using CommonLibrary;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SunCheckPoint
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Scenario2 : Page
    {
        public MainPage rootPage;
        List<Goods> goodslist = new List<Goods>
        {
            new Goods() {Goodsname="Potato",Price=0.99 },
            new Goods() {Goodsname="Gabbage",Price=1.99 },
            new Goods() {Goodsname="Tomato",Price=2.99 },
            new Goods() {Goodsname="Onion",Price=3.99 },
        };
        public Scenario2()
        {
            rootPage = MainPage.Current;
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

            #region  valueset exceed
            //string content;
            //var Folder1 = Windows.Storage.ApplicationData.Current.LocalFolder;
            //var shareFile = await Folder1.GetFileAsync("App Communication(Sunteen Checkpoint Preview).pptx");
            //using (Stream stream = await shareFile.OpenStreamForReadAsync())
            //{
            //    StreamReader reader = new StreamReader(stream);
            //    content = reader.ReadToEnd();
            //    System.Text.Encoding.UTF8.GetBytes(content);
            //}
            //inputData.Add("test", content);
            #endregion

            LaunchUriResult result =await Windows.System.Launcher.LaunchUriForResultsAsync(testAppUri, options, inputData);
            #region
            if (result.Status == LaunchUriStatus.Success &&
                result.Result != null &&
                result.Result.ContainsKey("ReturnData"))
            {
                ValueSet theValues = result.Result;
                TxtAmount.Text = string.Format("Total{0}，already paid successful", theValues["ReturnData"] as string);
                return;
            }
            switch (result.Status)
            {
                case LaunchUriStatus.AppUnavailable:
                    rootPage.NotifyUser("The application cannot be activated which may be because it is being updated by the store, it was installed on a removable device that is not available, and so on", NotifyType.ErrorMessage);
                    break;
                case LaunchUriStatus.ProtocolUnavailable:
                    rootPage.NotifyUser("The application you are trying to activate does not support this URI", NotifyType.ErrorMessage);
                    break;
                case LaunchUriStatus.Unknown:
                default:
                    rootPage.NotifyUser("User canceled or Unknown error", NotifyType.ErrorMessage);
                    break;

            }
            #endregion


        }
    }
}
