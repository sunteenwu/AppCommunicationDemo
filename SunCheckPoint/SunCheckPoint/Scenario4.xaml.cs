using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.AppService;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using CommonLibrary;
using Windows.System;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SunCheckPoint
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Scenario4 : Page
    {
        public   MainPage rootPage;

        List<Goods> goodslist = new List<Goods>
        {
            new Goods() {Goodsname="Potato",Price=0.99 },
            new Goods() {Goodsname="Gabbage",Price=1.99 },
            new Goods() {Goodsname="Tomato",Price=2.99 },
            new Goods() {Goodsname="Onion",Price=3.99 },
        };
        public Scenario4()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            rootPage = MainPage.Current;
            ListGood.ItemsSource = goodslist;
        }


        private async void OnServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                //Dispose the connection reference we're holding
                if (CashierServiceConnection != null)
                {
                    CashierServiceConnection.Dispose();
                    CashierServiceConnection = null;
                }
            });
        }
        AppServiceConnection CashierServiceConnection = null;
        private async void BtnCheckOut_Click(object sender, RoutedEventArgs e)
        {
            if (this.CashierServiceConnection == null)
            {
                rootPage.NotifyUser("You need to open a connection before trying to excute.", NotifyType.ErrorMessage);
                return;               
            }
            List<Goods> goodselectlist = new List<Goods> { };
            for (int i = 0; i < goodslist.Count; i++)
            {
                if (goodslist[i].IsSelected)
                {
                    goodselectlist.Add(goodslist[i]);
                }

            }
            string items = JSONHelper.JsonSerializer(goodselectlist);
            var message = new ValueSet();            
            message.Add("Items", items);
     
            AppServiceResponse response = await CashierServiceConnection.SendMessageAsync(message);
            if (response.Status == AppServiceResponseStatus.Success)
            {
                if (response.Message.ContainsKey("Result"))
                {
                    ValueSet result = response.Message;
                    TxtAmount.Text =string.Format("Successful paid :{0}", result["Total"].ToString());
                }
                return;
            }
            switch (response.Status)
            {
                case AppServiceResponseStatus.Failure:
                        rootPage.NotifyUser("The service failed to acknowledge the message we sent it. It may have been terminated because the client was suspended.", NotifyType.ErrorMessage);
                        break;

                    case AppServiceResponseStatus.ResourceLimitsExceeded:
                        rootPage.NotifyUser("The service exceeded the resources allocated to it and had to be terminated.", NotifyType.ErrorMessage);
                        break;

                    case AppServiceResponseStatus.Unknown:
                    default:
                        rootPage.NotifyUser("An unkown error occurred while we were trying to send a message to the service.", NotifyType.ErrorMessage);
                        break;
            }//  CashierServiceConnection.ServiceClosed += OnServiceClosed;

        }  
        private void BtnCloseConnection_Click(object sender, RoutedEventArgs e)
        {
            //Is there an open connection?
            if (CashierServiceConnection == null)
            {
                rootPage.NotifyUser("There's no open connection to close", NotifyType.ErrorMessage);
                return;
            }

            //Close the open connection
            CashierServiceConnection.Dispose();
            CashierServiceConnection = null;

            //Let the user know we closed the connection
            rootPage.NotifyUser("Connection is closed", NotifyType.StatusMessage);
        }

        private async void BtnOpenConnection_Click(object sender, RoutedEventArgs e)
        {

            if (this.CashierServiceConnection == null)
            {
                CashierServiceConnection = new AppServiceConnection();
                CashierServiceConnection.AppServiceName = "SunCashierService";
                CashierServiceConnection.PackageFamilyName = "62762cd1-1887-405e-93cc-30d1754f1737_75cr2b68sm664";
                AppServiceConnectionStatus connectionStatus = await CashierServiceConnection.OpenAsync();
                if (connectionStatus == AppServiceConnectionStatus.Success)
                {
                    rootPage.NotifyUser("Connection is open", NotifyType.StatusMessage);
                    return;
                }

                switch (connectionStatus)
                {
                    case AppServiceConnectionStatus.AppNotInstalled:
                        rootPage.NotifyUser("The app AppServicesProvider is not installed. Deploy AppServicesProvider to this device and try again.", NotifyType.ErrorMessage);
                        break;

                    case AppServiceConnectionStatus.AppUnavailable:
                        rootPage.NotifyUser("The app AppServicesProvider is not available. This could be because it is currently being updated or was installed to a removable device that is no longer available.", NotifyType.ErrorMessage);
                        break;

                    case AppServiceConnectionStatus.AppServiceUnavailable:
                        rootPage.NotifyUser(string.Format("The app AppServicesProvider is installed but it does not provide the app service {0}.", CashierServiceConnection.AppServiceName), NotifyType.ErrorMessage);
                        break;

                    case AppServiceConnectionStatus.Unknown:
                        rootPage.NotifyUser("An unkown error occurred while we were trying to open an AppServiceConnection.", NotifyType.ErrorMessage);
                        break;
                }

            }
            else
            {
                rootPage.NotifyUser("A connection already exists", NotifyType.ErrorMessage);
                return;
            }
        }

        private async void BtnLaunchSecond_Click(object sender, RoutedEventArgs e)
        {
            var testAppUri = new Uri("sun-targetapp:");
            var options = new LauncherOptions();
            options.TargetApplicationPackageFamilyName = "cfb33137-5baf-411d-9b3d-a518fd0c4f9b_75cr2b68sm664";
            await Launcher.LaunchUriAsync(testAppUri, options);
        }
    }
}
