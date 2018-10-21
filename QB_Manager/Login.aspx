<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" style="background-image: url(mit.jpg); background-repeat:no-repeat;background-size:cover;background-position: center;height: 100%;">
<head runat="server">
    <title></title>
    <link href="StyleSheet.css" rel="stylesheet" />
</head>
<body">
    <form id="form1" runat="server">
        
        
        <div style="text-align: center;">
            <br />
            <br />
            <br />
            <h1 style="font-family:'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif; font-size:45px">Question Bank Portal</h1>

            <div style="width: 400px; margin: auto; margin-top:190px; background-color:rgba(128,128,128,0.9)">
            

          
                <br />
                User Name&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="TextBox1" runat="server" ></asp:TextBox><br />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="Red" runat="server" ErrorMessage="Username can't be empty" ControlToValidate="TextBox1"></asp:RequiredFieldValidator>
            <br />
            <br />
            &nbsp;&nbsp;&nbsp;Password&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="TextBox2" runat="server" TextMode="Password"></asp:TextBox><br />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ForeColor="Red" ErrorMessage="Password can't be empty" ControlToValidate="TextBox2" ></asp:RequiredFieldValidator>
            <br />
            <br />
            &nbsp;<asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Submit" />

                <br />

                <br />
                
                </div>
        </div>
      
    </form>
</body>
</html>
