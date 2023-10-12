 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAP_WPF.Entity
{
    public class Researcher_Publication
    {
        public int Id { get; set; }
        public string DOI { get; set; }

        public Researcher_Publication()
        {
            Id = -1;
            DOI = "";
        }
    }
} 