using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRMSAdminModule
{
    public partial class CreateEmployee : System.Web.UI.Page
    {
        SqlConnection conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
            conn = new SqlConnection(cs);
            conn.Open();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string firstName = TextBox1.Text;
            string lastName = TextBox2.Text;
            string email = TextBox3.Text;
            string contact = TextBox4.Text;
            string DOJ = TextBox5.Text;
            string dept = DropDownList1.SelectedValue;
            string username = TextBox6.Text;
            string pass = TextBox7.Text;
            string status = rbActive.Checked ? "Active" : "Inactive";
            string urole = "User";

            string query = $"exec FindExistingUser '{email}'";
            SqlCommand cm = new SqlCommand(query, conn);
            SqlDataReader rdr = cm.ExecuteReader();

            if (rdr.HasRows)
            {
                Response.Write("<script>alert('Employee Already Exists!!!!')</script>");
            }

            else
            {
                string sp_addEmployee = $"exec AddEmployee '{firstName}','{lastName}','{email}','{contact}','{DOJ}','{dept}','{username}','{pass}','{status}','{urole}'";
                SqlCommand cmd = new SqlCommand(sp_addEmployee, conn);
                cmd.ExecuteNonQuery();

                Response.Write("<script>alert('Employee Created Successfuly');windows.location.href='ManageEmloyees.aspx';</script>");

                string subject = "Login Credentials";
                string body = $"Hello {firstName},\n\nWelcome to our company!\n\n Please Find Your Login Credentials \n\n username: {email}.\n\n Password: {pass}\n\nThank you,\nHR Team";

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("pragatikachare8@gmail.com");
                mail.To.Add(email);
                mail.Subject = subject;
                mail.Body = body;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential("pragatikachare84@gmail.com", "qamcixcknozekzmk");
                smtp.Send(mail);

                Response.Write("<script>alert('Mail send Successfully !!!!!')</script>");



            }
        }
    }
}