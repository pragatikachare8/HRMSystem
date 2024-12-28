using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRMSAdminModule
{
    public partial class UserViewDocument : System.Web.UI.Page
    {

        // Original

        //string s = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!IsPostBack)
        //    {
        //        FetchData();
        //    }   
        //}
        //private void FetchData()
        //{
        //    string q = "exec FetchDocumentData";
        //    using (SqlConnection conn = new SqlConnection(s))
        //    {
        //        SqlDataAdapter da = new SqlDataAdapter(q, conn);
        //        DataTable dt = new DataTable();
        //        da.Fill(dt);
        //        Repeater1.DataSource = dt;
        //        Repeater1.DataBind();
        //    }
        //}

        //protected void Repeater1_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        //{
        //    if (e.CommandName == "ViewDocument")
        //    {
        //        int documentID = int.Parse(e.CommandArgument.ToString());

        //        using (SqlConnection conn = new SqlConnection(s))
        //        {
        //            SqlCommand cmd = new SqlCommand("SELECT DocName, DocData FROM Document WHERE DocId = @DocId", conn);
        //            cmd.Parameters.AddWithValue("@DocId", documentID);
        //            conn.Open();

        //            SqlDataReader reader = cmd.ExecuteReader();
        //            if (reader.Read())
        //            {
        //                string documentType = reader["DocName"].ToString();
        //                byte[] docdata = (byte[])reader["DocData"];

        //                Response.Clear();
        //                Response.ContentType = "application/pdf";
        //                Response.AddHeader("Content-Disposition", "inline; filename=document." + documentType.ToLower());
        //                Response.BinaryWrite(docdata);
        //                Response.End();
        //            }
        //        }
        //    }
        //}

        SqlConnection conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
            conn = new SqlConnection(cs);
            conn.Open();
            if (!IsPostBack)
            {
                FetchData();
            }
        }

        private void FetchData()
        {
            string q = "exec FetchDocumentData";

            SqlDataAdapter da = new SqlDataAdapter(q, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Repeater1.DataSource = dt;
            Repeater1.DataBind();

        }

        protected void Repeater1_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {

            if (e.CommandName == "ViewDocument")
            {

                int documentID = int.Parse(e.CommandArgument.ToString());


                string connectionString = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;


                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    SqlCommand cmd = new SqlCommand("SELECT DocName, DocData FROM Document WHERE DocId = @DocId", conn);
                    cmd.Parameters.AddWithValue("@DocId", documentID);

                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();


                        if (reader.Read())
                        {

                            string documentName = reader["DocName"].ToString();
                            byte[] docData = (byte[])reader["DocData"];


                            Response.Clear();
                            Response.ContentType = "application/pdf";
                            Response.AddHeader("Content-Disposition", "inline; filename=" + documentName.ToLower());
                            Response.BinaryWrite(docData);
                            Response.End();
                        }
                        else
                        {

                            Response.Write("Document not found.");
                        }
                    }
                    catch (Exception ex)
                    {

                        Response.Write("Error: " + ex.Message);
                    }
                }
            }
        }
    }
}