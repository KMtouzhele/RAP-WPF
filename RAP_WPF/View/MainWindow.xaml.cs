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
using RAP_WPF.Controller;
using RAP_WPF.WhiteBoxTest;

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
            List<Researcher> researchers = ResearcherController.LoadAllResearchers();
            ResearcherList.ItemsSource = researchers;

            //Poor report
            //Poor.ItemsSource = WhiteBox.AddFakeStaffByPeformance(ResearcherController.LoadResearcherByPerformance(0, 0.7));
            Poor.ItemsSource = ResearcherController.LoadResearcherByPerformance(0, 0.7);

            //Below expectations report
            BelowExpectations.ItemsSource = ResearcherController.LoadResearcherByPerformance(0.7, 1.1);

            //Meeting Minimun report
            var meetingminimun = ResearcherController.LoadResearcherByPerformance(1.1, 2);
            var r3 = from researcher in meetingminimun
                    orderby researcher.Performance descending
                    select researcher;
            MeetingMinimum.ItemsSource = r3.ToList();
            
            //Star performance report
            var starperformancer = ResearcherController.LoadResearcherByPerformance(2, 999);
            var r4 = from researcher in starperformancer
                    orderby researcher.Performance descending
                    select researcher;
            //StarPerformancer.ItemsSource = WhiteBox.AddFakeStaffByPeformance(r4.ToList());
            StarPerformancer.ItemsSource = r4.ToList();

            //Every time launched window, white box test code could be activated and could see the outcome via OutPut window
            //WhiteBox.TestFilterByName();
        }


        //Double click to open a new window for researcher details
        public void UpdateDataGridItemsSource(IEnumerable<object> items)
        {
            ResearcherList.ItemsSource = items;
        }

        private void SelectResearcher(object sender, MouseButtonEventArgs e)
        {
            Researcher selectedresearcher = (Researcher)ResearcherList.SelectedItem;

            if (selectedresearcher != null)
            {
                ResearcherDetail researcherdetail = new ResearcherDetail(selectedresearcher);
                researcherdetail.Show();
            }
        }

        //Click report to show Report and collapse researcher list
        private void ReportClicked(object sender, RoutedEventArgs e)
        {
            ResearcherList.Visibility = Visibility.Collapsed;
            ResearcherListFilterControl.Visibility = Visibility.Collapsed;

            ReportGrid.Visibility = Visibility.Visible;
        }

        //Click report to show researcher list and collapse report
        private void ResearcherListClicked(object sender, RoutedEventArgs e)
        {
            ResearcherList.Visibility = Visibility.Visible;
            ResearcherListFilterControl.Visibility = Visibility.Visible;

            ReportGrid.Visibility = Visibility.Collapsed;
        }

        //Click to copy emails
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
                bool empty = true;
                foreach (var item in activeDataGrid.Items)
                {
                    string email = (activeDataGrid.Columns[2].GetCellContent(item) as TextBlock)?.Text;
                    if (email != null)
                    {
                        clipboard += email + "; ";
                        empty = false;
                    }
                }
                if (empty == false)
                {
                    Clipboard.SetText(clipboard);
                    MessageBox.Show("Emails copied to clipboard.");
                }
                else
                {
                    MessageBox.Show("No emails in the datagrid.");
                }
            }
        }

        
    }
}
