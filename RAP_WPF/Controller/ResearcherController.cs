using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAP_WPF.Entity;
using RAP_WPF.DataSource;

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
    }
}