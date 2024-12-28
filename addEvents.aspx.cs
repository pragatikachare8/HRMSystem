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
    public partial class addEvents : System.Web.UI.Page
    {
        SqlConnection conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
            conn = new SqlConnection(cs);
            conn.Open();

            if (!IsPostBack)
            {
                
                Panel1.Visible = false;
            }

        }

        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownList2.SelectedValue == "Holiday")
            {
                Panel1.Visible = false;
            }
            else
            {
                Panel1.Visible = true;
            }
        }

        public void FetchEvents()
        {
            string fetchEvent = $"exec fetchEvent";
            SqlCommand cmd = new SqlCommand(fetchEvent, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            GridView1.DataBind();
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            
            string eventType = DropDownList2.SelectedValue;
            string eventName = TextBox1.Text;
            string email = TextBox2.Text;

            DateTime startDate, endDate;
            bool isStartDateValid = DateTime.TryParse(TextBox3.Text, out startDate);
            bool isEndDateValid = DateTime.TryParse(TextBox4.Text, out endDate);
            string formattedStartDate = startDate.ToString("yyyy-MM-dd");
            string formattedEndDate = endDate.ToString("yyyy-MM-dd");
            if (!isStartDateValid || !isEndDateValid)
            {

                Response.Write("<script>alert('Please enter valid date');</script>");
                return;
            }
            else
            {
                string q = $"exec AddEvent '{eventName}','{eventType}','{email}','{formattedStartDate}','{formattedEndDate}'";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
                Response.Write("<script>alert('Event Added Successfully');windows.location.href='EventCalendar.aspx'';</script>");
                FetchEvents();
            }

        }
        
       

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string eventId = GridView1.DataKeys[e.RowIndex].Value.ToString();
            string deleteEvent = $"exec deleteEvent '{eventId}'";
            SqlCommand cmd = new SqlCommand(deleteEvent, conn);
            cmd.ExecuteNonQuery();
            Response.Write("<script>alert('Event Deleted Successfully');</script>");
            FetchEvents();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            FetchEvents();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int eventId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);

            GridViewRow row = GridView1.Rows[e.RowIndex];
            string eName = ((TextBox)row.FindControl("txtEName")).Text;
            string eType = ((TextBox)row.FindControl("txtEType")).Text;
            string email = ((TextBox)row.FindControl("txtEmail")).Text;

            string q = $"exec updateEvent '{eventId}','{eName}','{eType}','{email}'";
            SqlCommand command = new SqlCommand(q, conn);
            command.ExecuteNonQuery();

            GridView1.EditIndex = -1;
            FetchEvents();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            FetchEvents();

        }

    
    }
}