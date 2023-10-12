using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAP_WPF.Entity;
using RAP_WPF.Controller;
using System.Diagnostics;

namespace RAP_WPF.WhiteBoxTest
{
    class WhiteBox
    {
        //All the code outcome in this file can be checked in OutPut wimdow.

        //Create Fake Researcher and Publication
        public static List<Researcher> CreateFakeResearcher()
        {
            List<Researcher> fakeResearchers = new List<Researcher>();
            Student student1 = new Student { Id = 999, FamilyName = "Test1", GivenName = "Test1", Supervisor = 100 };
            Student student2 = new Student { Id = 998, FamilyName = "Test2", GivenName = "Test2", Supervisor = 100 };
            Student student3 = new Student { Id = 997, FamilyName = "Test3", GivenName = "Test3", Supervisor = 101 };
            Staff staff1 = new Staff { Id = 100, FamilyName = "Staff1", GivenName = "Staff1" };
            Staff staff2 = new Staff { Id = 101, FamilyName = "Staff1", GivenName = "Staff1" };
            fakeResearchers.Add(student1);
            fakeResearchers.Add(student2);
            fakeResearchers.Add(student3);
            fakeResearchers.Add(staff1);
            fakeResearchers.Add(staff2);
            return fakeResearchers;
        } 


        //FilterByName Test: when multiple types of input.
        public static void TestFilterByName()
        {
            List<Researcher> researchers = ResearcherController.FilterByName(ResearcherController.LoadAllResearchers(), "John");
            Debug.WriteLine(researchers.Count + " researchers selected. Name shown as below");
            foreach (Researcher researcher in researchers)
            {
                Debug.WriteLine(researcher.NameShown);
            }
        }

        public static void SupervisionPreview(List<Student> students)
        {
            foreach (Student s in students)
            {
                Debug.WriteLine(s.NameShown);
            }
        }

        public static void PublicationPreview(List<Publication> publications)
        {
            foreach(Publication pub in publications)
            {
                Debug.WriteLine(pub.Year + " " + pub.Title);
            }
        }

        public static List<Publication> AddFakePublication(List<Publication> publications)
        {
            Publication pub1 = new Publication { DOI = "Test1", Year = 2023, Title = "Test1" };
            Publication pub2 = new Publication { DOI = "Test2", Year = 2022, Title = "Test2" };
            publications.Add(pub1);
            publications.Add(pub2);
            return publications;
        }

        public static List<ResearcherController.StaffByPerformance> AddFakeStaffByPeformance(List<ResearcherController.StaffByPerformance> performances)
        {
            ResearcherController.StaffByPerformance performance = new ResearcherController.StaffByPerformance {Name="Test", Performance="200%", Email="Test1@test.com"};
            performances.Add(performance);
            return performances;
        }

    }
}


