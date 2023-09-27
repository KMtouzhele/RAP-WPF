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

        public List<Researcher> LoadResearcherDetials(List<Researcher> researchers, int id)
        {
            var r = from researcher in researchers
                    where researcher.Id == id
                    select researcher;
            return (List<Researcher>)r.ToList();
        }
    }
}