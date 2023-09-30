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

        public List<Publication> LoadPubBy3Year(Researcher researcher, List<Publication> publications)
        {
            var selectedpub = from pub in publications
                              where pub.Author.Contains(researcher.GivenName + " " + researcher.FamilyName)
                              where pub.Year >= DateTime.Today.Year - 3
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

        public List<Publication> LoadPubFunding(Researcher researcher, List<Publication> publications)
        {
            var selectedpub = from pub in publications
                              where pub.Staff.Contains(researcher.Id.ToString())
                              where pub.Available >= researcher.UtasStart
                              select pub;
            Debug.WriteLine("Selected Publications for Researcher " + researcher.Id + ":");
            foreach (var pub in selectedpub)
            {
                Debug.WriteLine($"Publication ID: {pub.Id}, Funding: {pub.Funding}, Available: {pub.Available}"); 
}


            return (List<Publication>)selectedpub.ToList();
        }

        public List<Researcher_Publication> LoadAllRelations()
        {
            return DBAdapter.Relation();
        }
    }
}