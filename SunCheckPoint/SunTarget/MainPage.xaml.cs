using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SunPublishShare
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
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var protocolArgs = e.Parameter as ProtocolActivatedEventArgs;
            if (protocolArgs.Uri.Query != "")
            {
                var queryStrings = new WwwFormUrlDecoder(protocolArgs.Uri.Query);
                string gpxFileToken = queryStrings.GetFirstValueByName("GpxFile");
                if (!string.IsNullOrEmpty(gpxFileToken))
                {
                  StorageFile fileget=await SharedStorageAccessManager.RedeemTokenForFileAsync(gpxFileToken);
                    BitmapImage bitmapimage = new BitmapImage();
                    bitmapimage.SetSource(await fileget.OpenReadAsync());
                    imgAppdeliver.Source =bitmapimage;
                }
            }
        }

        private async void btnRead_Click(object sender, RoutedEventArgs e)
        {
            StorageFolder sharedFolder = null;
            if (rbtMyfolder1r.IsChecked == true)
            {
                sharedFolder = Windows.Storage.ApplicationData.Current.GetPublisherCacheFolder("myFolder1");
            }
            if (rbtMyfolder2r.IsChecked == true)
            {
                sharedFolder = Windows.Storage.ApplicationData.Current.GetPublisherCacheFolder("myFolder2");
            }
            if (sharedFolder == null)
            {
                await new MessageDialog("Please choose one folder.", "Notify").ShowAsync();
                return;
            }
            StorageFile shareFile = null;
            try
            {
                shareFile = await sharedFolder.GetFileAsync("share.txt");
            }
            catch (Exception exc)
            {
                await new MessageDialog(exc.Message, "提示").ShowAsync();
            }

            if (shareFile != null)
            {
                var accessStream = await shareFile.OpenReadAsync();
                using (Stream stream = accessStream.AsStreamForRead((int)accessStream.Size))
                {
                    byte[] content = new byte[stream.Length];
                    await stream.ReadAsync(content, 0, (int)stream.Length);

                    txtShareRead.Text = System.Text.Encoding.UTF8.GetString(content, 0, content.Length);
                }
            }
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtShareInsert.Text.Trim())) return;
            StorageFolder sharedFolder = null;
            if (rbtMyfolder1.IsChecked == true)
            {
                sharedFolder = Windows.Storage.ApplicationData.Current.GetPublisherCacheFolder("myFolder1");
            }
            if (rbtMyfolder2.IsChecked == true)
            {
                sharedFolder = Windows.Storage.ApplicationData.Current.GetPublisherCacheFolder("myFolder2");
            }
            if (sharedFolder == null)
            {
                await new MessageDialog("Please choose one folder.", "Notify").ShowAsync();
                return;
            }
            var shareFile = await sharedFolder.CreateFileAsync("share.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);

            using (Stream stream = await shareFile.OpenStreamForWriteAsync())
            {
                byte[] content = System.Text.Encoding.UTF8.GetBytes(txtShareInsert.Text.Trim());
                await stream.WriteAsync(content, 0, content.Length);
            }
            await new MessageDialog("Insert sucessful.", "Notify").ShowAsync();
        }

    }
}
