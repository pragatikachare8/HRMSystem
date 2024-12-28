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
    public partial class index : System.Web.UI.Page
    {
        SqlConnection conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
            conn = new SqlConnection(cs);
            conn.Open();
            if (!IsPostBack)
            {
                ActiveUserCount();
            }

        }

        public void ActiveUserCount()
        {
            string ActiveUserCount = "exec  ActiveEmpCount";
            SqlCommand cmd = new SqlCommand(ActiveUserCount, conn);
            object result = cmd.ExecuteScalar();
           
            if(result != null)
            {
                txtEmpCount.Text = result.ToString();
            }
            else
            {
                txtEmpCount.Text = "0";
            }
        }
    }
}