using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Runtime.Remoting.Lifetime;
namespace HRMSAdminModule
{
    public partial class LeaveManagement : System.Web.UI.Page
    {
        SqlConnection conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
            conn = new SqlConnection(cs);
            conn.Open();
            if (!IsPostBack)
            {
                GetData();
            }
        }

        public void GetData()
        {
            try
            {

                string cs = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    conn.Open();


                    string q = "FetchLeaveApplicationProc";
                    using (SqlCommand cmd = new SqlCommand(q, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;


                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            GridView1.DataSource = dt;
                            GridView1.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
            //Response.Redirect("User.Master.aspx");
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Approved")
            {
                try
                {
                    // Split the CommandArgument and validate
                    string[] args = e.CommandArgument.ToString().Split(',');
                    if (args.Length != 5)
                    {
                        throw new ArgumentException("Invalid CommandArgument format.");
                    }

                    string ID = args[0];
                    string UEmail = args[1];
                    string From_Date = args[2];
                    string To_Date = args[3];
                    string Action = args[4];

                    // Example debug message (useful during development)
                    Response.Write($"ID: {ID}, UEmail: {UEmail}, From_Date: {From_Date}, To_Date: {To_Date}, Action: {Action}");

                    // Approve leave logic
                    string cs = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
                    using (SqlConnection conn = new SqlConnection(cs))
                    {
                        conn.Open();

                        // Use the stored procedure to approve the leave
                        string q = "ApproveLeaveProc";
                        using (SqlCommand cmd = new SqlCommand(q, conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ID", ID);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    Response.Write("<script>alert('Leave approved successfully.');</script>");

                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                }
            }
        }
    }

}