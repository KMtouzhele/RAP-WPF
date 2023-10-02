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

        public ResearcherDetail(Researcher researcher)
        {
            InitializeComponent();
            DataContext = this;
            PublicationController publicationController = new PublicationController();
            List<Publication> publications = publicationController.LoadPublicationFor(researcher);
            PublicationList.ItemsSource = publications;
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
                publicationDetail.Available = selectedpublication.Available.ToString();
                publicationDetail.Age = selectedpublication.Age.ToString();
                publicationDetail.Show();
                
            }
        }
    }
}
