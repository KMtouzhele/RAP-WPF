using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAP_WPF.Entity;
using RAP_WPF.DataSource;
using System.Diagnostics;

namespace RAP_WPF.Controller
{
    public class ResearcherController
    {
        public List<Researcher> LoadAllResearchers()
        {
            return DBAdapter.AllResearchers();
        }

        public List<Researcher> FilterByLevel(List<Researcher> researchers, AllEnum.EmploymentLevel level)
        {
            var r = from researcher in researchers
                    where researcher.Level == level
                    select researcher;
            return (List<Researcher>)r.ToList();
        }

        public List<Researcher> FilterByName(List<Researcher> researchers, string input)
        {
            string lowerinput = input.ToLower();
            var r = from researcher in researchers
                    where researcher.GivenName.ToLower().Contains(lowerinput) || researcher.FamilyName.ToLower().Contains(lowerinput)
                    select researcher;
            return (List<Researcher>)r.ToList();
        }

        public List<Researcher> LoadResearcherDetials(List<Researcher> researchers, Researcher researcher)
        {
            var r = from researcher1 in researchers
                    where researcher1.Id == researcher.Id
                    select researcher1;
            return (List<Researcher>)r.ToList();
        }

        public List<Researcher> LoadSupervision(Researcher researcher, List<Researcher> researchers)
        {
            var s = from student in researchers
                    where student.Supervisor == researcher.Id
                    select student;
            return (List<Researcher>)s.ToList();
        }

        public double ThreeYearAverage(Researcher researcher)
        {
            List<Publication> publications = DBAdapter.AllPublications();

            var selectedpub = from pub in publications
                              where pub.Author.Contains(researcher.GivenName + " " + researcher.FamilyName)
                              where pub.Year >= DateTime.Today.Year - 3
                              select pub;
            List<Publication> selectedpublications = (List<Publication>)selectedpub.ToList();
            return selectedpublications.Count / 3;
        }
        public double Performance(Researcher researcher)
        {
            double expectednumber = -1;
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
        public List<Researcher> LoadResearcherByPerformance(double bottom, double cap)
        {
            List<Researcher> researchers = DBAdapter.AllResearchers();
            var r = from researcher in researchers
                    where researcher.Performance >= bottom && researcher.Performance <= cap
                    select researcher;
            return (List<Researcher>)r.ToList();

        }

        //To load previous positions
        public string LoadPreviousPosition(Researcher researcher)
        {
            Debug.WriteLine("Loading positions...");
            List<Position> AllPosition = DBAdapter.AllPosition();
            DateTime currentDate = DateTime.Today;
            var p = from position in AllPosition
                    where position.Id == researcher.Id
                    orderby position.End
                    select position;
            List<Position> previousposition = (List<Position>)p.ToList();
            string positions = "";
            for(int i=0; i<previousposition.Count-1; i++)
            {
                //Debug.WriteLine($"Position {i + 1}: Id={previousposition[i].Id}, Level={previousposition[i].Level}, Start={previousposition[i].Start}");
                positions +=previousposition[i].Start.ToString("yyyy/MM/dd") +" - "+previousposition[i].End.ToString("yyyy/MM/dd") + " "+ previousposition[i].Title+"\n";
            }
            //Debug.WriteLine($"Final positions: {positions}");
            return positions;
        }
    }
}