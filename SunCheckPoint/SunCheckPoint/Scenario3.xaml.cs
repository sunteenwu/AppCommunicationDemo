using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.AppService;
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
using Windows.ApplicationModel.DataTransfer;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SunCheckPoint
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Scenario3 : Page
    { 
        public Scenario3()
        {
            this.InitializeComponent();
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += DataTransferManager_DataRequested;
        }
         
        private void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;
            request.Data.RequestedOperation = DataPackageOperation.Move;
            request.Data.SetText(TxtSharetext.Text);
            request.Data.Properties.Title = "Share Example";
            Clipboard.SetContent(request.Data);

        } 
         
        private void BtnShareUI_OnClick(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();

        }
    }
}
