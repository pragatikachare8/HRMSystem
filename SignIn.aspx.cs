using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRMSAdminModule
{
    public partial class SignIn : System.Web.UI.Page
    {
        SqlConnection conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
            conn = new SqlConnection(cs);
            conn.Open();
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            string email = TextBox1.Text, password = TextBox2.Text;
            string q = $"exec SignIn '{email}','{password}'";
            SqlCommand cmd = new SqlCommand(q, conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                  
                    if (rdr["Stat"].Equals("Active"))
                    {
                        if (rdr["Email"].Equals(email) && rdr["Pass"].Equals(password) && rdr["urole"].Equals("Admin"))
                        {
                            Session["UserEmail"] = email;
                            Response.Redirect("AdminDashboard.aspx");
                        }

                        if (rdr["Email"].Equals(email) && rdr["Pass"].Equals(password) && rdr["urole"].Equals("User"))
                        {
                            Session["UserEmail"] = email;
                            Response.Redirect("Userdashboards.aspx");
                        }
                    }
                    else
                    {
                        Response.Write("<script>alert('Your account is deactivated. Please contact Admin.');</script>");
                        return;
                    }
                }
            }
            else
            {
                Response.Write("<script>alert('Invalid User Credentials!!!! try again');</script>");
            }
        }
    }
}