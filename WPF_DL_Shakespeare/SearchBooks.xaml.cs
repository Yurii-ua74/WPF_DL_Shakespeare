using HtmlAgilityPack;  // бібліотека для парсингу HTML
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class SearchBooks : Window
    {
        MainWindow mw = new MainWindow();
        private readonly HttpClient client = new();
        public async Task<string> GetUrlAsync(string Url)
        {
            try
            {
                var data = await client.GetByteArrayAsync(Url).ConfigureAwait(false);
                var str = Encoding.UTF8.GetString(data);
                return str;
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }
        public SearchBooks()
        {
            InitializeComponent();
        }

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string bookName = "";
            var tempKey = "";
            string[] tempName;
        
            if (txtSearch.Text != "")
                bookName = txtSearch.Text;
            else
            {
                MessageBox.Show("Введіть в поле назву книги");
                return;
            }
            try
            {
                var html = await GetUrlAsync($"https://www.gutenberg.org/ebooks/search/?query={bookName}&submit_search=Search");                
                
                    if (html == null) throw new Exception("Something went wrong.");
                    var temp = new List<string>();
                    html = html.Replace('\n', ' ');
                    html = html.Replace("<li class=\"booklink\">", "\n<li class=\"booklink\">");
                    var regex = new Regex(@"<li class=.booklink.>.*<span class=.hstrut.></span> </a> </li> ");

                    foreach (Match str in regex.Matches(html))
                        temp.Add(str.Value);
                    
                    foreach (var str in temp)
                    {
                        tempKey = str.Substring(str.IndexOf("ebooks/") + 7);
                        tempKey = tempKey.Remove(tempKey.IndexOf("\""));

                        var tempValue = str.Substring(str.IndexOf("=\"title\">") + 9);
                        tempName = tempValue.Split('<', '>');                       

                    if (tempName[0] == bookName) break;
                    }
                    myWebBrowser.Visibility = Visibility.Visible;                    
                  
                    myWebBrowser.Navigate(new Uri($"https://www.gutenberg.org/files/{tempKey}/{tempKey}-h/{tempKey}-h.htm"));
                    
            }
            catch (HttpRequestException ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            mw.Show();
        }
    }
}
