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
    public partial class UserFileUpload : System.Web.UI.Page
    {
        string s = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            //string cs = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
            //conn= new SqlConnection(cs);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                int EmployeeId = int.Parse(TextBox1.Text);
                string EmployeeName = TextBox2.Text;
                string DocName = DropDownList1.SelectedValue;
                byte[] DocData = FileUpload1.FileBytes;

                using (SqlConnection conn = new SqlConnection(s))
                {
                    string q = "insert into Document(EmployeeId, DocName, DocData) values(@EmployeeId, @DocName, @DocData)";

                    using (SqlCommand cmd = new SqlCommand(q, conn))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeId", EmployeeId);
                        cmd.Parameters.AddWithValue("@DocName", DocName);
                        cmd.Parameters.AddWithValue("@DocData", DocData);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        Response.Write("<script>alert('Document Uploaded Successfully!!!!!');</script>");
                    }
                }
            }
        }

    }
}