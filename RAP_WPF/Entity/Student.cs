using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAP_WPF.Entity
{
    public class Student : Researcher
    {
        public string Degree { get; set; }
        public int Supervisor { get; set; }

        public Student() : base()
        {
            
        }
    }
}