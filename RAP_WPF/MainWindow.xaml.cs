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

            List<Researcher> researchers = DBAdapter.AllResearchers();
            ResearcherList.ItemsSource = researchers;
 
        }

        private void filtered(object sender, SelectionChangedEventArgs e)
        {
                ComboBoxItem selecteditem = (ComboBoxItem)FilterByLevel.SelectedItem;
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
                ResearcherController researcherController = new ResearcherController();
                List<Researcher> filteredresearchers = researcherController.FilterByLevel(DBAdapter.AllResearchers(), selectedlevel);
                ResearcherList.ItemsSource = filteredresearchers;

        }

        private void Search(object sender, RoutedEventArgs e)
        {
            string input = SearchBox.Text;
            ResearcherController researchercontroller = new ResearcherController();
            List<Researcher> filteredresearchers = researchercontroller.FilterByName(DBAdapter.AllResearchers(), input);
            ResearcherList.ItemsSource = filteredresearchers;
        }

        private void Reset(object sender, RoutedEventArgs e)
        {
            ResearcherList.ItemsSource = DBAdapter.AllResearchers();
            SearchBox.Text = "";
        }

        private void SelectResearcher(object sender, MouseButtonEventArgs e)
        {
            Researcher selectedresearcher = (Researcher)ResearcherList.SelectedItem;
            if (selectedresearcher != null)
            {
                ResearcherDetail researcherdetail = new ResearcherDetail(selectedresearcher);
                researcherdetail.PhotoSource = new BitmapImage(new Uri(selectedresearcher.Photo));
                researcherdetail.NameInDetail = selectedresearcher.NameShown;
                researcherdetail.JobTitle = selectedresearcher.JobTitle;
                researcherdetail.School = selectedresearcher.School;
                researcherdetail.Campus = selectedresearcher.Campus.ToString();
                researcherdetail.Email = selectedresearcher.Email;
                researcherdetail.UtasStart = selectedresearcher.UtasStart.ToString("yyyy-MM-dd");
                researcherdetail.CurrentStart = selectedresearcher.CurrentStart.ToString("yyyy-MM-dd");
                researcherdetail.Tenure = selectedresearcher.Tenure.ToString("0.00") + " years";
                researcherdetail.Q1Percentage = selectedresearcher.Q1Percentage.ToString();
                researcherdetail.ThreeYearAverage = selectedresearcher.ThreeYearAverage.ToString();
                researcherdetail.Show();
            }
        }
           
    }
}
