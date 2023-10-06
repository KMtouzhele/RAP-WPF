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
using RAP_WPF.Entity;
using RAP_WPF.DataSource;
using RAP_WPF.Controller;

namespace RAP_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ResearcherController researcherController = new ResearcherController();
            List<Researcher> researchers = DBAdapter.AllResearchers();
            ResearcherList.ItemsSource = researchers;
            Poor.ItemsSource = researcherController.LoadResearcherByPerformance(0, 0.7);
            BelowExpectations.ItemsSource = researcherController.LoadResearcherByPerformance(0.7, 1.1);
            var meetingminimun = researcherController.LoadResearcherByPerformance(1.1, 2);
            var r3 = from researcher in meetingminimun
                    orderby researcher.Performance descending
                    select researcher;
            MeetingMinimum.ItemsSource = r3.ToList();

            var starperformancer = researcherController.LoadResearcherByPerformance(2, 999);
            var r4 = from researcher in starperformancer
                    orderby researcher.Performance descending
                    select researcher;
            StarPerformancer.ItemsSource = r4.ToList();

        }

        private List<Researcher> FilterAndDisplayResults()
        {
            ComboBoxItem selecteditem = (ComboBoxItem)FilterByLevel.SelectedItem;
            ResearcherController researcherController = new ResearcherController();
            List<Researcher> result = new List<Researcher>();
            if (selecteditem != null)
            {
                string selectedcontent = selecteditem.Content.ToString();
                AllEnum.EmploymentLevel selectedlevel = AllEnum.EmploymentLevel.Student;

                switch (selectedcontent)
                {
                    case "Level A":
                        selectedlevel = AllEnum.EmploymentLevel.A;
                        break;
                    case "Level B":
                        selectedlevel = AllEnum.EmploymentLevel.B;
                        break;
                    case "Level C":
                        selectedlevel = AllEnum.EmploymentLevel.C;
                        break;
                    case "Level D":
                        selectedlevel = AllEnum.EmploymentLevel.D;
                        break;
                    case "Level E":
                        selectedlevel = AllEnum.EmploymentLevel.E;
                        break;
                    case "Student":
                        selectedlevel = AllEnum.EmploymentLevel.Student;
                        break;
                    default:
                        break;
                }
                List<Researcher> filteredresearchersByLevel = researcherController.FilterByLevel(DBAdapter.AllResearchers(), selectedlevel);
                if (SearchBox.Text != null)
                {
                    string input = SearchBox.Text;
                    result = researcherController.FilterByName(filteredresearchersByLevel, input);
                }
                else
                {
                    result = filteredresearchersByLevel;
                }
            }
            else
            {
                if (SearchBox.Text != null)
                {
                    string input = SearchBox.Text;
                    result = researcherController.FilterByName(DBAdapter.AllResearchers(), input);
                }
                else
                {
                    result = DBAdapter.AllResearchers();
                }
            }
            return result;
            
        }

        private void filtered(object sender, SelectionChangedEventArgs e)
        {
            ResearcherList.ItemsSource = FilterAndDisplayResults();
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            ResearcherList.ItemsSource = FilterAndDisplayResults();
        }


        private void Reset(object sender, RoutedEventArgs e)
        {
            ResearcherList.ItemsSource = DBAdapter.AllResearchers();
            SearchBox.Text = "";
            FilterByLevel.SelectedItem = null;
        }

        private void SelectResearcher(object sender, MouseButtonEventArgs e)
        {
            Researcher selectedresearcher = (Researcher)ResearcherList.SelectedItem;
            ResearcherController researcherController = new ResearcherController();
            PublicationController publicationController = new PublicationController();

            if (selectedresearcher != null)
            {
                ResearcherDetail researcherdetail = new ResearcherDetail(selectedresearcher);
                researcherdetail.PhotoSource = new BitmapImage(new Uri(selectedresearcher.Photo));
                researcherdetail.NameInDetail = selectedresearcher.NameShown;
                researcherdetail.JobTitle = selectedresearcher.JobTitle;
                researcherdetail.School = selectedresearcher.School;
                if (selectedresearcher.Campus == AllEnum.Campus.Cradle_Coast)
                {
                    researcherdetail.Campus = "Cradle Coast";
                }
                else
                {
                    researcherdetail.Campus = selectedresearcher.Campus.ToString();
                }
                researcherdetail.Email = selectedresearcher.Email;
                researcherdetail.UtasStart = selectedresearcher.UtasStart.ToString("yyyy-MM-dd");
                researcherdetail.CurrentStart = selectedresearcher.CurrentStart.ToString("yyyy-MM-dd");
                researcherdetail.Tenure = selectedresearcher.Tenure.ToString("0.00") + " years";
                researcherdetail.Q1Percentage = selectedresearcher.Q1Percentage.ToString();
                researcherdetail.ThreeYearAverage = researcherController.ThreeYearAverage(selectedresearcher).ToString();
                researcherdetail.Cumulative = publicationController.LoadCumulativeNumber(selectedresearcher);
                if(selectedresearcher is Student)
                {
                    Student selectedstudent = (Student)selectedresearcher;
                    researcherdetail.Supervisor = researcherController.LoadSupervisor(selectedstudent);
                }
                else
                {
                    Staff selectedstaff = (Staff)selectedresearcher;
                    researcherdetail.PerformanceByPublications = selectedstaff.PerformanceByPublicaton.ToString("0") + " publications per year";
                    researcherdetail.PerformanceByFunding = publicationController.FundingPerformance(selectedresearcher).ToString("0,000") + " AUD/year"; researcherdetail.PreviousPositions = researcherController.LoadPreviousPosition(selectedresearcher);
                    researcherdetail.SupervisionNumber = researcherController.CalculateSupervision(selectedstaff).ToString();
                    researcherdetail.StudentNames = researcherController.LoadSupervision(selectedstaff);
                }
                researcherdetail.Show();
            }
        }

        private void ReportClicked(object sender, RoutedEventArgs e)
        {
            ResearcherList.Visibility = Visibility.Collapsed;
            Controller.Visibility = Visibility.Collapsed;

            ReportGrid.Visibility = Visibility.Visible;
        }

        private void ResearcherListClicked(object sender, RoutedEventArgs e)
        {
            ResearcherList.Visibility = Visibility.Visible;
            Controller.Visibility = Visibility.Visible;

            ReportGrid.Visibility = Visibility.Collapsed;
        }
        private void CopyEmails(object sender, RoutedEventArgs e)
        {
            DataGrid activeDataGrid = null;
            TabItem selectedTab = (TabItem)Report.SelectedItem;
            switch (selectedTab.Header.ToString())
            {
                case "Poor":
                    activeDataGrid = Poor;
                    break;
                case "Below Expectations":
                    activeDataGrid = BelowExpectations;
                    break;
                case "Meeting Minimum":
                    activeDataGrid = MeetingMinimum;
                    break;
                case "Star Performers":
                    activeDataGrid = StarPerformancer;
                    break;
                default:
                    break;
            }

            if (activeDataGrid != null)
            {
                string clipboard="";
                foreach (var item in activeDataGrid.Items)
                {
                    string email = (activeDataGrid.Columns[2].GetCellContent(item) as TextBlock)?.Text;
                    clipboard += email + "; ";
                }
                Clipboard.SetText(clipboard);
                MessageBox.Show("Emails copied to clipboard.");
            }
        }
    }
}
