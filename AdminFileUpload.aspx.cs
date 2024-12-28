using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Org.BouncyCastle.Asn1.Ocsp;

namespace HRMSAdminModule
{
    public partial class AdminFileUpload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "POST")
            {
                string docName = Request.Form["DocName"];  // Retrieve selected document type
                HttpPostedFile file = Request.Files["File"];  // Retrieve the uploaded file

                // Get employee IDs from the request (if any)
                string[] employeeIds = Request.Form.GetValues("EmployeeIds[]");

                if (file != null && !string.IsNullOrEmpty(docName))
                {
                    byte[] docData = new byte[file.ContentLength];
                    file.InputStream.Read(docData, 0, docData.Length);

                    // Insert the document into the database
                    using (SqlConnection conn = new SqlConnection(cs))
                    {
                        string insertDocument = "INSERT INTO Document(DocName, DocData) VALUES(@DocName, @DocData)";
                        using (SqlCommand cmd = new SqlCommand(insertDocument, conn))
                        {
                            cmd.Parameters.Add("@DocName", SqlDbType.VarChar).Value = docName;
                            cmd.Parameters.Add("@DocData", SqlDbType.VarBinary).Value = docData;

                            try
                            {
                                conn.Open();
                                cmd.ExecuteNonQuery();

                                // Optionally, insert employee associations if needed
                                if (employeeIds != null)
                                {
                                    foreach (var empId in employeeIds)
                                    {
                                        // Insert employee-document association logic here (if required)
                                        // Example: Insert into EmployeeDocuments table or similar
                                    }
                                }

                                Response.Write("Success");
                            }
                            catch (Exception ex)
                            {
                                Response.StatusCode = 500;
                                Response.Write("Error: " + ex.Message);
                            }
                        }
                    }
                }
                else
                {
                    Response.StatusCode = 400;
                    Response.Write("Error: Invalid document type or file.");
                }
            }
        }


    }
}