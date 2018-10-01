<%@ Page Title="" Language="C#" MasterPageFile="~/home.master" AutoEventWireup="true" CodeFile="home.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="User" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" Runat="Server">
    Select Operation to Perform:<asp:RadioButtonList ID="RadioButtonList1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged">
        <asp:ListItem Value="0">Add new Question</asp:ListItem>
        <asp:ListItem Value="1">See Current Questions</asp:ListItem>
        <asp:ListItem Value="2">None</asp:ListItem>
    </asp:RadioButtonList>
    <asp:Panel ID="Panel1" runat="server" Visible="False">
        Enter Question:
        <asp:TextBox ID="TextBox1" runat="server" Width="468px"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="You must Enter a Question!!" ControlToValidate="TextBox1"></asp:RequiredFieldValidator>
        <br />
        <asp:CheckBoxList ID="CheckBoxList1" runat="server" AutoPostBack="True" TextAlign="Left" OnSelectedIndexChanged="CheckBoxList1_SelectedIndexChanged">
            <asp:ListItem Value="0">Is it a MCQ?</asp:ListItem>
        </asp:CheckBoxList>
        <asp:Panel ID="Panel2" runat="server" Visible="False">
            Enter Options:<br /> A)
            <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Option must not be Empty!!" ControlToValidate="TextBox2" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:CustomValidator ID="CustomValidator1" runat="server" Display="Dynamic" ErrorMessage="Options Can't be Same!!" OnServerValidate="CustomValidator1_ServerValidate" ></asp:CustomValidator>
            <br />
            B)
            <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Option must not be Empty!!" ControlToValidate="TextBox3" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:CustomValidator ID="CustomValidator2" runat="server" Display="Dynamic" ErrorMessage="Options Can't be Same!!" OnServerValidate="CustomValidator2_ServerValidate"></asp:CustomValidator>
            <br />
            C)
            <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="TextBox4" ErrorMessage="Option must not be Empty!!" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:CustomValidator ID="CustomValidator3" runat="server" Display="Dynamic" ErrorMessage="Options Can't be Same!!" OnServerValidate="CustomValidator3_ServerValidate"></asp:CustomValidator>
            <br />
            D)
            <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Option must not be Empty!!" ControlToValidate="TextBox5" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:CustomValidator ID="CustomValidator4" runat="server" Display="Dynamic" ErrorMessage="Options Can't be Same!!" OnServerValidate="CustomValidator4_ServerValidate"></asp:CustomValidator>
        </asp:Panel>
        Enter Marks: 
        <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="TextBox6" ErrorMessage="You must Enter Marks!!"></asp:RequiredFieldValidator>
        <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="Enter a proper number!!" MinimumValue="1" Type="Integer" ControlToValidate="TextBox6" MaximumValue="100"></asp:RangeValidator>
        <br />
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Submit" />
    </asp:Panel>
&nbsp;

</asp:Content>

