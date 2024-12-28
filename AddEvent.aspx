<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddEvent.aspx.cs" Inherits="HRMSAdminModule.AddEvent" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Event</title>
    <!-- Bootstrap CSS -->
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-5">
            <div class="card">
                <div class="card-header bg-primary text-white">
                    <h4 class="mb-0">Add Event</h4>
                </div>
                <div class="card-body">
                    <div class="form-group">
                        <label for="DropDownList1">Event Type:</label>
                        <asp:DropDownList
                            ID="DropDownList1"
                            runat="server"
                            CssClass="form-control"
                            AutoPostBack="True"
                            OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                            <asp:ListItem Value="Holiday">Holiday</asp:ListItem>
                            <asp:ListItem Value="Birthday">Birthday</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="form-group">
                        <label for="TextBox1">Event Name:</label>
                        <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <asp:Panel class="form-group" ID="Panel1" runat="server">
                        <label for="TextBox2" runat="server" id="Label1">Email:</label>
                        <asp:TextBox ID="TextBox2" runat="server" CssClass="form-control"></asp:TextBox>
                    </asp:Panel>

                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <label for="TextBox3">From:</label>
                            <asp:TextBox ID="TextBox3" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group col-md-6">
                            <label for="TextBox4">To:</label>
                            <asp:TextBox ID="TextBox4" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>

                    <div class="text-center">
                        <asp:Button ID="Button1" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="Button1_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div class="card">
            <div class="card-header text-black">
                <h4 class="mb-0">Events List</h4>
            </div>
            <div class="card-body">
                <asp:GridView
                    ID="GridView1"
                    runat="server"
                    CssClass="table table-bordered table-striped"
                    AutoGenerateColumns="False"
                    DataKeyNames="eventId"
                    DataSourceID="SqlDataSource1"
                    OnRowDeleting="GridView1_RowDeleting"
                    OnRowEditing="GridView1_RowEditing"
                    OnRowUpdating="GridView1_RowUpdating"
                    OnRowCancelingEdit="GridView1_RowCancelingEdit">
                    <Columns>
                        <asp:BoundField DataField="eventId" HeaderText="eventId" InsertVisible="False" ReadOnly="True" SortExpression="eventId" />
                        <asp:TemplateField HeaderText="eName">
                            <ItemTemplate>
                                <%# Eval("eName") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEName" runat="server" Text='<%# Bind("eName") %>' />
                            </EditItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="eType">
                            <ItemTemplate>
                                <%# Eval("eType") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEType" runat="server" Text='<%# Bind("eType") %>' />
                            </EditItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Email">
                            <ItemTemplate>
                                <%# Eval("Email") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEmail" runat="server" Text='<%# Bind("Email") %>' />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="StartDate" HeaderText="StartDate" SortExpression="StartDate" />
                        <asp:BoundField DataField="EndDate" HeaderText="EndDate" SortExpression="EndDate" />
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="Edit" CommandArgument='<%# Eval("eventId") %>' CssClass="btn btn-primary btn-sm" />
                                <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="Delete" CommandArgument='<%# Eval("eventId") %>' CssClass="btn btn-danger btn-sm" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button ID="btnUpdate" runat="server" Text="Update" CommandName="Update" CssClass="btn btn-success btn-sm" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CommandName="Cancel" CssClass="btn btn-warning btn-sm" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1"
                    runat="server"
                    ConnectionString="<%$ ConnectionStrings:HRMSConnectionString2 %>"
                    ProviderName="<%$ ConnectionStrings:HRMSConnectionString2.ProviderName %>"
                    SelectCommand="SELECT * FROM [Event]"
                    UpdateCommand="UPDATE [Event] SET eName = @eName, eType = @eType, Email = @Email WHERE eventId = @eventId"
                    DeleteCommand="Delete FROM [Event] WHERE eventId=@eventID">
                    <UpdateParameters>
                        <asp:Parameter Name="eName" Type="String" />
                        <asp:Parameter Name="eType" Type="String" />
                        <asp:Parameter Name="Email" Type="String" />
                        <asp:Parameter Name="eventId" Type="Int32" />
                    </UpdateParameters>
                </asp:SqlDataSource>
            </div>
        </div>
    </form>

    <!-- Bootstrap JS and dependencies -->
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.4.4/dist/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
</body>
</html>
