using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAP_WPF.DataSource;
using RAP_WPF.Controller;
using System.Diagnostics;

namespace RAP_WPF.Entity
{
    public class Researcher
    {
        public int Id { get; set; }
        public AllEnum.ReseacherType Type { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public AllEnum.Title Title { get; set; }
        public string School { get; set; }
        public AllEnum.Campus Campus { get; set; }
        public AllEnum.EmploymentLevel Level { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
        public DateTime UtasStart { get; set; }
        public DateTime CurrentStart { get; set; }
        public int Supervisor { get; set; }

        public string JobTitle
        {
            get
            {
                Position position = new Position();
                return position.GetToTitle(Level);
            }
        }

        public string NameShown
        {
            get { return FamilyName + ", " + GivenName + " (" + Title + ") "; }
        }

        public float Tenure
        {
            get
            {
                TimeSpan period = DateTime.Today - UtasStart;
                float tenure = (float)period.TotalDays / 365;
                return tenure;
            }
        }
        public float Q1Percentage
        {
            get
            {
                PublicationController publicationcontroller = new PublicationController();
                List<Publication> publications = publicationcontroller.LoadPubCountFor(GivenName, FamilyName, DBAdapter.AllPublications());
                int count = publications.Count;
                var q1 = from pub in publications
                         where pub.Ranking == AllEnum.OutputRanking.Q1
                         select pub;
                int q1count = q1.Count();
                float q1percentage = (float)q1count / count;
                return q1percentage;

            }
        }

        public float ThreeYearAverage
        {
            get
            {
                PublicationController publicationcontroller = new PublicationController();
                List<Publication> publications = publicationcontroller.LoadPubBy3Year(this, DBAdapter.AllPublications());
                int threeyearcount = publications.Count;
                float threeyearaverage = (float)threeyearcount / 3;
                return threeyearaverage;
            }
        }

        public float PerformanceByPublicaton
        {
            get
            {
                PublicationController publicationcontroller = new PublicationController();
                List<Publication> publications = publicationcontroller.LoadPubSinceCommence(this, DBAdapter.AllPublications());
                float performancebypublication = publications.Count / Tenure;
                return performancebypublication;
            }
        }

        public float PerformanceByFunding //PROBLEM WITH THIS ATTRIBUTE
        {
            get
            {
                PublicationController publicationcontroller = new PublicationController();
                List<Publication> publications = publicationcontroller.LoadPubFunding(this, XmlAdapter.LoadAll());

                int totalfunding = publications.Sum(pub => pub.Funding);

                float performancebyfunding = totalfunding / Tenure;
                return performancebyfunding;
            }
        }

        public List<Researcher> Supervision
        {
            get
            {
                ResearcherController researcherController = new ResearcherController();
                List<Researcher> researchers = researcherController.LoadSupervision(this, DBAdapter.AllResearchers());
                return researchers;
            }
        }

        public string StudentNames
        {
            get
            {
                string names = "";
                for(int i = 0; i< Supervision.Count; i++)
                {
                    names = names + Supervision[i].NameShown +"\n";
                }
                return names;
            }
        }
        public Researcher()
        {
            Id = -1;
            GivenName = "";
            FamilyName = "";
            School = "";

        }



        public override string ToString()
        {
            return FamilyName + ", " + GivenName + " (" + Title + ")" + " Level: " + Level;
        }
    }
}