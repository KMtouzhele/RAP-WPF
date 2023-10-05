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
                    orderby pub.Year descending, pub.Title
                    select pub;

            return (List<Publication>)p.ToList();
        }

        public string LoadCumulativeNumber(Researcher researcher)
        {
            string cumulative = "";
            List<Publication> publications = LoadPublicationFor(researcher);
            for(int i = researcher.UtasStart.Year; i<DateTime.Today.Year +1; i++)
            {
                var p = from pub in publications
                        where pub.Year == i
                        select pub;
                List<Publication> Cumulative = (List<Publication>)p.ToList();
                cumulative += i + ": " + Cumulative.Count +" in total \n";
            }
            return cumulative;
        }

        public List<Researcher_Publication> LoadAllRelations()
        {
            return DBAdapter.Relation();
        }
    }
}