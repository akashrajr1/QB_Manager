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
        Enter New Username : <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox><br />
        Enter New Password : <asp:TextBox ID="TextBox2" runat="server" TextMode="Password"></asp:TextBox><br />
        <asp:Button ID="Button1" runat="server" Text="Create" OnClick="Button1_Click" />
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server" Width="190px" Visible="False">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="username" DataSourceID="SqlDataSource1" AllowPaging="True" AllowSorting="True" PageSize="5" EnableSortingAndPagingCallbacks="True">
            <Columns>
                <asp:BoundField DataField="username" HeaderText="Username" ReadOnly="True" SortExpression="username" />
                <asp:BoundField DataField="role" HeaderText="Role" SortExpression="role" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:QB_ManagerConnectionString %>" SelectCommand="SELECT [username], [role] FROM [Users] WHERE ([role] &lt;&gt; @role)">
            <SelectParameters>
                <asp:Parameter DefaultValue="0" Name="role" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
</asp:Panel>
    <asp:Panel ID="Panel3" runat="server" Visible="False">
        Select User:
        <asp:DropDownList ID="DropDownList2" runat="server" DataSourceID="SqlDataSource2" DataTextField="username" DataValueField="username">
            <asp:ListItem>None</asp:ListItem>
        </asp:DropDownList>
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:QB_ManagerConnectionString %>" SelectCommand="SELECT [username] FROM [Users] WHERE ([role] &lt;&gt; @role2)">
            <SelectParameters>
                <asp:Parameter DefaultValue="0" Name="role2" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
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
        <asp:DropDownList ID="DropDownList3" runat="server" DataSourceID="SqlDataSource3" DataTextField="username" DataValueField="username" AutoPostBack="True" OnSelectedIndexChanged="DropDownList3_SelectedIndexChanged">
            <asp:ListItem>None</asp:ListItem>
        </asp:DropDownList>
        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:QB_ManagerConnectionString %>" SelectCommand="SELECT [username] FROM [Users] WHERE ([role] &lt;&gt; @role)">
            <SelectParameters>
                <asp:Parameter DefaultValue="0" Name="role" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
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
        <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="Change!!" />
        <br />
    </asp:Panel>
    <asp:Panel ID="Panel5" runat="server" Visible="False" >
        Select User:
        <asp:DropDownList ID="DropDownList5" runat="server" DataSourceID="SqlDataSource4" DataTextField="username" DataValueField="username" >
        </asp:DropDownList>
        <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:QB_ManagerConnectionString %>" SelectCommand="SELECT [username] FROM [Users] WHERE ([role] &gt; @role)">
            <SelectParameters>
                <asp:Parameter DefaultValue="0" Name="role" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
        <br />
        <asp:Button ID="Button4" runat="server" OnClick="Button4_Click" OnClientClick="return confirm('Are you sure?')" Text="Block" />
        <br />
    </asp:Panel>
    <asp:Panel ID="Panel6" runat="server" Visible="False">
        Select User To Delete:
        <asp:DropDownList ID="DropDownList6" runat="server" DataSourceID="SqlDataSource5" DataTextField="username" DataValueField="username">
        </asp:DropDownList>
        <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:QB_ManagerConnectionString %>" SelectCommand="SELECT [username] FROM [Users] WHERE ([role] &lt;&gt; @role)">
            <SelectParameters>
                <asp:Parameter DefaultValue="0" Name="role" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:Button ID="Button5" runat="server" Text="Delete" OnClientClick="return confirm('Are you sure you want to Delete this user?')" OnClick="Button5_Click"/>
        <br />
    </asp:Panel>
    <br />
</asp:Content>


