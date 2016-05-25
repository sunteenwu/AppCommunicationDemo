using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SunCheckPoint
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Scenario1 : Page
    {
        public MainPage rootPage;
        public Scenario1()
        {
            this.InitializeComponent();
            rootPage = MainPage.Current;
        }
        
        //private async void LaunchMap_Click(object sender, RoutedEventArgs e)
        //{
        //    var uriNewYork = new Uri(@"bingmaps:");
        //    // Launch the Windows Maps app
        //    var launcherOptions = new Windows.System.LauncherOptions();
        //    launcherOptions.TargetApplicationPackageFamilyName = "Microsoft.WindowsMaps_8wekyb3d8bbwe";
        //    var success = await Windows.System.Launcher.LaunchUriAsync(uriNewYork, launcherOptions);
        //}

        private async void LaunchFile_Click(object sender, RoutedEventArgs e)
        {
       // Windows.ApplicationModel.Activation.ActivationKind
            string imageFile = @"Image\googlelogo_color_272x92dp.png";             
            //string location = Windows.ApplicationModel.Package.Current.InstalledLocation.ToString();
            //System.Diagnostics.Debug.WriteLine(location);
            var file = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(imageFile);
            var options = new Windows.System.LauncherOptions();
            options.DisplayApplicationPicker = false;
            options.DesiredRemainingView = Windows.UI.ViewManagement.ViewSizePreference.UseLess;          
           
            if (file != null)
            {
                // Launch the retrieved file
                var success = await Windows.System.Launcher.LaunchFileAsync(file, options);               
                if (success)
                {
                    // File launched
                    rootPage.NotifyUser("Launch success", NotifyType.StatusMessage);
                }
                else
                {
                    rootPage.NotifyUser("Launch Fail", NotifyType.ErrorMessage);
                }
            }
            else
            {
                rootPage.NotifyUser("Cannot find the file", NotifyType.ErrorMessage);
            }
        }

        private async void LaunchPeople_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri(@"ms-people:viewcontact:");
           var success=  await Launcher.LaunchUriAsync(uri);
            if(success)
            {
                rootPage.NotifyUser("Launch success", NotifyType.StatusMessage);                
            }
            else
            {
                rootPage.NotifyUser("Launch Fail", NotifyType.ErrorMessage);
            }
        }

        private async void LaunchTargetBySameUri_Click(object sender, RoutedEventArgs e)
        {
            var testAppUri = new Uri("sun-targetapp:");
            await Launcher.LaunchUriAsync(testAppUri);
        }

        private async void LaunchTargetByFamily_Click(object sender, RoutedEventArgs e)
        {
            var testAppUri = new Uri("sun-targetapp:");
            var options = new LauncherOptions();
            options.TargetApplicationPackageFamilyName = "3f162b5e-98e5-4118-a2d9-8882398d7d8d_75cr2b68sm664";
            await Launcher.LaunchUriAsync(testAppUri,options);
        }

        private async void LaunchTargetPostFile_Click(object sender, RoutedEventArgs e)
        {
     
            var options = new LauncherOptions();
            options.TargetApplicationPackageFamilyName = "3f162b5e-98e5-4118-a2d9-8882398d7d8d_75cr2b68sm664";
            var folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile gpxFile = await folder.GetFileAsync("Garfield.jpg");
            var token = SharedStorageAccessManager.AddFile(gpxFile);
            var testAppUri = new Uri("sun-targetapp:?GpxFile="+token);
            //ValueSet inputData = new ValueSet();
            //inputData.Add("Token", token);
            await Launcher.LaunchUriAsync(testAppUri, options);
        }

        private async void ReplaceLocal_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".bmp");
            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read);
                BitmapImage pic = new BitmapImage();
                pic.SetSource(stream);
                this.head.Source = pic;
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile sampleFile = await storageFolder.CreateFileAsync(PlayerName.Text + ".jpg", CreationCollisionOption.ReplaceExisting);
                await file.CopyAndReplaceAsync(sampleFile);
            }
        }

        private void Replacedirectly_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
