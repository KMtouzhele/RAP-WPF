using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAP_WPF.DataSource;

namespace RAP_WPF.Entity
{
    public class Publication
    {
        public string Id { get; set; }
        public string DOI { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public AllEnum.OutputRanking Ranking { get; set; }
        public int Year { get; set; }
        public AllEnum.OutputType Type { get; set; }
        public string CiteAs { get; set; }
        public DateTime Available { get; set; }
        public int Funding { get; set; }
        public List<string> Staff { get; set; }
        public int Age => GetAge(Available);
    

        public Publication()
        {
            DOI = "";
            Title = "";
            Author = "";
            CiteAs = "";
        }


        //METHOD: GetAge
        int GetAge(DateTime avaliable)
        {
            TimeSpan span = DateTime.Today - avaliable;
            double days = span.TotalDays;
            return (int)days;
        }



        public override string ToString()
        {
            return DOI + " " + Title + " " + Author + " " + Year + " " + Type + " " + CiteAs + " " + Available + " " + Age;
        }

    }
}