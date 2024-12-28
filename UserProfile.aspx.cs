using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRMSAdminModule
{
    public partial class UserProfile : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;

        public string EmployeeId { get; set; }
        public string EmpFirstName { get; set; }
        public string EmpLastName { get; set; }
        protected string Email { get; set; }
        protected string Contact { get; set; }
        protected string Doj { get; set; }
        protected string Department { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserEmail"] != null)
            {
                // Get the user's email from the session
                string userEmail = Session["UserEmail"].ToString();

                // Load user profile data from the database using the email
                LoadUserProfile(userEmail);
            }
            else
            {
                // If the session does not contain the email, the user is not logged in
                lblMessage.InnerText = "User is not logged in.";
            }
        }

        private void LoadUserProfile(string Email)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("GetUserProfile", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", Email);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // Fetching data from the database
                        EmployeeId = reader["EmployeeId"].ToString();
                        EmpFirstName = reader["EmpFirstName"].ToString();
                        EmpLastName = reader["EmpLastName"].ToString();
                        Email = reader["Email"].ToString();
                        Contact = reader["Contact"].ToString();
                        Doj = Convert.ToDateTime(reader["Doj"]).ToString("yyyy-MM-dd");
                        Department = reader["Department"].ToString();
                    }
                    reader.Close();

                    // Bind values to controls
                    lblEmpID.InnerText = EmployeeId;
                    lblName.InnerText = $"{EmpFirstName} {EmpLastName}";
                    lblEmail.InnerText = Email;
                    lblContactNo.InnerText = Contact;
                    lblDOJ.InnerText = Doj;
                    lblDepartment.InnerText = Department;
                }
                catch (Exception ex)
                {
                    lblMessage.InnerText = "Error: " + ex.Message;
                }
            }
        }
    }
}