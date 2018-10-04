<%@ Page Title="" Language="C#" MasterPageFile="~/home.master" AutoEventWireup="true" CodeFile="home.aspx.cs" Inherits="incharge_home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="User" Runat="Server">
    <asp:Label ID="Label1" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" Runat="Server">
    <asp:RadioButtonList ID="RadioButtonList1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged">
        <asp:ListItem Value="0">Choose Final Questions</asp:ListItem>
        <asp:ListItem Value="1">View Question Paper</asp:ListItem>
        <asp:ListItem Value="2">None</asp:ListItem>
    </asp:RadioButtonList>
    <asp:Panel ID="Panel1" runat="server" Visible="False">
        <b>MCQs:</b><br />
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False">
            <Columns>
                <asp:TemplateField HeaderText="Select">
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBox1" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="question" HeaderText="Questions" SortExpression="question" />
                <asp:BoundField DataField="optiona" HeaderText="Option A" SortExpression="optiona" />
                <asp:BoundField DataField="optionb" HeaderText="Option B" SortExpression="optionb" />
                <asp:BoundField DataField="optionc" HeaderText="Option C" SortExpression="optionc" />
                <asp:BoundField DataField="optiond" HeaderText="Option D" SortExpression="optiond" />
                <asp:BoundField DataField="marks" HeaderText="Marks" SortExpression="marks" />
            </Columns>
        </asp:GridView>
        <b>Questions:</b><br />
        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField HeaderText="Select">
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBox2" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="question" HeaderText="Questions" SortExpression="question" />
                <asp:BoundField DataField="marks" HeaderText="Marks" SortExpression="marks" />
            </Columns>
        </asp:GridView>
        <asp:Button ID="Button1" runat="server" Text="Save Selection" OnClick="SaveCheckBoxState"/>
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server" BorderStyle="Double" BorderColor="OrangeRed" Visible="false">
        <img alt="Logo" longdesc="Logo for question paper" src="../images/questionlogo.jpg" style="width: 960px; height: 131px" />
        <br />
        <br />
        <br />
        <b><asp:Label ID="Label2" runat="server" Text="Attempt all questions"></asp:Label></b>
        <br />
        <br />
        <br />
        <%= GeneratePaper() %>
    </asp:Panel>
    <br />
</asp:Content>

