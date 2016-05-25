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
using Windows.System;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Sun_ResultTarget
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ObservableCollection<Goods> selectedgoods;
        ProtocolForResultsOperation operation;
        string totalAmount;
        public MainPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var protocolForResultsArgs = e.Parameter as ProtocolForResultsActivatedEventArgs;
            operation = protocolForResultsArgs.ProtocolForResultsOperation;
            var callerpfn = protocolForResultsArgs.CallerPackageFamilyName;
            if (protocolForResultsArgs.Data.Keys.Count == 1 && protocolForResultsArgs.Data.ContainsKey("Items"))
            {
                selectedgoods = JSONHelper.JsonDeserialize<ObservableCollection<Goods>>(protocolForResultsArgs.Data["Items"].ToString());
            }
           totalAmount = selectedgoods.Select(i => i.Price).Sum().ToString();
            txtTotal.Text = string.Format("Total:{0}", totalAmount);
        }

        private void BtnCheckout_Click(object sender, RoutedEventArgs e)
        {
            ValueSet result = new ValueSet();
            result["ReturnData"] =totalAmount;
            operation.ReportCompleted(result);
        }

        private async void BtnLaunchc_Click(object sender, RoutedEventArgs e)
        {
            var testAppUri = new Uri("sun-targetapp:"); // The protocol handled by the launched app
            var options = new LauncherOptions();
            options.TargetApplicationPackageFamilyName = "3f162b5e-98e5-4118-a2d9-8882398d7d8d_75cr2b68sm664";
        
            var inputData = new ValueSet();
            inputData["Items"] ="aaa";
            LaunchUriResult result = await Windows.System.Launcher.LaunchUriForResultsAsync(testAppUri, options, inputData);
        }
    }
}
