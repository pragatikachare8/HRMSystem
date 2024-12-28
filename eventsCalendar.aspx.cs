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
    public partial class eventsCalendar : System.Web.UI.Page
    {
        SqlConnection conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
            conn = new SqlConnection(cs);
            conn.Open();
            if (!IsPostBack)
            {

            }
        }


        protected void Calendar1_DayRender(object sender, DayRenderEventArgs e)
        {
            string query = $"exec fetchEvent";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {

                DateTime startDate = Convert.ToDateTime(reader["StartDate"]);
                DateTime endDate = Convert.ToDateTime(reader["EndDate"]);
                string eventType = reader["eType"].ToString();
                string eventName = reader["eName"].ToString();



                if (e.Day.Date >= startDate.Date && e.Day.Date <= endDate.Date)
                {
                    e.Cell.ToolTip = eventName;

                    if (eventType.Equals("Holiday"))
                    {
                        string hexRed = "#f59b98";
                        e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml(hexRed);
                    }
                    else if (eventType.Equals("Birthday"))
                    {
                        string hexBlue = "#8bbdf5";
                        e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml(hexBlue);
                    }
                }
            }


        }
    }
}