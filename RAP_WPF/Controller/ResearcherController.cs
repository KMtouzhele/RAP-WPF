using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAP_WPF.Entity;
using RAP_WPF.DataSource;
using System.Diagnostics;
using RAP_WPF.WhiteBoxTest;

namespace RAP_WPF.Controller
{
    public class ResearcherController
    {
        public static List<Researcher> LoadAllResearchers()
        {
            return DBAdapter.AllResearchers();
        }

        public static List<Researcher> FilterByLevel(List<Researcher> researchers, AllEnum.EmploymentLevel level)
        {
            var r = from researcher in researchers
                    where researcher.Level == level
                    select researcher;
            return (List<Researcher>)r.ToList();
        }

        public static List<Researcher> FilterByName(List<Researcher> researchers, string input)
        {
            string lowerinput = input.ToLower();
            var r = from researcher in researchers
                    where researcher.GivenName.ToLower().Contains(lowerinput) || researcher.FamilyName.ToLower().Contains(lowerinput)
                    select researcher;
            return (List<Researcher>)r.ToList();
        }


        public static string LoadSupervision(Staff staff)
        {
            List<Student> students = LoadAllResearchers().OfType<Student>().ToList();
            var s = from student in students
                    where student.Supervisor == staff.Id
                    select student;
            List<Student> supervisions = (List<Student>)s.ToList();
            string supervisionnames = "";
            if (supervisions.Count > 0)
            {
                foreach (Student supervision in supervisions)
                {
                    supervisionnames += supervision.NameShown + "\n";
                }
            }
            else supervisionnames = "No supervisions found.";
            return supervisionnames;
        }
        public static int CalculateSupervision(Staff staff)
        {
            List<Student> students = LoadAllResearchers().OfType<Student>().ToList();
            var s = from student in students
                    where student.Supervisor == staff.Id
                    select student;
            List<Student> supervisions = (List<Student>)s.ToList();

            //WhiteBox.SupervisionPreview(supervisions);
            return supervisions.Count;
        }

        public static double ThreeYearAverage(Researcher researcher)
        {
            List<Publication> publications = PublicationController.LoadAllPublications();

            var selectedpub = from pub in publications
                              where pub.Author.Contains(researcher.GivenName + " " + researcher.FamilyName)
                              where pub.Year >= DateTime.Today.Year - 3
                              select pub;
            List<Publication> selectedpublications = (List<Publication>)selectedpub.ToList();
            return (double)selectedpublications.Count / 3;
        }

        public static double Performance(Staff researcher)
        {
            double expectednumber = 1;
            switch (researcher.Level)
            {
                case AllEnum.EmploymentLevel.A:
                    expectednumber = 0.5;
                    break;
                case AllEnum.EmploymentLevel.B:
                    expectednumber = 1;
                    break;
                case AllEnum.EmploymentLevel.C:
                    expectednumber = 2;
                    break;
                case AllEnum.EmploymentLevel.D:
                    expectednumber = 3.2;
                    break;
                case AllEnum.EmploymentLevel.E:
                    expectednumber = 4;
                    break;
                default:
                    break;
            }

            double performance = researcher.ThreeYearAverage / expectednumber;
            return performance;
        }

        //To load the researcher list based on the calculated performance
        public static List<StaffByPerformance> LoadResearcherByPerformance(double bottom, double cap)
        {
            List<Staff> staff = LoadAllResearchers().OfType<Staff>().ToList();
            var r = from researcher in staff
                    where researcher.Performance >= bottom && researcher.Performance <= cap
                    orderby researcher.Performance
                    select new StaffByPerformance
                    {
                        Name = researcher.NameShown.ToString(),
                        Email = researcher.Email.ToString(),
                        Performance = researcher.Performance.ToString("P") 
                    };
            return (List<StaffByPerformance>)r.ToList();
        }

        //To load previous positions
        public static string LoadPreviousPosition(Researcher researcher)
        {
            //Debug.WriteLine("Loading positions...");
            List<Position> AllPosition = DBAdapter.AllPosition();
            DateTime currentDate = DateTime.Today;
            var p = from position in AllPosition
                    where position.Id == researcher.Id
                    orderby position.End
                    select position;
            List<Position> previousposition = (List<Position>)p.ToList();
            string positions = "";
            if(previousposition.Count <= 1)
            {
                positions = "No previous positions found.";
            }
            else
            {
                for (int i = 0; i < previousposition.Count - 1; i++)
                {
                    //Debug.WriteLine($"Position {i + 1}: Id={previousposition[i].Id}, Level={previousposition[i].Level}, Start={previousposition[i].Start}");
                    positions += previousposition[i].Title + ": \n"+ previousposition[i].Start.ToString("yyyy-MM-dd") + " to " + previousposition[i].End.ToString("yyyy-MM-dd") + "\n" ;
                }
            }
            
            //Debug.WriteLine($"Final positions: {positions}");
            return positions;
        }
        
        public static string LoadSupervisor(Student researcher)
        {
            List<Researcher> AllResearcher = LoadAllResearchers();
            var s = from supervisor in AllResearcher
                    where researcher.Supervisor == supervisor.Id
                    select supervisor;
            List<Researcher> supervisors = (List<Researcher>)s.ToList();
            string supervisorname = "";
            foreach (Researcher supervisor in supervisors)
            {
                supervisorname = supervisor.NameShown;
            }
            return supervisorname;

        }

        public class StaffByPerformance
        {
            public string Name { get; set; }
            public string Performance { get; set; }
            public string Email { get; set; }
        }
    }
}