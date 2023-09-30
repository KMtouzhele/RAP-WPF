using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAP_WPF.Entity;
using MySql.Data.MySqlClient;

namespace RAP_WPF.DataSource
{
    abstract class DBAdapter
    {
        private const string db = "kit206";
        private const string user = "kit206";
        private const string pass = "kit206";
        private const string server = "alacritas.cis.utas.edu.au";

        private static MySqlConnection conn = null;

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        private static MySqlConnection GetConnection()
        {
            if (conn == null)
            {
                string connectionString = String.Format("Database={0};Data Source={1};User Id={2};Password={3}", db, server, user, pass);
                conn = new MySqlConnection(connectionString);
            }
            return conn;
        }

        public static List<Publication> AllPublications()
        {
            List<Publication> publications = new List<Publication>();

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select doi, title, ranking, authors, year, type, cite_as, available from publication", conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    publications.Add(new Publication
                    {
                        DOI = rdr.GetString(0),
                        Title = rdr.GetString(1),
                        Ranking = ParseEnum<AllEnum.OutputRanking>(rdr.GetString(2)),
                        Author = rdr.GetString(3),
                        Year = rdr.GetInt32(4),
                        Type = ParseEnum<AllEnum.OutputType>(rdr.GetString(5)),
                        CiteAs = rdr.GetString(6),
                        Available = rdr.GetDateTime(7)
                    });
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error connecting to database: " + e);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return publications;
        }


        public static List<Researcher> AllResearchers()
        {
            List<Researcher> researchers = new List<Researcher>();

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select id, type, given_name, family_name, title, unit, campus, IFNULL(email , ' '), IFNULL(photo , ' '), degree, IFNULL(supervisor_id, '0'), IFNULL(level , 'Student'), utas_start, current_start from researcher", conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    researchers.Add(new Researcher
                    {
                        Id = rdr.GetInt32(0),
                        Type = ParseEnum<AllEnum.ReseacherType>(rdr.GetString(1)),
                        GivenName = rdr.GetString(2),
                        FamilyName = rdr.GetString(3),
                        Title = ParseEnum<AllEnum.Title>(rdr.GetString(4)),
                        School = rdr.GetString(5),
                        //Campus = ParseEnum<AllEnum.Campus>(rdr.GetString(6)),
                        Email = rdr.GetString(7),
                        Photo = rdr.GetString(8),
                        Level = ParseEnum<AllEnum.EmploymentLevel>(rdr.GetString(11)),
                        UtasStart = rdr.GetDateTime(12),
                        CurrentStart = rdr.GetDateTime(13),
                        Supervisor = Int32.Parse(rdr.GetString(10)),
                    });
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error connecting to database: " + e);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return researchers;
        }

        public static List<Researcher_Publication> Relation()
        {
            List<Researcher_Publication> relation = new List<Researcher_Publication>();

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select researcher_id, doi from researcher_publication", conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    relation.Add(new Researcher_Publication
                    {
                        Id = rdr.GetInt32(0),
                        DOI = rdr.GetString(1),
                    });
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error connecting to database: " + e);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return relation;
        }
    }
}