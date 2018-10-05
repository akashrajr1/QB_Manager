<%@ Page Title="" Language="C#" MasterPageFile="~/home.master" AutoEventWireup="true" CodeFile="home.aspx.cs" Inherits="admin_home" %>



<asp:Content ID="Content2" ContentPlaceHolderID="Body" Runat="Server">

    Select Operation to Perform:<br />
    <asp:RadioButtonList ID="RadioButtonList1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged">
        <asp:ListItem Value="0">Create New User</asp:ListItem>
        <asp:ListItem Value="1">Show Current Users</asp:ListItem>
        <asp:ListItem Value="2">Change User Password</asp:ListItem>
        <asp:ListItem Value="3">Change User Role</asp:ListItem>
        <asp:ListItem Value="4">Block a User</asp:ListItem>
        <asp:ListItem Value="5">Delete User</asp:ListItem>
        <asp:ListItem Value="6">None</asp:ListItem>
    </asp:RadioButtonList>
    <br />
    <asp:Panel ID="Panel1" runat="server" Visible="False">
        Select Role of the new User: <asp:DropDownList ID="DropDownList1" runat="server" Width="120px">
            <asp:ListItem Value="1">Faculty In Charge</asp:ListItem>
            <asp:ListItem Value="2">Faculty</asp:ListItem>
        </asp:DropDownList><br />
        Enter New Username : <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TextBox1" ErrorMessage="Usrname Required!!"></asp:RequiredFieldValidator>
        <br />
        Enter New Password : <asp:TextBox ID="TextBox2" runat="server" TextMode="Password"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="TextBox2" ErrorMessage="Password Can't be Empty!!"></asp:RequiredFieldValidator>
        <br />
        Renter Password:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="TextBox5" runat="server" TextMode="Password"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="TextBox5" ErrorMessage="RequiredFieldValidator" Text="Password Can't be empty!!" Display="Dynamic"></asp:RequiredFieldValidator>
        <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToCompare="TextBox2" ControlToValidate="TextBox5" ErrorMessage="CompareValidator" Text="Password doesn't match!!" Display="Dynamic"></asp:CompareValidator>
        <br />
        Select Subject/s:<br />
        <asp:GridView ID="SubjectGridView" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="8" OnPageIndexChanging="SubjectGridViewPageIndexChanging" RowStyle-HorizontalAlign="Center">
            <Columns>
                <asp:BoundField DataField="subid" HeaderText="Id" SortExpression="subid" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBox1" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="subject" HeaderText="Subject" SortExpression="subject" />
                <asp:BoundField DataField="branch" HeaderText="Branch" SortExpression="branch" />
                <asp:BoundField DataField="semester" HeaderText="Semester" SortExpression="semester" />
            </Columns>
        </asp:GridView>
        <br />
        <asp:Button ID="Button1" runat="server" Text="Create" OnClick="Button1_Click" />
    </asp:Panel>

    <asp:Panel ID="Panel2" runat="server" Width="190px" Visible="False">
        <asp:GridView ID="UserGridView" runat="server" AutoGenerateColumns="False" AllowPaging="true" PageSize="8" OnPageIndexChanging="UserGridViewPageIndexChanging" RowStyle-HorizontalAlign="Center">
            <Columns>
                <asp:BoundField DataField="username" HeaderText="Username" SortExpression="username" />
                <asp:BoundField DataField="role" HeaderText="Role" SortExpression="role"/>
            </Columns>
        </asp:GridView>
        
</asp:Panel>
    <asp:Panel ID="Panel3" runat="server" Visible="False">
        Select User:
        <asp:DropDownList ID="UserDropDownList" runat="server">
        </asp:DropDownList>
        <br />
        New Password&nbsp;&nbsp;
        <asp:TextBox ID="TextBox3" runat="server" TextMode="Password"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox3" ErrorMessage="RequiredFieldValidator" Text="Passwords can't be empty!!"></asp:RequiredFieldValidator>
        <br />
        Verify Password
        <asp:TextBox ID="TextBox4" runat="server" TextMode="Password"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="RequiredFieldValidator" ControlToValidate="TextBox4" Text="Passwords can't be empty!!"></asp:RequiredFieldValidator>
        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="CompareValidator" ControlToValidate="TextBox4" ControlToCompare="TextBox3" Text="Password doesn't match!!"></asp:CompareValidator>
        <br />
        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Change!!" />
        <br />
    </asp:Panel>
    <asp:Panel ID="Panel4" runat="server" Visible="False">
        Select User:
        <asp:DropDownList ID="UserDropDownList2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="UserDropDownList_SelectedIndexChanged">
        </asp:DropDownList>
        <br />
        Current Role:
        <asp:Label ID="Label2" runat="server"></asp:Label>
        <br />
        Select Role :
        <asp:DropDownList ID="DropDownList4" runat="server" Width="120px">
            <asp:ListItem Value="1">Faculty In Charge</asp:ListItem>
            <asp:ListItem Value="2">Faculty</asp:ListItem>
        </asp:DropDownList>
        <br />
        <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="Change!!"  />
        <br />
    </asp:Panel>
    <asp:Panel ID="Panel5" runat="server" Visible="False" >
        Select User:
        <asp:DropDownList ID="UserDropDownList3" runat="server">
        </asp:DropDownList>
        <br />
        <asp:Button ID="Button4" runat="server" OnClick="Button4_Click" OnClientClick="return confirm('Are you sure?')" Text="Block" />
        <br />
    </asp:Panel>
    <asp:Panel ID="Panel6" runat="server" Visible="False">
        Select User To Delete:
        <asp:DropDownList ID="UserDropDownList4" runat="server">
        </asp:DropDownList>
        <br />
        <asp:Button ID="Button5" runat="server" Text="Delete" OnClientClick="return confirm('Are you sure you want to Delete this user?')" OnClick="Button5_Click"/>
        <br />
    </asp:Panel>
    <br />
</asp:Content>


