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
using RAP_WPF.Controller;
using RAP_WPF.DataSource;

namespace RAP_WPF
{
    /// <summary>
    /// Interaction logic for PublicationDetail.xaml
    /// </summary>
    public partial class PublicationDetail : Window
    {
        public string DOI { get; set; }
        public string PubTitle { get; set; }
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
            DOI = publication.DOI;
            PubTitle = publication.Title;
            Authors = publication.Author;
            Ranking = publication.Ranking.ToString();
            Type = publication.Type.ToString();
            CiteAs = publication.CiteAs;
            Year = publication.Year.ToString();
            Available = publication.Available.ToString("yyyy-MM-dd");
            Age = publication.Age.ToString() + " days";
        }
    }
}
