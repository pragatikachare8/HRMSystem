using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Reflection.Emit;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace HRMSAdminModule
{
    public partial class PayslipGeneration : System.Web.UI.Page
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
            string Email = TextBox1.Text;
            string q = "exec FetchEmpPayRoll @Email";
            SqlCommand cmd = new SqlCommand(q, conn);
            cmd.Parameters.AddWithValue("@Email", Email);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                Label1.Text = reader["Email"]?.ToString() ?? "N/A";
                Label2.Text = reader["BasicSalary"]?.ToString() ?? "0";
                Label3.Text = reader["PresentDays"]?.ToString() ?? "0";
                Label4.Text = reader["AbsentDays"]?.ToString() ?? "0";
                Label5.Text = reader["TotalLeaves"]?.ToString() ?? "0";
            }
            else
            {
                Label1.Text = "N/A";
                Label2.Text = "5000";
                Label3.Text = "N/A";
                Label4.Text = "N/A";
                Label5.Text = "N/A";
            }
            reader.Close();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            string Email = Label1.Text;



            string email, basicSalary, presentDays, absentDays, totalLeaves, month, year;

            SqlCommand cmd = new SqlCommand("FetchEmpPayRoll", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Email", Email);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                email = reader["Email"]?.ToString() ?? "N/A";
                basicSalary = reader["BasicSalary"]?.ToString() ?? "0";
                presentDays = reader["PresentDays"]?.ToString() ?? "0";
                absentDays = reader["AbsentDays"]?.ToString() ?? "0";
                totalLeaves = reader["TotalLeaves"]?.ToString() ?? "0";
                month = reader["Month"]?.ToString() ?? "Unknown";
                year = reader["Year"]?.ToString() ?? "Unknown";
            }
            else
            {
                Response.Write("<script>alert('Employee not found');</script>");
                return;
            }



            string paySlipPath = GeneratePaySlipPDF(email, basicSalary, presentDays, absentDays, totalLeaves, month, year);
            if (!string.IsNullOrEmpty(paySlipPath))
            {
                SendPaySlipEmail(email, paySlipPath);
                Response.Write("<script>alert('PaySlip generated and emailed successfully');</script>");
            }
        }
        private string GeneratePaySlipPDF(string Email, string basicSalary, string presentDays, string absentDays, string totalLeaves, string month, string year)
        {
            try
            {
                string directoryPath = Server.MapPath("~/PaySlips");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                string fileName = $"PaySlip_{ID}{month}{year}.pdf";
                string paySlipPath = Path.Combine(directoryPath, fileName);

                Document document = new Document();
                PdfWriter.GetInstance(document, new FileStream(paySlipPath, FileMode.Create));
                document.Open();

                document.Add(new Paragraph("Pay Slip"));
                document.Add(new Paragraph("Employee ID: " + ID));
                document.Add(new Paragraph("Email: " + Email));
                document.Add(new Paragraph("Basic Salary: " + basicSalary));
                document.Add(new Paragraph("Present Days: " + presentDays));
                document.Add(new Paragraph("Absent Days: " + absentDays));
                document.Add(new Paragraph("Total Leaves: " + totalLeaves));
                document.Add(new Paragraph("Month: " + month));
                document.Add(new Paragraph("Year: " + year));

                document.Close();


                UpdatePaySlipPathInDatabase(Email, paySlipPath);

                return paySlipPath;
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Error generating PDF: {ex.Message}');</script>");
                return null;
            }
        }

        private void UpdatePaySlipPathInDatabase(string Email, string paySlipPath)
        {
            try
            {
                string updateQuery = "UPDATE EmpPayRoll SET PaySlipPath = @PaySlipPath WHERE Email = @Email";
                SqlCommand cmd = new SqlCommand(updateQuery, conn);
                cmd.Parameters.AddWithValue("@PaySlipPath", paySlipPath);
                cmd.Parameters.AddWithValue("@email", Email);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    Response.Write("<script>alert('Failed to update PaySlip path in the database.');</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Error updating PaySlip path in database: {ex.Message}');</script>");
            }
        }



        private void SendPaySlipEmail(string email, string paySlipPath)
        {
            try
            {
                string subject = "Your Pay Slip";
                string body = "Dear Employee,\n\nPlease find your pay slip attached.\n\nBest Regards,\nPayroll Team";

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("chavanvinayak292@gmail.com");
                mail.To.Add(email);
                mail.Subject = subject;
                mail.Body = body;
                mail.Attachments.Add(new Attachment(paySlipPath));

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.Credentials = new NetworkCredential("chavanvinayak292@gmail.com", "shhgnreeqzfbkray");
                smtpClient.EnableSsl = true;

                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Error sending email: {ex.Message}');</script>");
            }
        }

    }
}