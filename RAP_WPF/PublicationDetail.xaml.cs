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
using System.Windows.Shapes;
using RAP_WPF.Entity;

namespace RAP_WPF
{
    /// <summary>
    /// Interaction logic for PublicationDetail.xaml
    /// </summary>
    public partial class PublicationDetail : Window
    {
        public string DOI { get; set; }
        public string Authors { get; set; }
        public string Year { get; set; }
        public string Ranking { get; set; }
        public string Type { get; set; }
        public string CiteAs { get; set; }
        public string Available { get; set; }
        public string Age { get; set; }

        public PublicationDetail(Publication publication)
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
