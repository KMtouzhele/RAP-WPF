using RAP_WPF.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace RAP_WPF.DataSource
{
    public static class XmlAdapter
    {
        private static string filePath = "C:\\Users\\kaimol\\source\\repos\\RAP-WPF4\\RAP_WPF\\DataSource\\PublicationRecord.xml";
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
        }
    }
}
