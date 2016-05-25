using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
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
    public sealed partial class Scenario5 : Page
    {
        public MainPage rootPage;
        public Scenario5()
        {
            this.InitializeComponent();
            rootPage = MainPage.Current;
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
                rootPage.NotifyUser("Please choose one folder.", NotifyType.ErrorMessage);
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
                rootPage.NotifyUser("Please choose one folder.", NotifyType.ErrorMessage);
                return;
            }
            var shareFile = await sharedFolder.CreateFileAsync("share.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);

            using (Stream stream = await shareFile.OpenStreamForWriteAsync())
            {
                byte[] content = System.Text.Encoding.UTF8.GetBytes(txtShareInsert.Text.Trim());
                await stream.WriteAsync(content, 0, content.Length);
            }
            rootPage.NotifyUser("Insert succesful.", NotifyType.StatusMessage);
        }
    }
}
