<%@ Page Title="" Language="C#" MasterPageFile="~/home.master" AutoEventWireup="true" CodeFile="home.aspx.cs" Inherits="incharge_home" Theme="master" %>


<asp:Content ID="Content2" ContentPlaceHolderID="Body" Runat="Server">
    <br />
    <br />

    <div style="width:300px; margin: auto;background-color:rgba(0,255,255,0.5);padding:10px">
        Select Operation to Perform: <br /> <br />
    <asp:RadioButtonList ID="RadioButtonList1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged">
        <asp:ListItem Value="0">Choose Final Questions</asp:ListItem>
        <asp:ListItem Value="1">View Question Paper</asp:ListItem>
        <asp:ListItem Value="2">None</asp:ListItem>
    </asp:RadioButtonList>
        </div>
    <br />
    <div >
    <asp:Panel ID="Panel1" runat="server" Visible="False" style="width:650px; margin: auto;background-color:rgba(192,192,192,0.7);padding:5px">
        Select Subject:
        <asp:DropDownList ID="SubjectDropDownList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="SubjectDropDownList_SelectedIndexChanged">
        </asp:DropDownList>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="SubjectDropDownList" ErrorMessage="Must Select A Subject!!"></asp:RequiredFieldValidator>
        <br />
        <br />
        MCQs:<br />
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False">
            <Columns>
                <asp:TemplateField HeaderText="Select">
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBox1" runat="server" /> 
                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("qid")%>'/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="question" HeaderText="Questions" SortExpression="question" />
                <asp:BoundField DataField="optiona" HeaderText="Option A" SortExpression="optiona" />
                <asp:BoundField DataField="optionb" HeaderText="Option B" SortExpression="optionb" />
                <asp:BoundField DataField="optionc" HeaderText="Option C" SortExpression="optionc" />
                <asp:BoundField DataField="optiond" HeaderText="Option D" SortExpression="optiond" />
                <asp:BoundField DataField="marks" HeaderText="Marks" SortExpression="marks" />
            </Columns>
        </asp:GridView> <br />
        Questions:<br />
        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField HeaderText="Select">
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBox2" runat="server" />
                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("qid")%>'/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="question" HeaderText="Questions" SortExpression="question" />
                <asp:BoundField DataField="marks" HeaderText="Marks" SortExpression="marks" />
            </Columns>
        </asp:GridView> <br />
        <asp:Button ID="Button1" runat="server" Text="Save Selection" OnClick="SaveCheckBoxState"/>
    </asp:Panel>

        </div>
    <asp:Panel ID="OutsidePanel" runat="server" Visible="false">

        <asp:Panel ID="Panel2" runat="server" BorderStyle="Double" BorderColor="OrangeRed" Visible="false" BackColor="White">
            <img alt="Logo" longdesc="Logo for question paper" src="../images/questionlogo.jpg" style="width: 960px; height: 131px" />
            <br />
            &nbsp;
            <asp:Label ID="Semester" runat="server"></asp:Label>
            &nbsp;Semester&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="Branch" runat="server"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Subject:&nbsp;
            <asp:Label ID="Subject" runat="server"></asp:Label>
            <br />
            <br />
            <b>&nbsp; <asp:Label ID="Label2" runat="server" Text="Attempt all questions"></asp:Label></b>
            <br />
            <br />
            <br />
            <%= GeneratePaper() %>
        </asp:Panel> <br />
    <asp:Button ID="Button2" runat="server" Text="Save Paper" OnClick="Button2_Click" />
    </asp:Panel>
    <br />
</asp:Content>


