using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace HRMSAdminModule
{
    public partial class payslipManagement : System.Web.UI.Page
    {
        SqlConnection conn;

        protected void Page_Load(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
            conn = new SqlConnection(cs);
            conn.Open();


            if (!IsPostBack)
            {
                GridView2.DataSource = null;
                GridView2.DataBind();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string Email = TextBox2.Text;


                SqlCommand cmd = new SqlCommand("exec FetchEmpPayRoll @Email", conn);
                cmd.Parameters.AddWithValue("@Email", Email);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    GridView2.DataSource = dt;
                    GridView2.DataBind();
                }
                else
                {
                    Response.Write("<script>alert('No payslip data found for this Email');</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Error fetching data: {ex.Message}');</script>");
            }
        }
        protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "View" || e.CommandName == "Download")
            {
                try
                {

                    string[] args = e.CommandArgument.ToString().Split(',');
                    int ID = int.Parse(args[0]);
                    string month = args[1];
                    string year = args[2];

                    string fileName = $"PaySlip_{ID}{month}{year}.pdf";
                    string filePath = Server.MapPath($"~/PaySlips/{fileName}");

                    if (File.Exists(filePath))
                    {
                        if (e.CommandName == "View")
                        {

                            Response.ContentType = "application/pdf";
                            Response.AppendHeader("Content-Disposition", $"inline; filename={fileName}");
                            Response.TransmitFile(filePath);
                            Response.End();
                        }
                        else if (e.CommandName == "Download")
                        {

                            Response.ContentType = "application/pdf";
                            Response.AppendHeader("Content-Disposition", $"attachment; filename={fileName}");
                            Response.TransmitFile(filePath);
                            Response.End();
                        }
                    }
                    else
                    {
                        Response.Write("<script>alert('Payslip file not found.');</script>");
                    }
                }
                catch (Exception ex)
                {
                    Response.Write($"<script>alert('Error processing request: {ex.Message}');</script>");
                }
            }
        }
    }
}