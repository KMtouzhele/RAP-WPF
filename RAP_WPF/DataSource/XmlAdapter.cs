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
        private static string filePath = "C:\\Users\\kaimol\\source\\repos\\RAP-WPF\\RAP_WPF\\DataSource\\Fundings_Rankings.xml";
        public static List<Publication> LoadAll()
        {
            Debug.WriteLine("Start loading");
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
                Debug.WriteLine("Funding is " + publication.Funding);
                XmlNodeList researcherNode = node.SelectNodes("Researchers/staff_id");
                List<string> staff = new List<string>();
                foreach (XmlNode rnode in researcherNode)
                {
                    Debug.WriteLine("LoadAll ID "+ rnode.InnerText);
                    staff.Add(rnode.InnerText);
                }
                publication.Staff = staff.ToList();
                

                projects.Add(publication);

            }

            Debug.WriteLine("Projects List:");
            foreach (var project in projects)
            {
                Debug.WriteLine($"Project ID: {project.Id}, Funding: {project.Funding}, Year: {project.Year}");
                Debug.WriteLine("Researchers:");
                foreach (var researcher in project.Staff)
                {
                    Debug.WriteLine($" - {researcher}");
                }
            }

            return projects;
            
        }

        /*private static string filePath = "C:\\Users\\kaimol\\source\\repos\\RAP-WPF4\\RAP_WPF\\DataSource\\PublicationRecord.xml";
        public static List<Publication> LoadAll()
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(filePath);

            List<Publication> publications = new List<Publication>();

            XmlNodeList publicationNode = xml.SelectNodes("/Publications/Publication");

            foreach (XmlNode node in publicationNode)
            {
                Publication publication = new Publication();
                publication.Id = node.Attributes["id"].Value;
                publication.Title = node["Title"].InnerText;
                publication.Year = Int32.Parse(node["Year"].InnerText);
                publication.Available = DateTime.Parse(node["AvailableFrom"].InnerText);
                publication.Type = (AllEnum.OutputType)Enum.Parse(typeof(AllEnum.OutputType), node["Type"].InnerText);
                publication.Ranking = (AllEnum.OutputRanking)Enum.Parse(typeof(AllEnum.OutputRanking), node["Ranking"].InnerText);

                publication.Author = node["Author"].InnerText;
            }

            return publications;
        }*/
    }
}
