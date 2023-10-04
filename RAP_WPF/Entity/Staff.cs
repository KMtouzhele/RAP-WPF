using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAP_WPF.Controller;
using RAP_WPF.Entity;
using RAP_WPF.DataSource;

namespace RAP_WPF.Entity
{
    public class Staff : Researcher
    {
        public float PerformanceByFunding
        {
            get
            {
                PublicationController publicationcontroller = new PublicationController();
                List<Publication> publications = XmlAdapter.LoadAll();

                int totalfunding = publications.Sum(pub => pub.Funding);

                float performancebyfunding = totalfunding / Tenure;
                return performancebyfunding;
            }
        }


        public double ThreeYearAverage
        {
            get
            {
                ResearcherController researcherController = new ResearcherController();
                double threeyearaverage = researcherController.ThreeYearAverage(this);
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
        public double Performance
        {
            get
            {
                ResearcherController researcherController = new ResearcherController();
                double performance = researcherController.Performance(this);
                return performance;
            }
        }
    }
}