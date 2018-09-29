<%@ Page Title="" Language="C#" MasterPageFile="~/home.master" AutoEventWireup="true" CodeFile="home.aspx.cs" Inherits="admin_home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="User" Runat="Server">
    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" Runat="Server">
    Select Operation to Perform:<br />
    <asp:RadioButtonList ID="RadioButtonList1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged">
        <asp:ListItem Value="0">Create New User</asp:ListItem>
    </asp:RadioButtonList>
    <br />
    <asp:Panel ID="Panel1" runat="server" Visible="False">
        Select Role of the new User: <asp:DropDownList ID="DropDownList1" runat="server" Width="120px">
            <asp:ListItem Value="1">Faculty</asp:ListItem>
            <asp:ListItem Value="2">Faculty In Charge</asp:ListItem>
        </asp:DropDownList><br />
        Enter New Username : <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox><br />
        Enter New Password : <asp:TextBox ID="TextBox2" runat="server" TextMode="Password"></asp:TextBox><br />
        <asp:Button ID="Button1" runat="server" Text="Create" OnClick="Button1_Click" />
    </asp:Panel>
    <br />
</asp:Content>


