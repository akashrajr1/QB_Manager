<%@ Page Title="" Language="C#" MasterPageFile="~/home.master" AutoEventWireup="true" CodeFile="home.aspx.cs" Inherits="_Default"  Theme="master" %>


<asp:Content ID="Content2" ContentPlaceHolderID="Body" Runat="Server">
     <br />
    <br />

    <div style="width:300px; margin: auto;background-color:rgba(0,255,255,0.5);padding:10px">
    Select Operation to Perform: <br /> <br />
        <asp:RadioButtonList ID="RadioButtonList1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged">
        <asp:ListItem Value="0">Add new Question</asp:ListItem>
        <asp:ListItem Value="1">See Current Questions</asp:ListItem>
    </asp:RadioButtonList>
        </div>
    <br />
    <div >
    <asp:Panel ID="Panel1" runat="server" Visible="False" style="width:650px; margin: auto;background-color:rgba(192,192,192,0.7);padding:5px">
        Subject:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:DropDownList ID="SubjectsDropDownList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="SubjectsDropDownList_SelectedIndexChanged" >
        </asp:DropDownList>
        &nbsp;Semester:
        <asp:Label ID="Semester" runat="server"></asp:Label>
        &nbsp;Branch:
        <asp:Label ID="Branch" runat="server"></asp:Label>
        &nbsp;<br /> Enter Question:
        <asp:TextBox ID="TextBox1" runat="server" Width="468px"></asp:TextBox> <br />
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
        <br /> <br />
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Submit" />
    </asp:Panel>
        </div>
        <div >
    <asp:Panel ID="Panel3" runat="server"  Visible="false" style="width:650px; margin: auto;background-color:rgba(192,192,192,0.7);padding:5px">
        MCQs:
        <br />
        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="question" HeaderText="Questions"/>
                <asp:BoundField DataField="optiona" HeaderText="Option A"/>
                <asp:BoundField DataField="optionb" HeaderText="Option B"/>
                <asp:BoundField DataField="optionc" HeaderText="Option C"/>
                <asp:BoundField DataField="optiond" HeaderText="Option D"/>
                <asp:BoundField DataField="marks" HeaderText="Marks" SortExpression="marks"/>
                <asp:BoundField DataField="subject" HeaderText="Subject" SortExpression="subject"/>
            </Columns>
        </asp:GridView> <br />
        Questions:
        <br />
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating" OnRowDeleting="GridView1_RowDeleting"
            AllowPaging="true" PageSize="2" OnPageIndexChanging="GridView1_PageIndexChanging" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" >

            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide">  
                    <ItemTemplate>  
                        <asp:Label ID="lbl_qid1" runat="server" Text='<%#Eval("qid") %>' Visible="false"></asp:Label>  
                    </ItemTemplate>  
                    <EditItemTemplate>  
                        <asp:Label ID="lbl_qid2" runat="server" Text='<%#Eval("qid") %>' Visible="false"></asp:Label>  
                    </EditItemTemplate>
                </asp:TemplateField> 
                <asp:TemplateField HeaderStyle-CssClass="nullcol" ItemStyle-CssClass="nullcol">
                    <ItemTemplate>
                        <asp:Button ID="EditButton1" runat="server" Text="Edit" CommandName="Edit" />
                        <asp:Button ID="DeleteButton1" runat="server" Text="Delete" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to Delete this question?')"/>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Button ID="UpdateButton1" runat="server" Text="Update" CommandName="Update" />
                        <asp:Button ID="CancelButton1" runat="server" Text="Cancel" CommandName="Cancel" />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Questions">  
                    <ItemTemplate>  
                        <asp:Label ID="lbl_question" runat="server" Text='<%#Eval("question") %>' Wrap="true"></asp:Label>  
                    </ItemTemplate>  
                    <EditItemTemplate>  
                        <asp:TextBox ID="txt_question" runat="server" Text='<%#Eval("question") %>' Wrap="true"></asp:TextBox>  
                    </EditItemTemplate>
                </asp:TemplateField>  
                <asp:TemplateField HeaderText="Marks">  
                    <ItemTemplate>  
                        <asp:Label ID="lbl_marks" runat="server" Text='<%#Eval("marks") %>'></asp:Label>  
                    </ItemTemplate>  
                    <EditItemTemplate>  
                        <asp:TextBox ID="txt_marks" runat="server" Text='<%#Eval("marks") %>'></asp:TextBox>  
                    </EditItemTemplate>  
                </asp:TemplateField>  
                <asp:TemplateField HeaderText="Subjects">  
                    <ItemTemplate>  
                        <asp:Label ID="lbl_subject1" runat="server" Text='<%#Eval("subject") %>'></asp:Label>  
                    </ItemTemplate>  
                    <EditItemTemplate>  
                        <asp:Label ID="lbl_subject2" runat="server" Text='<%#Eval("subject") %>'></asp:Label>  
<%--                        <asp:DropDownList ID="SubjectsDropDownList2" runat="server" DataSourceID="SqlDataSource1" DataTextField="subject">

                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:QB_ManagerConnectionString %>" SelectCommand="select distinct subject from subjects join teaches on subjects.subid=teaches.subid where uid=@uid" >
                             <SelectParameters>
                                <asp:SessionParameter Name="uid" SessionField="uid"/>
                            </SelectParameters>
                        </asp:SqlDataSource>--%>
                    </EditItemTemplate>  
                </asp:TemplateField>  
            </Columns>
            <PagerSettings FirstPageText="First" LastPageText="Last" />
        </asp:GridView>

        <br />

    </asp:Panel>
            </div>
&nbsp;

</asp:Content>

