using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAP_WPF.Entity
{
    public class Staff : Researcher
    {
        public float ThreeYearAverage { get; set; }
        public float Performance { get; set; }

        //METHOD: GetThreeYearAverage
        public static float GetThreeYearAverage(List<Publication> publications)
        {
            int count = 0;
            for (int i = 0; i < publications.Count; i++)
            {
                if (publications[i].Avaliable > DateTime.Today.AddYears(-3))
                {
                    count = count + 1;
                }
            }
            float threeyearaverage = count / 3;
            return threeyearaverage;
        }

    }
}