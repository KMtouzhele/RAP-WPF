using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAP_WPF.Entity;
using MySql.Data.MySqlClient;
using System.Diagnostics;

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

                MySqlCommand cmd = new MySqlCommand("select * from publication", conn);
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
            Position position = new Position();
            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select id, type, given_name, family_name, title, unit, campus, IFNULL(email , ' '), IFNULL(photo , ' '), degree, IFNULL(supervisor_id, '0'), IFNULL(level , 'Student'), utas_start, current_start from researcher", conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Researcher researcher=new Researcher();
                    researcher.Id = rdr.GetInt32(0);
                    researcher.Type = ParseEnum<AllEnum.ReseacherType>(rdr.GetString(1));
                    researcher.GivenName = rdr.GetString(2);
                    researcher.FamilyName = rdr.GetString(3);
                    researcher.NameShown = rdr.GetString(2) + ", " + rdr.GetString(3) + " (" + rdr.GetString(4) + ")";
                    researcher.Title = ParseEnum<AllEnum.Title>(rdr.GetString(4));
                    researcher.School = rdr.GetString(5);
                    //researcher.Campus = ParseEnum<AllEnum.Campus>(rdr.GetString(6)),
                    researcher.Email = rdr.GetString(7);
                    researcher.Photo = rdr.GetString(8);
                    researcher.Level = ParseEnum<AllEnum.EmploymentLevel>(rdr.GetString(11));
                    researcher.UtasStart = rdr.GetDateTime(12);
                    researcher.CurrentStart = rdr.GetDateTime(13);
                    researcher.JobTitle = position.GetToTitle(ParseEnum<AllEnum.EmploymentLevel>(rdr.GetString(11)));
                    //Debug.WriteLine("CS is "+ rdr.GetDateTime(13).ToString());


                    if (researcher.Type == AllEnum.ReseacherType.Student)
                    {
                        Debug.WriteLine("Load Student");
                        Student student = new Student();
                        student.Id = rdr.GetInt32(0);
                        student.Type = ParseEnum<AllEnum.ReseacherType>(rdr.GetString(1));
                        student.GivenName = rdr.GetString(2);
                        student.FamilyName = rdr.GetString(3);
                        student.NameShown = rdr.GetString(2) + ", " + rdr.GetString(3) + " (" + rdr.GetString(4) + ")";
                        student.Title = ParseEnum<AllEnum.Title>(rdr.GetString(4));
                        student.School = rdr.GetString(5);

                        student.Email = rdr.GetString(7);
                        student.Photo = rdr.GetString(8);
                        student.Level = ParseEnum<AllEnum.EmploymentLevel>(rdr.GetString(11));
                        student.UtasStart = rdr.GetDateTime(12);
                        student.CurrentStart = rdr.GetDateTime(13);
                        student.JobTitle = position.GetToTitle(ParseEnum<AllEnum.EmploymentLevel>(rdr.GetString(11)));
                        student.Degree = rdr.GetString(9);
                        student.Supervisor = rdr.GetInt32(10);
                        researcher = student;
                    }

                    else
                    {
                        Debug.WriteLine("Load Staff");
                        Staff staff = new Staff();
                        staff.Id = rdr.GetInt32(0);
                        staff.Type = ParseEnum<AllEnum.ReseacherType>(rdr.GetString(1));
                        staff.GivenName = rdr.GetString(2);
                        staff.FamilyName = rdr.GetString(3);
                        staff.NameShown = rdr.GetString(2) + ", " + rdr.GetString(3) + " (" + rdr.GetString(4) + ")";
                        staff.Title = ParseEnum<AllEnum.Title>(rdr.GetString(4));
                        staff.School = rdr.GetString(5);

                        staff.Email = rdr.GetString(7);
                        staff.Photo = rdr.GetString(8);
                        staff.Level = ParseEnum<AllEnum.EmploymentLevel>(rdr.GetString(11));
                        staff.UtasStart = rdr.GetDateTime(12);
                        staff.CurrentStart = rdr.GetDateTime(13);
                        staff.JobTitle = position.GetToTitle(ParseEnum<AllEnum.EmploymentLevel>(rdr.GetString(11)));
                        researcher =staff;
                    }
                    researchers.Add(researcher);
                    Debug.WriteLine("Mapped" + researcher.NameShown);
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

        public static List<Position> AllPosition()
        {
            List<Position> positions = new List<Position>();

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select id, level, start, IFNULL(end, CURDATE()) from position", conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    
                    Position newPosition = new Position
                    {
                        Id = rdr.GetInt32(0),
                        Level = ParseEnum<AllEnum.EmploymentLevel>(rdr.GetString(1)),
                        Start = rdr.GetDateTime(2),
                        End = rdr.GetDateTime(3),
                    };
                    positions.Add(newPosition);
                    Debug.WriteLine($"Added Position: Id={newPosition.Id}, Level={newPosition.Level}, Start={newPosition.Start}, End={newPosition.End}");
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
            
            return positions;
        }
    }
}