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
    public partial class manageEmployee : System.Web.UI.Page
    {
        SqlConnection conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
            conn = new SqlConnection(cs);
            conn.Open();
            if (!IsPostBack)
            {
                FetchEmployees();
            }

        }

        public void FetchEmployees()
        {
            string query = "exec FetchEmployeeDetails";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            GridView1.DataBind();
        }
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            FetchEmployees();
        }


        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int employeeId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);

            GridViewRow row = GridView1.Rows[e.RowIndex];

            TextBox txtFirstName = (TextBox)row.FindControl("txtFirstName");
            TextBox txtEmail = (TextBox)row.FindControl("txtEmail");
            TextBox txtContactNo = (TextBox)row.FindControl("txtContactNo");
            TextBox txtDOJ = (TextBox)row.FindControl("txtDOJ");
            TextBox txtDepartment = (TextBox)row.FindControl("txtDepartment");


            string firstName = txtFirstName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string contactNo = txtContactNo.Text.Trim();
            DateTime doj = DateTime.Parse(txtDOJ.Text.Trim());
            string department = txtDepartment.Text.Trim();

            string updateEmployee = "exec updateEmployeeDetils @employeeId, @firstName, @email, @contactNo, @doj, @department";

            SqlCommand cmd = new SqlCommand(updateEmployee, conn);
            cmd.Parameters.AddWithValue("@employeeId", employeeId);
            cmd.Parameters.AddWithValue("@firstName", firstName);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@contactNo", contactNo);
            cmd.Parameters.AddWithValue("@doj", doj);
            cmd.Parameters.AddWithValue("@department", department);
            cmd.ExecuteNonQuery();
            GridView1.EditIndex = -1;
            GridView1.DataBind();
            FetchEmployees();
        }



        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            FetchEmployees();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {


            string empid = GridView1.DataKeys[e.RowIndex].Value.ToString();

            string deleteEmp = $"exec DeleteEmployee '{empid}'";
            SqlCommand cmd = new SqlCommand(deleteEmp, conn);
            cmd.ExecuteNonQuery();
            Response.Write("<script>alert('Employee Deleted Successfully');</script>");
            FetchEmployees();
        }


        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblStatus = e.Row.FindControl("lblStatus") as Label;
                Button btnActivate = e.Row.FindControl("btnActivate") as Button;
                Button btnDeactivate = e.Row.FindControl("btnDeactivate") as Button;

                if (lblStatus != null)
                {
                    if (btnActivate != null)
                    {
                        if (lblStatus.Text == "Active")
                        {
                            btnActivate.Enabled = false;
                        }
                    }
                    if (btnDeactivate != null)
                    {
                        if (lblStatus.Text == "Inactive")
                        {
                            btnDeactivate.Enabled = false;
                        }
                    }

                }
            }
        }
        protected void btnActivate_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            string empid = btn.CommandArgument;
            Label statusValue = row.FindControl("lblStatus") as Label;
            string status = (statusValue.Text.Trim());

            int empIdInt;
            if (int.TryParse(empid, out empIdInt))
            {

                if (status != "Active")
                {
                    string updateEmpStatusActive = $"exec UpdateEmployeeStatustoActive {empIdInt}";
                    SqlCommand cmd = new SqlCommand(updateEmpStatusActive, conn);
                    cmd.ExecuteNonQuery();
                    FetchEmployees();
                }
            }

        }

        protected void btnDeactivate_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            string empid = btn.CommandArgument;
            Label statusValue = row.FindControl("lblStatus") as Label;
            string status = statusValue.Text.Trim();

            int empIdInt;
            if (int.TryParse(empid, out empIdInt))
            {
                if (status == "Active")
                {
                    string updateEmpStatusInactive = $"exec UpdateEmployeeStatustoInactive {empIdInt}";
                    SqlCommand cmd = new SqlCommand(updateEmpStatusInactive, conn);
                    cmd.ExecuteNonQuery();
                    FetchEmployees();
                }
            }

        }
    }
}