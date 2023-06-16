using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPF_DL_Shakespeare
{
    public partial class Hamlet : Window
    {
        MainWindow mw = new MainWindow();
        public Hamlet()
        {
            InitializeComponent();
        }

        private async void download_Click(object sender, RoutedEventArgs e)
        {
            DownloadFileAsync().GetAwaiter();
            MessageBox.Show("Файл завантажено та збережено під назвою RomeoAndJuliet.txt");
            myBrowser.Navigate(new Uri("https://www.gutenberg.org/files/27761/27761-h/27761-h.htm"));
            myBrowser.Visibility = Visibility.Visible;
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (myBrowser.CanGoForward) myBrowser.GoForward();            
            else  MessageBox.Show("Cannot Go to next");            
        }

        private void btnPrevew_Click(object sender, RoutedEventArgs e)
        {
            if (myBrowser.CanGoBack) myBrowser.GoBack();
            else MessageBox.Show("Cannot Go back");           
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            mw.Show();
        }
        private static async Task DownloadFileAsync()
        {
            WebClient client = new WebClient();
            await client.DownloadFileTaskAsync(new Uri("https://www.gutenberg.org/files/1513/1513-0.txt"), "RomeoAndJuliet.txt");
        }
    }
}
