using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAP_WPF.Entity
{
    public class Publication
    {
        public string DOI { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public AllEnum.OutputRanking Ranking { get; set; }
        public int Year { get; set; }
        public AllEnum.OutputType Type { get; set; }
        public string CiteAs { get; set; }
        public DateTime Avaliable { get; set; }
        public int Age => GetAge(Avaliable);

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
            return DOI + " " + Title + " " + Author + " " + Year + " " + Type + " " + CiteAs + " " + Avaliable + " " + Age;
        }

    }
}