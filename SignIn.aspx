<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignIn.aspx.cs" Inherits="HRMSAdminModule.SignIn" %>

<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="utf-8">
 <meta http-equiv="X-UA-Compatible" content="IE=edge">
 <meta name="viewport" content="width=device-width,initial-scale=1">
 <title>Quixlab - Bootstrap Admin Dashboard Template by Themefisher.com</title>
 <!-- Favicon icon -->
 <link rel="icon" type="image/png" sizes="16x16" href="../../assets/images/favicon.png">
 <!-- <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.5.0/css/all.css" integrity="sha384-B4dIYHKNBt8Bc12p+WXckhzcICo0wtJAoU8YZTY5qE0Id1GSseTk6S+L3BlXeVIU" crossorigin="anonymous"> -->
 <link href="css/style.css" rel="stylesheet">
   
</head>

<body>
        <div class="login-form-bg h-100">
     <div class="container h-100">
         <div class="row justify-content-center h-100">
             <div class="col-xl-6">
                 <div class="form-input-content">
                     <div class="card login-form mb-0">
                         <div class="card-body pt-5">
                             <a class="text-center" href="index.html"> <h4>Masstech</h4></a>
     
                             <form class="mt-5 mb-5 login-input" runat="server">
                                 <div class="form-group">
                                     <asp:TextBox ID="TextBox1"  class="form-control" placeholder="Email" runat="server"></asp:TextBox>
                                 </div>
                                 <div class="form-group">
                                    <asp:TextBox ID="TextBox2"  class="form-control" placeholder="Password" TextMode="Password" runat="server"></asp:TextBox>
                                 </div>
                                 <asp:Button ID="Button1" runat="server" Text="SignIn"  class="btn login-form__btn submit w-100" OnClick="Button1_Click1"/>
                             </form>
                         </div>
                     </div>
                 </div>
             </div>
         </div>
     </div>
 </div>
   
</body>

</html>
