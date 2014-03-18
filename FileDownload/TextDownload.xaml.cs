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
    public partial class TextDownload : PhoneApplicationPage
    {
        String doc;
        int j;
        private BackgroundWorker bw = new BackgroundWorker();
        String[] url ={"http://www.sjsu.edu/towerfoundation/docs/Employment-Handbook-12.doc",
                      "http://www.engr.sjsu.edu/cme/assets/files/aluminfo.doc",
                      "http://www.engr.sjsu.edu/E10/E10pdf/RobotProjectGuidelinesF13.doc",
                      "http://www.sjsu.edu/edd/Letter_of_Rec_form.doc",
                      "http://www.sjsu.edu/publicaffairs/docs/sjsu_fax_template.doc"};


        public TextDownload()
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
                using (IsolatedStorageFileStream stream = storageFile.OpenFile(url[j].Substring(url[j].LastIndexOf("/"),15) + ".doc", FileMode.Create))
                {
                    await stream.WriteAsync(buffer, 0, buffer.Length);
                   // stream.Close();
                }
            }
        }
    }
}