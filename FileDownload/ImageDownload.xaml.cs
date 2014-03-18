using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using System.ComponentModel;
using System.Threading;
using System.IO;

namespace FileDownload
{
    public partial class ImageDownload : PhoneApplicationPage
    {
        String doc;
        int j;
        private BackgroundWorker bw = new BackgroundWorker();
        String[] url ={"http://blogs.sjsu.edu/today/files/2014/01/mima-mounds-1l9fs40.jpg",
                       "http://blogs.sjsu.edu/today/files/2014/01/Gragera-inpost-11xdqr8.jpg",
                       "http://blogs.sjsu.edu/today/files/2014/01/spider-inpost-285iz3l.jpg",
                       "http://www.engr.sjsu.edu/files/images/exceed-image-14-120815.thumbnail.jpg",
                      "http://www.engr.sjsu.edu/files/images/exceed-group-1-120815.thumbnail.jpg",
                       "http://blogs.sjsu.edu/today/files/2014/02/0005_proam-1uawpew.jpg",
                       "http://blogs.sjsu.edu/today/files/2013/11/dance_0003_20131101-396-0045.jpg-zq3m7w.jpg",
                       "http://blogs.sjsu.edu/today/files/2013/11/dance_0009_20131101-396-0355.jpg-16qimv6.jpg",
                       "http://blogs.sjsu.edu/today/files/2013/11/dance_0022_20131101-396-0822.jpg-1qyfyhk.jpg", 
                       "http://blogs.sjsu.edu/today/files/2013/11/dance_0027_20131101-396-0996.jpg-2klbnkv.jpg"};


        public ImageDownload()
        {
            InitializeComponent();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            //  bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //   MessageBox.Show(e.ProgressPercentage.ToString() + "%");

        }
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            for (int i = 0; i < url.Length; i++)
            {
                var client = new WebClient();
                client.OpenReadCompleted += client_OpenReadCompleted;
                client.OpenReadAsync(new Uri(url[i]));
                //worker.ReportProgress(10);
            }

        }
        private void Download(object sender, RoutedEventArgs e)
        {
            if (bw.IsBusy != true)
            {
                bw.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("wait");
            }
        }
        async void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            byte[] buffer = new byte[e.Result.Length];
            await e.Result.ReadAsync(buffer, 0, buffer.Length);
            using (IsolatedStorageFile storageFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                j++;
                using (IsolatedStorageFileStream stream = storageFile.OpenFile(url[j].Substring(url[j].LastIndexOf("/"),15) + ".jpg", FileMode.Create))
                {
                    await stream.WriteAsync(buffer, 0, buffer.Length);
                    // stream.Close();
                }
            }
        }
    }
}