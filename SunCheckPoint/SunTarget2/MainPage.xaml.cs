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
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Activation;
using CommonLibrary;
using Windows.ApplicationModel.AppService;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SunTarget2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private Windows.System.ProtocolForResultsOperation _operation = null;
        AppServiceConnection CashierServiceConnection = null;
        public MainPage()
        {
            this.InitializeComponent();
            //this.ViewModel = new GoodsViewModel(selectedgoods);
        }

        private async void BtnConnectAppService_Click(object sender, RoutedEventArgs e)
        {
            if (this.CashierServiceConnection == null)
            {
                CashierServiceConnection = new AppServiceConnection();
                CashierServiceConnection.AppServiceName = "SunCashierService";
                CashierServiceConnection.PackageFamilyName = "62762cd1-1887-405e-93cc-30d1754f1737_75cr2b68sm664";
                AppServiceConnectionStatus connectionStatus = await CashierServiceConnection.OpenAsync();
                if (connectionStatus == AppServiceConnectionStatus.Success)
                {
                    txtresult.Text = "Connection is open";
                }

            }
        }

    }
}
