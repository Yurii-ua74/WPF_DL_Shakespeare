using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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


namespace WPF_DL_Shakespeare
{
    public partial class HundredBooks : Window
    {
        MainWindow mw = new MainWindow();
        private readonly HttpClient _httpClient;       

        public HundredBooks()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
        }
        
        private async void Button_Click(object sender, RoutedEventArgs e)
        {          
            ParseHtmlAsync();
        }
        public async Task ParseHtmlAsync()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("https://www.gutenberg.org/browse/scores/top");
                string htmlResponse = await response.Content.ReadAsStringAsync();

                // Розпарсити HTML
                var nStart = htmlResponse.IndexOf("Top 100 EBooks last 30 days</h2>");
                var nEnd = htmlResponse.IndexOf("Top 100 Authors last 30 days</h2>");

                if (nStart != -1 && nEnd != -1)
                {
                    string html = htmlResponse.Substring(nStart, nEnd - nStart);

                    var regex = new Regex(@"<a.*/ebooks/\d+.*>.*</a>");
                    foreach (Match match in regex.Matches(html))
                    {
                        var str = match.Value;

                        var data = new KeyValuePair<string, string>(
                            str.Substring(str.IndexOf("/ebooks/") + 8, str.IndexOf("\">") - (str.IndexOf("/ebooks/") + 8)),
                            str.Substring(str.IndexOf("\">") + 2, str.Length - str.IndexOf("\">") - 6)
                        );
                       
                        lstBox.Items.Add($"{data.Key}, {data.Value}\n");
                    }
                }
            }
            catch (Exception ex) { }
        }

        private void lstBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            listBX.Items.Clear();          
            ListBox listBox = (ListBox)sender;           
            string selectedItem = listBox.SelectedItem.ToString();
            string[] ind = selectedItem.Split(',');
            string i = ind[0];
            string text = "";
            WebClient client = new WebClient();
            using (Stream stream = client.OpenRead($"https://www.gutenberg.org/files/{i}/{i}-0.txt"))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string line = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBX.Items.Add(line);
                    }
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            mw.Show();
        }
    }
}
