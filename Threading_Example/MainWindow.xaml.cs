using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

        private List<int> list1;
        private List<int> list2;
        private List<int> list3;
        private List<int> list4;
        private List<int> list5;
        private List<int> list6;
        private List<int> list7;
        private List<int> list8;

        public MainWindow()
        {
            InitializeComponent();
            // Initializes the background worker
            InitializeBackgroundWorker();

            // Initialize lists of 1 million integers
            var count = 1000000;
            list1 = new List<int>(count);
            list2 = new List<int>(count);
            list3 = new List<int>(count);
            list4 = new List<int>(count);
            list5 = new List<int>(count);
            list6 = new List<int>(count);
            list7 = new List<int>(count);
            list8 = new List<int>(count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SortListButton_Click(object sender, RoutedEventArgs e)
        {
            SortedStringTextBox.Clear();
            SortListButton.IsEnabled = false;
            SortedStringTextBox.IsEnabled = false;
            PopulateListsHelper();
            SortThreadedSortListsHelper();

            PopulateListsHelper();
            MultiThreadedSortLists();

            SortListButton.IsEnabled = true;
            SortedStringTextBox.IsEnabled = true;
        }

        /// <summary>
        /// Download button click, disable UI, and run async task(s)
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

        //TODO: NOT WORKING
        /// <summary>
        /// Hitting enter key while in the text box automates clicking of the DownloadStringButton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UrlTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DownloadStringButton_Click(sender, e);
            }
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

        /// <summary>
        /// Populates a list with 1 million integers with random integers
        /// </summary>
        /// <param name="list"></param>
        /// <param name="count"></param>
        private List<int> PopulateLists(List<int> list, int count)
        {
            var random = new Random();
            list.Add(0);
            for (var i = 1; i < count; i++)
            {
                list.Add(random.Next());
            }
            return list;
        }

        /// <summary>
        /// Clear the contents of all lists
        /// </summary>
        private void ClearLists()
        {
            list1.Clear();
            list2.Clear();
            list3.Clear();
            list4.Clear();
            list5.Clear();
            list6.Clear();
            list7.Clear();
            list8.Clear();
        }

        /// <summary>
        /// Aids in single threaded lists sort
        /// </summary>
        private void SortThreadedSortListsHelper()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var task = Task.Factory.StartNew(() => SingleThreadedSortLists());
            Task.WaitAll(task);
            stopWatch.Stop();

            SortedStringTextBox.AppendText("Single-threaded Elapsed Time: " + stopWatch.ElapsedMilliseconds + "ms" + "\n");
        }

        /// <summary>
        /// Sort lists with one thread
        /// </summary>
        private void SingleThreadedSortLists()
        {
            var sorted = false;
            while (!sorted)
            {
                list1.Sort();
                list2.Sort();
                list3.Sort();
                list4.Sort();
                list5.Sort();
                list6.Sort();
                list7.Sort();
                list8.Sort();
                sorted = true;
            }
            ClearLists();
        }

        /// <summary>
        /// Multi-threaded sort of lists using task
        /// </summary>
        private void MultiThreadedSortLists()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var t1 = Task.Factory.StartNew(() => list1.Sort());
            var t2 = Task.Factory.StartNew(() => list1.Sort());
            var t3 = Task.Factory.StartNew(() => list1.Sort());
            var t4 = Task.Factory.StartNew(() => list1.Sort());
            var t5 = Task.Factory.StartNew(() => list1.Sort());
            var t6 = Task.Factory.StartNew(() => list1.Sort());
            var t7 = Task.Factory.StartNew(() => list1.Sort());
            var t8 = Task.Factory.StartNew(() => list1.Sort());
            Task.WaitAll(t1, t2, t3, t4, t5, t6, t7, t8);
            stopWatch.Stop();
            SortedStringTextBox.AppendText("Multi-threaded Elapsed Time: " + stopWatch.ElapsedMilliseconds + "ms\n");
            ClearLists();
        }

        /// <summary>
        /// Aids in populating all lists
        /// </summary>
        private void PopulateListsHelper()
        {
            var count = 1000000;
            list1 = PopulateLists(list1, count);
            list2 = PopulateLists(list2, count);
            list3 = PopulateLists(list3, count);
            list4 = PopulateLists(list4, count);
            list5 = PopulateLists(list5, count);
            list6 = PopulateLists(list6, count);
            list7 = PopulateLists(list7, count);
            list8 = PopulateLists(list8, count);
        }
    }
}
