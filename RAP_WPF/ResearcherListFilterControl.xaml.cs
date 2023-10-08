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
using RAP_WPF.Controller;
using RAP_WPF.Entity;

namespace RAP_WPF
{
    /// <summary>
    /// Interaction logic for ResearcherListFilterControl.xaml
    /// </summary>
    public partial class ResearcherListFilterControl : UserControl
    {
        private MainWindow mainWindow;
        public ResearcherListFilterControl()
        {
            InitializeComponent();
            mainWindow = (MainWindow)Application.Current.MainWindow;
        }
        //Process the list with Level filter and Name searching at the same time
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
                List<Researcher> filteredresearchersByLevel = ResearcherController.FilterByLevel(ResearcherController.LoadAllResearchers(), selectedlevel);
                if (SearchBox.Text != null)
                {
                    string input = SearchBox.Text;
                    result = ResearcherController.FilterByName(filteredresearchersByLevel, input);
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
                    result = ResearcherController.FilterByName(ResearcherController.LoadAllResearchers(), input);
                }
                else
                {
                    result = ResearcherController.LoadAllResearchers();
                }
            }
            return result;

        }

        //ComboBox call the function above
        private void filtered(object sender, SelectionChangedEventArgs e)
        {
            mainWindow.UpdateDataGridItemsSource(FilterAndDisplayResults());
        }

        //SearchBox call the function above
        private void Search(object sender, RoutedEventArgs e)
        {
            mainWindow.UpdateDataGridItemsSource(FilterAndDisplayResults());
        }


        //Click to show the default researcher list
        private void Reset(object sender, RoutedEventArgs e)
        {
            mainWindow.UpdateDataGridItemsSource(ResearcherController.LoadAllResearchers());
            SearchBox.Text = "";
            FilterByLevel.SelectedItem = null;
        }
    }
}
