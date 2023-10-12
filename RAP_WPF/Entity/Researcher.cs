 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public string NameShown { get; set; }
        public string JobTitle{ get; set; }


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
                return PublicationController.LoadQ1PercentageFor(this);
            }
        }



        public override string ToString()
        {
            return FamilyName + ", " + GivenName + " (" + Title + ")" + " Level: " + Level;
        }
    }
}