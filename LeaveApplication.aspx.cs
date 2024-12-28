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
    public partial class LeaveApplication : System.Web.UI.Page
    {
        SqlConnection conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
            conn = new SqlConnection(cs);
            conn.Open();

        }


        protected void Button2_Click(object sender, EventArgs e)
        {
            string semail = Session["UserEmail"].ToString();
            string Leave_Type = DropDownList2.SelectedValue;
            string From_Date = TextBox5.Text;
            string To_Date = TextBox1.Text;
            string Reason = TextBox2.Text;

            string q = $"exec LeaveApplicationProc '{semail}','{Leave_Type}','{From_Date}','{To_Date}','{Reason}'";
            SqlCommand cmd = new SqlCommand(q, conn);
            cmd.ExecuteNonQuery();
            Response.Write("<script>alert('Leave Applied Successfully!!!'); window.location.href='LeaveApplication.aspx';</script>");
        }
    }
}