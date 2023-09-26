using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAP_WPF.Entity;
using RAP_WPF.DataSource;

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

        public List<Researcher_Publication> LoadAllRelations()
        {
            return DBAdapter.Relation();
        }
    }
}