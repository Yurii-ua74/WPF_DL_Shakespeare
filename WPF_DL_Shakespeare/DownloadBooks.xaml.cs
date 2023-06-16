using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace WPF_DL_Shakespeare
{    
    public partial class DownloadBooks : Window
    {
        private readonly HttpClient client = new();
        MainWindow mw = new MainWindow();        
        Dictionary<string, string> index = new Dictionary<string, string>();
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
        public DownloadBooks()
        {
            InitializeComponent();
        }

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            progressStatus.IsIndeterminate = true;
            string bookName = "";
            var tempKey = "";
            string[] tempName;

            if (txtSearch.Text != "")
            {
                bookName = txtSearch.Text;
                bookName = bookName.Replace(" ", "");
            }
            else
            {
                MessageBox.Show("Введіть в поле назву книги");
                return;
            }
            var nextIndex = 1;
            try
            {
                progressStatus.IsIndeterminate = true;
                do 
                { 
                    var html = await GetUrlAsync($"https://www.gutenberg.org/ebooks/search/?query=&submit_search=&start_index={nextIndex}");
                     
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
                         //tempKey = tempKey.Replace("\"", "");

                         var tempValue = str.Substring(str.IndexOf("=\"title\">") + 9);
                         tempValue = tempValue.Replace("</span> <span class=\"subtitle\">", "; Author: ");
                         tempValue = tempValue.Remove(tempValue.IndexOf("</span> "));
                         tempName = tempValue.Split("Author:");
                         string name = tempName[tempName.Length - 1].Replace(" ", "");

                        if (name == bookName)
                            index.Add(tempKey, tempName[0]);
                     }
                    if (html.IndexOf("\">Next</a>") == -1) break;
                                      
                    nextIndex += 25;
                } while (true);
                int i = 1;
                foreach(var ind in index)
                {
                    txtBoxInd.Text += i +": "+ ind.Value + "\n";
                    DownloadFileAsync(ind.Key, ind.Value); i++;
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
            progressStatus.IsIndeterminate = false;
            progressStatus.Value = 100;
        }
        private static async Task DownloadFileAsync(string index, string name_book)
        {
            WebClient client = new WebClient();
            await client.DownloadFileTaskAsync(new Uri($"https://www.gutenberg.org/files/{index}/{index}-0.txt"), name_book+".txt");
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            mw.Show();
        }
    }
}
