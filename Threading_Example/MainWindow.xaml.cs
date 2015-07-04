using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Threading_Example
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BackgroundWorker ScrapeBackgroundWorker = new BackgroundWorker();

        public MainWindow()
        {
            InitializeComponent();
            // Initializes the background worker
            InitializeBackgroundWorker();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SortListButton_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Download button click - disable UI and run async task(s)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownloadStringButton_Click(object sender, RoutedEventArgs e)
        {
            // Disable input and output textbox and download button until page source download is complete
            UrlTextBox.IsEnabled = false;
            DownloadStringButton.IsEnabled = false;
            DownloadedStringTextBox.IsEnabled = false;

            // Run BackgroundWorker to scrape asynchronously.
            ScrapeBackgroundWorker.RunWorkerAsync(UrlTextBox.Text);
        }

        /// <summary>
        /// Hitting enter key while in the text box automates clicking of the DownloadStringButton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UrlTextBox_KeyDown(object sender, KeyEventArgs e)
        {

        }

        /// <summary>
        /// Clear URL text box on first click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UrlTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            // If box contains "Enter URL" and user has clicked the box, clear the box
            if (string.Compare(UrlTextBox.Text, "Enter URL") == 0)
            {
                UrlTextBox.Text = "";
            }
        }

        /// <summary>
        /// Initialize the background worker.
        /// </summary>
        private void InitializeBackgroundWorker()
        {
            // Subscribe to DoWork and Completed events
            ScrapeBackgroundWorker.DoWork += new DoWorkEventHandler(ScrapeBackgroundWorker_DoWork);
            ScrapeBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ScrapeBackgroundWorker_RunWorkerCompleted);
        }

        /// <summary>
        /// Download page source work tasks (upon execution).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrapeBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Set e to the string returned from the DownloadPageSource method
            e.Result = DownloadPageSource((string)e.Argument);
        }

        /// <summary>
        /// Display and enable UI work tasks (upon completion).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrapeBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DownloadedStringTextBox.Text = e.Result.ToString();

            // Enable input and output textbox and download button when complete
            UrlTextBox.IsEnabled = true;
            DownloadStringButton.IsEnabled = true;
            DownloadedStringTextBox.IsEnabled = true;
        }

        /// <summary>
        /// Download web page source code as a string
        /// </summary>
        private string DownloadPageSource(string url)
        {
            string source;
            // Initiallize req to an HTTP web request of the user-supplied URL
            var req = HttpWebRequest.Create(url);
            // Set the request method to GET
            // See GET/POST for details
            req.Method = "GET";

            // Download web page source code using StreamReader and save to source
            using (var reader = new StreamReader(req.GetResponse().GetResponseStream()))
            {
                source = reader.ReadToEnd();
            }

            return source;
        }
    }
}
