using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;
using CommonLibrary;
using System.IO;

namespace CashierTask
{
    public sealed class CashierTask : IBackgroundTask
    {

        private BackgroundTaskDeferral taskDeferral = null;
        public void Run(IBackgroundTaskInstance taskInstance)
        {

            taskDeferral = taskInstance.GetDeferral();
            //taskInstance.Canceled += OnTaskCanceled;
            AppServiceTriggerDetails details = taskInstance.TriggerDetails as AppServiceTriggerDetails;
            if (details != null)
            {
                string serviceName = details.Name;
                if (serviceName == "SunCashierService")
                {
                    AppServiceConnection conn = details.AppServiceConnection;
                    conn.RequestReceived += Conn_RequestReceived;
                }
            }

        }

        //private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        //{
        //    if (taskDeferral != null)
        //    {
        //        //Complete the service deferral
        //        taskDeferral.Complete();
        //        taskDeferral = null;
        //    }
        //}
        private async void Conn_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            var MessageDeferral = args.GetDeferral();
            var input = args.Request.Message;
            string jsonitems = Convert.ToString((args.Request.Message["Items"]));

            var goodselectlist = JSONHelper.JsonDeserialize<List<Goods>>(jsonitems);
            String totalAmount;
            totalAmount = goodselectlist.Select(i => i.Price).Sum().ToString();
            ValueSet resdata = new ValueSet();
            resdata["Result"] = "Success";
            resdata["Total"] = totalAmount;
            await args.Request.SendResponseAsync(resdata);
            #region
            // string exceedtest = Convert.ToString((args.Request.Message["test"]));
            //byte[] content= System.Text.Encoding.UTF8.GetBytes(exceedtest);
            // System.Diagnostics.Debug.WriteLine(jsonitems);
            //var Folder1 = Windows.Storage.ApplicationData.Current.LocalFolder;
            //var shareFile = await Folder1.CreateFileAsync("time.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            //using (Stream stream = await shareFile.OpenStreamForWriteAsync())
            //{
            //    byte[] content = System.Text.Encoding.UTF8.GetBytes(System.DateTime.Now.ToString().Trim());
            //    await stream.WriteAsync(content, 0, content.Length);
            //    totalAmount = Math.Pow(2, 100000).ToString();
            //   content = System.Text.Encoding.UTF8.GetBytes(System.DateTime.Now.ToString().Trim());
            //    await stream.WriteAsync(content, 0, content.Length);
            //}

            //while(true)
            //{
            //    totalAmount = totalAmount + "*";
            //    var Folder1 = Windows.Storage.ApplicationData.Current.LocalFolder;
            //    var shareFile = await Folder1.CreateFileAsync("share.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            //    using (Stream stream = await shareFile.OpenStreamForWriteAsync())
            //    {
            //        byte[] content = System.Text.Encoding.UTF8.GetBytes(totalAmount.Trim() );
            //        await stream.WriteAsync(content, 0, content.Length);
            //    }

            //}
            #endregion


        }

    }
}
