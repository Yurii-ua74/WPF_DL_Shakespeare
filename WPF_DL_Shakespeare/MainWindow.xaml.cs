using System;
using System.Collections.Generic;
using System.Linq;
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

namespace WPF_DL_Shakespeare
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnTask1_Click(object sender, RoutedEventArgs e)
        {
            Hamlet ham = new Hamlet();
            ham.Show();
            this.Close();
        }

        private void btnTask2_Click(object sender, RoutedEventArgs e)
        {
            HundredBooks hb = new HundredBooks();
            hb.Show();
            this.Close();
        }

        private void btnTask3_Click(object sender, RoutedEventArgs e)
        {
            SearchBooks sbooks = new SearchBooks();
            sbooks.Show();
            this.Close();
        }

        private void btnTask4_Click(object sender, RoutedEventArgs e)
        {
            DownloadBooks dl = new DownloadBooks();
            dl.Show();
            this.Close();
        }
    }
}
