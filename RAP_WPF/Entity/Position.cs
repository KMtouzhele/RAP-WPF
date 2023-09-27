using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAP_WPF.Entity
{
    public class Position
    {
        public AllEnum.EmploymentLevel Level { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Title { get; set; }
        public string ToTitle { get; set; }


        //METHOD: GetToTitle
        public string GetToTitle(AllEnum.EmploymentLevel level)
        {
            switch (level)
            {
                case AllEnum.EmploymentLevel.A:
                    return "Research Associate";
                case AllEnum.EmploymentLevel.B:
                    return "Lecturer";
                case AllEnum.EmploymentLevel.C:
                    return "Assistant Professor";
                case AllEnum.EmploymentLevel.D:
                    return "Associate Professor";
                case AllEnum.EmploymentLevel.E:
                    return "Professor";
                case AllEnum.EmploymentLevel.Student:
                    return "Student";
                default:
                    return "No title found";
            }
        }

        public Position()
        {
            Level = AllEnum.EmploymentLevel.Student;
        }
        
        public override string ToString()
        {
            return Level + " " + Start + " " + End + " " + Title;
        }
    }
}