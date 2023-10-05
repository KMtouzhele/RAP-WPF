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
using RAP_WPF.DataSource;
using RAP_WPF.Controller;

namespace RAP_WPF
{
    /// <summary>
    /// Interaction logic for ResearcherDetail.xaml
    /// </summary>
    public partial class ResearcherDetail : Window
    {
        public ImageSource PhotoSource { get; set; }
        public string NameInDetail { get; set; }
        public string JobTitle { get; set; }
        public string School { get; set; }
        public string Campus { get; set; }
        public string Email { get; set; }
        public string UtasStart { get; set; }
        public string CurrentStart { get; set; }
        public string Tenure { get; set; }
        public string Q1Percentage { get; set; }
        public string ThreeYearAverage { get; set; }
        public string PerformanceByPublications { get; set; }
        public string PerformanceByFunding { get; set; }
        public string SupervisionNumber { get; set; }
        public string StudentNames { get; set; }
        public string PreviousPositions { get; set; }
        public string Supervisor { get; set; }
        public string Degree { get; set; }
        public string Cumulative { get; set; }

        public List<Publication> OriginPublication(Researcher researcher)
        {
            DataContext = this;
            PublicationController publicationController = new PublicationController();
            ResearcherController researcherController = new ResearcherController();
            List<Publication> publications = publicationController.LoadPublicationFor(researcher);
            return publications;
        }
        public ResearcherDetail(Researcher researcher)
        {
            InitializeComponent();
            DataContext = this;
            ResearcherController researcherController = new ResearcherController();
            PublicationList.ItemsSource = OriginPublication(researcher);
            if(researcher is Student)
            {
                ForStudent.Visibility = Visibility.Visible;

            }
            else
            {
                ForStaff.Visibility = Visibility.Visible;
                PositionLabel.Visibility = Visibility.Visible;
                Position.Visibility = Visibility.Visible;
            }
        }

        private void SelectPublication(object sender, MouseButtonEventArgs e)
        {
            Publication selectedpublication = (Publication)PublicationList.SelectedItem;

            if (selectedpublication != null)
            {
                PublicationDetail publicationDetail = new PublicationDetail(selectedpublication);
                publicationDetail.DOI = selectedpublication.DOI;
                publicationDetail.Authors = selectedpublication.Author;
                publicationDetail.Ranking = selectedpublication.Ranking.ToString();
                publicationDetail.Type = selectedpublication.Type.ToString();
                publicationDetail.CiteAs = selectedpublication.CiteAs;
                publicationDetail.Year = selectedpublication.Year.ToString();
                publicationDetail.Available = selectedpublication.Available.ToString("yyyy-MM-dd");
                publicationDetail.Age = selectedpublication.Age.ToString("0,000")+ " days";
                publicationDetail.Show();
                
            }
        }

        private void EarlistFirst(object sender, RoutedEventArgs e)
        {
            List<Publication> currentItems = (List<Publication>)PublicationList.ItemsSource;
            if (currentItems != null)
            {
                var p = from pub in currentItems
                        orderby pub.Year, pub.Title descending
                        select pub;
                PublicationList.ItemsSource = (List<Publication>)p.ToList();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<Publication> currentItems = (List<Publication>)PublicationList.ItemsSource;
            if (currentItems != null)
            {
                var p = from pub in currentItems
                        orderby pub.Year descending, pub.Title
                        select pub;
                PublicationList.ItemsSource = (List<Publication>)p.ToList();
            }
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            int start = Int32.Parse(Start.Text);
            int end = Int32.Parse(End.Text);
            List<Publication> currentItems = (List<Publication>)PublicationList.ItemsSource;
            var p = from pub in currentItems
                    where pub.Year >= start && pub.Year <= end
                    select pub;
            PublicationList.ItemsSource = (List<Publication>)p.ToList();
        }

       

        /*private void Reset(object sender, RoutedEventArgs e)
        {
            PublicationList.ItemsSource = OriginPublication(researcher);
        }*/
    }
}
