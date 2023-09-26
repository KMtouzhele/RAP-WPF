using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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