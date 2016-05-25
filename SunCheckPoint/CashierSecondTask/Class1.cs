using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;

namespace CashierSecondTask
{
    public sealed class SecondTask : IBackgroundTask
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
                if (serviceName == "SunCashierSecondService")
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
            string type = input["Type"].ToString();
            long count =long.Parse(input["Items"].ToString());
            if (type == "cpu")
            {
                int loopCount = 6;
                Parallel.For(1, loopCount, (i, loopState) =>
                {
                    fib(count);
                });
            }
            if (type == "memory")
            {
                byte[] bytes = new byte[2000 * 1024 * 1024];
            }

            ValueSet resdata = new ValueSet();
            resdata["Result"] = "Success";
            await args.Request.SendResponseAsync(resdata);
            MessageDeferral.Complete();
            #region write log
            //var Folder1 = Windows.Storage.ApplicationData.Current.LocalFolder;
            //var shareFile = await Folder1.CreateFileAsync("log.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            //   
            //    using (Stream stream = await shareFile.OpenStreamForWriteAsync())
            //    {
            //        byte[] content = System.Text.Encoding.UTF8.GetBytes(System.DateTime.Now.ToString().Trim());
            //        await stream.WriteAsync(content, 0, content.Length);
             
            // Amount = Encoding.UTF8.GetBytes(fib(100).ToString());
            //content = System.Text.Encoding.UTF8.GetBytes(System.DateTime.Now.ToString().Trim());
            // await stream.WriteAsync(content, 0, content.Length);
            // await stream.WriteAsync(Amount, 0, content.Length);
            //}

            // resdata["Total"] = totalAmount;
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

        public long fib(long n)
        {
            if (n == 0)
            {
                return 0;
            }
            else if (n == 1)
            {
                return 1;
            }
            else
            {
                return fib(n - 1) + fib(n - 2);
            }
        }
    }
}

