﻿using System;
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
using RAP_WPF.WhiteBoxTest;

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
        public string FundingRecieved { get; set; }
        public string Performance { get; set; }
        public string ThreeYearAverage { get; set; }
        public string PerformanceByPublications { get; set; }
        public string PerformanceByFunding { get; set; }
        public string SupervisionNumber { get; set; }
        public string StudentNames { get; set; }
        public string PreviousPositions { get; set; }
        public string Supervisor { get; set; }
        public string Degree { get; set; }
        public string Cumulative { get; set; }
        public Researcher _researcher { get; set; }


        public ResearcherDetail(Researcher researcher)
        {
            InitializeComponent();
            DataContext = this;
            _researcher = researcher;
            //List<Publication> test = WhiteBox.AddFakePublication(PublicationController.LoadPublicationFor(researcher));
            //PublicationList.ItemsSource = test;
            PublicationList.ItemsSource = PublicationController.LoadPublicationFor(researcher);

            PhotoSource = new BitmapImage(new Uri(researcher.Photo));
            NameInDetail = researcher.NameShown;
            JobTitle = researcher.JobTitle;
            School = researcher.School;
            
            //Deal with the "Cradle Coast"
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
            Q1Percentage = researcher.Q1Percentage.ToString("P");
            ThreeYearAverage = ResearcherController.ThreeYearAverage(researcher).ToString("0.00");
            Cumulative = PublicationController.LoadCumulativeNumber(researcher);

            //Student page visibility and info
            if (researcher is Student)
            {
                Student selectedstudent = (Student)researcher;
                Supervisor = ResearcherController.LoadSupervisor(selectedstudent);
                Degree = selectedstudent.Degree;
                ForStudent.Visibility = Visibility.Visible;
            }
            //Staff page visibility and info
            else
            {
                Staff selectedstaff = (Staff)researcher;
                FundingRecieved = "AUD "+PublicationController.FundingRecieved(selectedstaff).ToString("0,000");
                Performance = ResearcherController.Performance(selectedstaff).ToString("P");
                PerformanceByPublications = selectedstaff.PerformanceByPublicaton.ToString("0") + " publications per year";
                PerformanceByFunding = PublicationController.FundingPerformance(researcher).ToString("0,000") + " AUD/year";
                PreviousPositions = ResearcherController.LoadPreviousPosition(researcher);
                SupervisionNumber = ResearcherController.CalculateSupervision(selectedstaff).ToString();
                StudentNames = ResearcherController.LoadSupervision(selectedstaff);
                ForStaff.Visibility = Visibility.Visible;
                PositionLabel.Visibility = Visibility.Visible;
                Position.Visibility = Visibility.Visible;
            }
            

        }

        //Open a new Window to show publication details
        private void SelectPublication(object sender, MouseButtonEventArgs e)
        {
            Publication selectedpublication = (Publication)PublicationList.SelectedItem;

            if (selectedpublication != null)
            {
                PublicationDetail publicationDetail = new PublicationDetail(selectedpublication);
                publicationDetail.Show();
            }
        }

        //Function that Invert the publication list
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

        //Function that Invert the publication list again
        private void LatestFirst(object sender, RoutedEventArgs e)
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

        //Function that filter publication list based on the input year range
        private void Search(object sender, RoutedEventArgs e)
        {

            //Verify if the input is valid or not
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

        //Function that reset the publication list as new
        private void Reset(object sender, RoutedEventArgs e)
        {
            Start.Text = "";
            End.Text = "";
            ReloadPublications();
        }

        public void ReloadPublications()
        {
            if (_researcher != null)
            {
                PublicationList.ItemsSource = PublicationController.LoadPublicationFor(_researcher);
            }
        }
    }
}
