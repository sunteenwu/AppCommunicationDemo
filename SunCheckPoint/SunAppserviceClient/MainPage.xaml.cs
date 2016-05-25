using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.AppService;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SunAppserviceClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        AppServiceConnection CashierServiceConnection = null;
        private async void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            //if (this.CashierServiceConnection == null)
            //{
            #region Appunavailable testing
            //for(int i=0;i<8; i++)
            //{
            //    CashierServiceConnection = new AppServiceConnection();
            //    CashierServiceConnection.AppServiceName = "SunCashierService";
            //    CashierServiceConnection.PackageFamilyName = "62762cd1-1887-405e-93cc-30d1754f1737_75cr2b68sm664";
            //    CashierServiceConnection.ServiceClosed += Conn_ServiceClosed;
            //    AppServiceConnectionStatus connectionStatus2 = await CashierServiceConnection.OpenAsync();
            //    System.Diagnostics.Debug.WriteLine(connectionStatus2.ToString());
            //}
            #endregion

            CashierServiceConnection = new AppServiceConnection();
            CashierServiceConnection.AppServiceName = "SunCashierSecondService";
            CashierServiceConnection.PackageFamilyName = "62762cd1-1887-405e-93cc-30d1754f1737_75cr2b68sm664";
            CashierServiceConnection.ServiceClosed += Conn_ServiceClosed;
            AppServiceConnectionStatus connectionStatus = await CashierServiceConnection.OpenAsync();
            if (connectionStatus == AppServiceConnectionStatus.Success)
            {
                await new MessageDialog("Connection is open.").ShowAsync();
                return;
            }

            switch (connectionStatus)
            {
                case AppServiceConnectionStatus.AppNotInstalled:
                    await new MessageDialog("The app AppServicesProvider is not installed. Deploy AppServicesProvider to this device and try again.").ShowAsync();
                    break;

                case AppServiceConnectionStatus.AppUnavailable:
                    await new MessageDialog("The app AppServicesProvider is not available. This could be because it is currently being updated or was installed to a removable device that is no longer available.").ShowAsync();
                    break;

                case AppServiceConnectionStatus.AppServiceUnavailable:
                    await new MessageDialog(string.Format("The app AppServicesProvider is installed but it does not provide the app service {0}.", CashierServiceConnection.AppServiceName)).ShowAsync();
                    break;

                case AppServiceConnectionStatus.Unknown:
                    await new MessageDialog("An unkown error occurred while we were trying to open an AppServiceConnection.").ShowAsync();

                    break;
            }

            //}
            //else
            //{
            //    await new MessageDialog("A connection is already open").ShowAsync();
            //}

        }
        private async void Conn_ServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
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
        private async void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //Is there an open connection?
            if (CashierServiceConnection == null)
            {
                await new MessageDialog("There's no open connection to close").ShowAsync();
                return;
            }

            //Close the open connection
            CashierServiceConnection.Dispose();
            CashierServiceConnection = null;

            //Let the user know we closed the connection
            await new MessageDialog("Connection closed").ShowAsync();
        }

        private async void btnExcute_Click(object sender, RoutedEventArgs e)
        {
            var message = new ValueSet();
            Double n = Convert.ToDouble(txtexcute.Text);
            string type = "";
            if (cpu.IsChecked == true)
            {
                type = "cpu";
            }
            if (memory.IsChecked == true)
            {
                type = "memory";
            }
            message.Add("Items", n);
            message.Add("Type", type);
            AppServiceResponse response = await CashierServiceConnection.SendMessageAsync(message);
            if (response.Status == AppServiceResponseStatus.Success)
            {
                await new MessageDialog("Excute succesful").ShowAsync();
                return;
            }
            switch (response.Status)
            {
                case AppServiceResponseStatus.Failure:
                    await new MessageDialog("The service failed to acknowledge the message we sent it.").ShowAsync();
                    break;

                case AppServiceResponseStatus.ResourceLimitsExceeded:
                    await new MessageDialog("The service exceeded the resources allocated to it and had to be terminated.").ShowAsync();
                    break;

                case AppServiceResponseStatus.Unknown:
                default:
                    await new MessageDialog("An unkown error occurred while we were trying to send a message to the service.").ShowAsync();
                    break;
            }
        }

    }
}
