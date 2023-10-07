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

        public static List<Publication> LoadAllPublications()
        {
            return DBAdapter.AllPublications();
        }

        public static List<Researcher_Publication> LoadAllRelations()
        {
            return DBAdapter.Relation();
        }

        public static float LoadQ1PercentageFor(Researcher researcher)
        {
            List<Publication> publications = PublicationController.LoadAllPublications();
            string name = researcher.GivenName + " " + researcher.FamilyName;
            var selectedpub = from pub in publications
                              where pub.Author.Contains(name)
                              select pub;
            List<Publication> publicationFor = (List<Publication>)selectedpub.ToList();

            int count = publicationFor.Count;
            var q1 = from pub in publicationFor
                     where pub.Ranking == AllEnum.OutputRanking.Q1
                     select pub;
            int q1count = q1.Count();
            float q1percentage = (float)q1count / count;
            return q1percentage;
        }


        public static List<Publication> LoadPubSinceCommence(Researcher researcher)
        {

            List<Publication> publications = PublicationController.LoadAllPublications(); var selectedpub = from pub in publications
                              where pub.Available >= researcher.UtasStart
                              select pub;
            return (List<Publication>)selectedpub.ToList();
        }

        public static float FundingRecieved(Researcher researcher)
        {

            List<Publication> pubFunding = XmlAdapter.LoadAll();
            var validpub = from pub in pubFunding
                           where pub.Staff.Contains(researcher.Id.ToString())
                           where pub.Year >= researcher.UtasStart.Year
                           select pub;

            float totalFunding = validpub.Sum(pub => pub.Funding);
            return totalFunding;
        }

        public static float FundingPerformance(Researcher researcher)
        {
            float totalFunding = FundingRecieved(researcher);
            float performancebyfunding = totalFunding / researcher.Tenure;
            return performancebyfunding;
        }

        //To load publications by specific researcher
        public static List<Publication> LoadPublicationFor(Researcher researcher)
        {
            List<Publication> AllPub = LoadAllPublications();
            List<Researcher_Publication> AllResearcherPublication = LoadAllRelations();
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

        public static string LoadCumulativeNumber(Researcher researcher)
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

        
    }
}