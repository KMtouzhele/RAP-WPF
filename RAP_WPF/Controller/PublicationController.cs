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
    class PublicationController
    {

        public List<Researcher_Publication> LoadDOIsFor(int id, List<Researcher_Publication> relations)
        {
            var rp = from relation in relations
                     where relation.Id == id
                     select relation;

            return (List<Researcher_Publication>)rp.ToList();
        }

        public List<Publication> LoadAllPublications()
        {
            return DBAdapter.AllPublications();
        }

        public List<Publication> LoadPubCountFor(string givenname, string familyname, List<Publication> publications)
        {
            string name = givenname + " " + familyname;
            var selectedpub = from pub in publications
                              where pub.Author.Contains(name)
                              select pub;
            return (List<Publication>)selectedpub.ToList();
        }

        public double ThreeYearAverage(Researcher researcher)
        {
            List<Publication> publications = DBAdapter.AllPublications();
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

            var selectedpub = from pub in publications
                              where pub.Author.Contains(researcher.GivenName + " " + researcher.FamilyName)
                              where pub.Year >= DateTime.Today.Year - 3
                              select pub;
            List<Publication> selectedpublications = (List<Publication>)selectedpub.ToList();
            return selectedpublications.Count / (researcher.Tenure * expectednumber);
        }

        public List<Publication> LoadPubSinceCommence(Researcher researcher, List<Publication> publications)
        {
            var selectedpub = from pub in publications
                              where pub.Available >= researcher.UtasStart
                              select pub;
            return (List<Publication>)selectedpub.ToList();
        }

        public float FundingPerformance(Researcher researcher)
        {

            List<Publication> pubFunding = XmlAdapter.LoadAll();
            var validpub = from pub in pubFunding
                            where pub.Staff.Contains(researcher.Id.ToString())
                            where pub.Year >= researcher.UtasStart.Year
                            select pub;


            float totalFunding = validpub.Sum(pub => pub.Funding);
            float performancebyfunding = totalFunding / researcher.Tenure;
            return performancebyfunding;

        }

        //To load publications by specific researcher
        public List<Publication> LoadPublicationFor(Researcher researcher)
        {
            List<Publication> AllPub = DBAdapter.AllPublications();
            List<Researcher_Publication> AllResearcherPublication = DBAdapter.Relation();
            var relation = from r_p in AllResearcherPublication
                           where researcher.Id == r_p.Id
                           select r_p;

            List<string> researcherDOIs = relation.Select(r_p => r_p.DOI).ToList();

            var p = from pub in AllPub
                    where researcherDOIs.Contains(pub.DOI)
                    select pub;

            return (List<Publication>)p.ToList();
        }



        public List<Researcher_Publication> LoadAllRelations()
        {
            return DBAdapter.Relation();
        }
    }
}