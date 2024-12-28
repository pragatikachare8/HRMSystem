using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRMSAdminModule
{
    public partial class BulkUpload : System.Web.UI.Page
    {
        
       SqlConnection conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
            conn = new SqlConnection(cs);
            conn.Open();

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {

                Stream fileStream = FileUpload1.PostedFile.InputStream;
                ProcessCsvFile(fileStream);

                Response.Write("<script>alert('File uploaded and processed successfully');</script>");

            }
            else
            {
                Response.Write("<script>alert('Please select a file to upload.');</script>");
            }

        }

        public void ProcessCsvFile(Stream fileStream)
        {

            if (fileStream == null || fileStream.Length == 0)
            {
                throw new ArgumentException("The provided file stream is null or empty.");
            }

            StreamReader reader = new StreamReader(fileStream);
            try
            {

                var lines = new List<string>();
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }

                if (lines.Count > 1)
                {
                    foreach (var line in lines.Skip(1))
                    {
                        var columns = line.Split(',');

                        if (columns.Length >= 10)
                        {
                            string empFirstName = columns[0].Trim();
                            string empLastName = columns[1].Trim();
                            string email = columns[2].Trim();
                            string contact = columns[3].Trim();
                            string doj = columns[4].Trim();
                            string department = columns[5].Trim();
                            string username = columns[6].Trim();
                            string password = columns[7].Trim();
                            string stat = columns[8].Trim();
                            string userRole = columns[9].Trim();

                            try
                            {
                                if (string.IsNullOrWhiteSpace(email))
                                {
                                    Console.WriteLine("<script>alert('Skipping row due to missing mandatory fields:');</script>");
                                    continue;
                                }

                                string query = $"exec FindExistingUser '{email}'";
                                SqlCommand cm = new SqlCommand(query, conn);
                                SqlDataReader rdr = cm.ExecuteReader();

                                bool userExists = rdr.HasRows;
                                reader.Close();

                                if (userExists)
                                {
                                    string alertScript = $"<script>alert('User with email \"{email}\" already exists. Skipping row.');</script>";
                                    Response.Write(alertScript);
                                    continue;
                                }

                                string blukuoloadEmployee = $"exec AddEmployee '{empFirstName}','{empLastName}','{email}','{contact}','{doj}','{department}','{username}','{password}','{stat}','{userRole}'";
                                SqlCommand cmd = new SqlCommand(blukuoloadEmployee, conn);
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception dbEx)
                            {

                                System.Diagnostics.Debug.WriteLine($"Error processing line: {line}. Error: {dbEx.Message}");
                            }
                        }

                        else
                        {
                            System.Diagnostics.Debug.WriteLine("Skipping row due to insufficient columns.");
                        }

                    }
                }

            }


            catch (Exception ex)
            {

                Console.WriteLine($"Error processing the CSV file: {ex.Message}");
            }

            finally
            {

                reader.Close();
            }
        }
    }
}