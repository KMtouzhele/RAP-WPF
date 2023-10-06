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


        public ResearcherDetail(Researcher researcher)
        {
            InitializeComponent();
            DataContext = researcher;
            ResearcherController researcherController = new ResearcherController();
            PublicationController publicationController = new PublicationController();
            PublicationList.ItemsSource = publicationController.LoadPublicationFor(researcher);
            PhotoSource = new BitmapImage(new Uri(researcher.Photo));
            NameInDetail = researcher.NameShown;
            JobTitle = researcher.JobTitle;
            School = researcher.School;

            if (researcher.Campus == AllEnum.Campus.Cradle_Coast)
            {
                this.Campus = "Cradle Coast";
            }
            else
            {
                this.Campus = researcher.Campus.ToString();
            }

            Email = researcher.Email;
            UtasStart = researcher.UtasStart.ToString("yyyy-MM-dd");
            CurrentStart = researcher.CurrentStart.ToString("yyyy-MM-dd");
            Tenure = researcher.Tenure.ToString("0.00") + " years";
            Q1Percentage = researcher.Q1Percentage.ToString();
            ThreeYearAverage = researcherController.ThreeYearAverage(researcher).ToString();
            Cumulative = publicationController.LoadCumulativeNumber(researcher);

            if (researcher is Student)
            {
                Student selectedstudent = (Student)researcher;
                Supervisor = researcherController.LoadSupervisor(selectedstudent);
                ForStudent.Visibility = Visibility.Visible;
            }
            else
            {
                Staff selectedstaff = (Staff)researcher;
                PerformanceByPublications = selectedstaff.PerformanceByPublicaton.ToString("0") + " publications per year";
                PerformanceByFunding = publicationController.FundingPerformance(researcher).ToString("0,000") + " AUD/year";
                PreviousPositions = researcherController.LoadPreviousPosition(researcher);
                SupervisionNumber = researcherController.CalculateSupervision(selectedstaff).ToString();
                StudentNames = researcherController.LoadSupervision(selectedstaff);
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
            if (!int.TryParse(Start.Text, out int start) || !int.TryParse(End.Text, out int end))
            {
                MessageBox.Show("Please enter 4-digit years in both Start and End fields.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                int startyear = Int32.Parse(Start.Text);
                int endyear = Int32.Parse(End.Text);
                List<Publication> currentItems = (List<Publication>)PublicationList.ItemsSource;
                var p = from pub in currentItems
                        where pub.Year >= startyear && pub.Year <= endyear
                        select pub;
                PublicationList.ItemsSource = (List<Publication>)p.ToList();
            }
        }

        private void Reset(object sender, RoutedEventArgs e)
        {
            Start.Text = "";
            End.Text = "";
            ReloadPublications();
        }

        public void ReloadPublications()
        {
            if (DataContext is Researcher researcher)
            {
                PublicationController publicationController = new PublicationController();
                PublicationList.ItemsSource = publicationController.LoadPublicationFor(researcher);
            }
        }
    }
}
