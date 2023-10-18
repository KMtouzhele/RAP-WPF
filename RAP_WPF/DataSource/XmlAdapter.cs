using RAP_WPF.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Diagnostics;

namespace RAP_WPF.DataSource
{
    public static class XmlAdapter
    {
        private static string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataSource\\Fundings_Rankings.xml");
        public static List<Publication> LoadAll()
        {
            /*Debug.WriteLine("Start loading");*/
            XmlDocument xml = new XmlDocument();
            xml.Load(filePath);

            List<Publication> projects = new List<Publication>();

            XmlNodeList projectNode = xml.SelectNodes("/Projects/Project");
            
            foreach (XmlNode node in projectNode)
            {
                Publication publication = new Publication();
                publication.Id = node.Attributes["id"].Value;
                publication.Funding = Int32.Parse(node["Funding"].InnerText);
                publication.Year = Int32.Parse(node["Year"].InnerText);
                /*Debug.WriteLine("Funding is " + publication.Funding);*/
                XmlNodeList researcherNode = node.SelectNodes("Researchers/staff_id");
                List<string> staff = new List<string>();
                foreach (XmlNode rnode in researcherNode)
                {
                    /*Debug.WriteLine("LoadAll ID "+ rnode.InnerText);*/
                    staff.Add(rnode.InnerText);
                }
                publication.Staff = staff.ToList();
                

                projects.Add(publication);

            }

            return projects;
            
        }

    }
}
