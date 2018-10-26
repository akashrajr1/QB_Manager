using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Web.Configuration;
using Enums;
using Exceptions;
using Methods;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HttpCookie cookie = Request.Cookies["userdata"];
            string username = Server.UrlDecode(Request.QueryString["Username"]);
            if (Session["username"] == null || username != Session["username"].ToString())
                throw new UserNotFound("Invalid User!!");
            else
            {
                Session["uid"] = cookie["uid"];
                Session["role"] = cookie["role"];
            }

            if (Session["flag"] != null)
            {
                int error = (int)Session["flag"];
                if (error == 1)
                    Alert.Generate(this, "Unable to delete as question exists as a part of a Question Paper. Please contact administrator");
                error = 0;
                Session["flag"] = error;
            }

        }
        catch (UserNotFound err)
        {
            Response.Redirect("http://localhost:60561/404.aspx?Error=" + Server.UrlEncode(err.Message));
        }
        catch
        {
        }
        GridView1.PageSize = 4;
    }

    public void SetToFalse()
    {
        Panel1.Visible = false;
        Panel2.Visible = false;
        Panel3.Visible = false;
    }

    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetToFalse();
        switch ((FacultyPower)RadioButtonList1.SelectedIndex)
        {
            case FacultyPower.CreateNewQuestion:
                LoadSubjects();
                //if(SubjectsDropDownList.Items.Count==1)
                    SubjectsDropDownList_SelectedIndexChanged(null, null);
                Panel1.Visible = true;
                break;
            case FacultyPower.DisplayAllQuestions:
                Panel3.Visible = true;
                UpdateGridView();
                UpdateGridViewMcqs();
                break;
        }
    }

    protected void CheckBoxList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (CheckBoxList1.SelectedIndex == 0)
            Panel2.Visible = true;
        else
            Panel2.Visible = false;
    }

    protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
    {
        string[] options = {TextBox2.Text, TextBox3.Text, TextBox4.Text, TextBox5.Text};
        if (options[0].Equals(options[1]) || options[0].Equals(options[2]) || options[0].Equals(options[3]))
            args.IsValid = false;
        else
            args.IsValid = true;
    }

    protected void CustomValidator2_ServerValidate(object source, ServerValidateEventArgs args)
    {
        string[] options = { TextBox2.Text, TextBox3.Text, TextBox4.Text, TextBox5.Text };
        if (options[1].Equals(options[0]) || options[1].Equals(options[2]) || options[1].Equals(options[3]))
            args.IsValid = false;
        else
            args.IsValid = true;
    }

    protected void CustomValidator3_ServerValidate(object source, ServerValidateEventArgs args)
    {
        string[] options = { TextBox2.Text, TextBox3.Text, TextBox4.Text, TextBox5.Text };
        if (options[2].Equals(options[0]) || options[2].Equals(options[1]) || options[2].Equals(options[3]))
            args.IsValid = false;
        else
            args.IsValid = true;
    }

    protected void CustomValidator4_ServerValidate(object source, ServerValidateEventArgs args)
    {
        string[] options = { TextBox2.Text, TextBox3.Text, TextBox4.Text, TextBox5.Text };
        if (options[3].Equals(options[0]) || options[3].Equals(options[1]) || options[3].Equals(options[2]))
            args.IsValid = false;
        else
            args.IsValid = true;
    }

    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["QB_Manager"].ConnectionString);
    SqlDataReader reader;
    SqlCommand cmd;

    public void LoadSubjects()
    {
        string uid = (string) Session["uid"];
        cmd = new SqlCommand("select distinct subject from subjects join teaches on subjects.subid=teaches.subid where uid=@uid",con);
        cmd.Parameters.AddWithValue("@uid", uid);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        SubjectsDropDownList.DataSource = ds;
        SubjectsDropDownList.DataValueField = "subject";
        SubjectsDropDownList.DataBind();
        SubjectsDropDownList.SelectedIndex = 0;
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string question = TextBox1.Text;
        string[] options = { TextBox2.Text, TextBox3.Text, TextBox4.Text, TextBox5.Text };
        string marks = TextBox6.Text;
        int ismcq=0;
        string uid = Session["uid"].ToString();
        string subid = Session["subid"].ToString();
        if (CheckBoxList1.SelectedIndex == 0) ismcq = 1;


        try
        {
            if (SubjectsDropDownList.Items.Count == 0) throw new NoSubjectFound("No Subjects Selected!!");
            con.Open();
            if (ismcq == 1)
            {
                cmd = new SqlCommand("insert into questions (ismcq,question,optiona,optionb,optionc,optiond,marks,uid,subid) values(1,@question,@optiona,@optionb,@optionc,@optiond,@marks,@uid,@subid)",con);
                cmd.Parameters.AddWithValue("@question", question);
                cmd.Parameters.AddWithValue("@optiona", options[0]);
                cmd.Parameters.AddWithValue("@optionb", options[1]);
                cmd.Parameters.AddWithValue("@optionc", options[2]);
                cmd.Parameters.AddWithValue("@optiond", options[3]);
                cmd.Parameters.AddWithValue("@marks", marks);
                cmd.Parameters.AddWithValue("@uid", uid);
                cmd.Parameters.AddWithValue("@subid", subid);
            }
            else
            {
                cmd = new SqlCommand("insert into questions (ismcq,question,marks,uid,subid) values(0,@question,@marks,@uid,@subid)",con);
                cmd.Parameters.AddWithValue("@question", question);
                cmd.Parameters.AddWithValue("@marks", marks);
                cmd.Parameters.AddWithValue("@uid", uid);
                cmd.Parameters.AddWithValue("@subid", subid);
            }
            int flag = cmd.ExecuteNonQuery();
            if (flag !=1)
                throw new QuestionNotUpdated("Question wasn't updated!!");
            else
                Alert.Generate(this, "New Question Created!!");

        }
        catch (QuestionNotUpdated err)
        {
            Alert.Generate(this,err.Message);
        }
        catch (NoSubjectFound err)
        {
            Alert.Generate(this, err.Message);
        }
        finally { con.Close(); }
    }

    public void UpdateGridView()
    {   
        string uid = Session["uid"].ToString();
        con.Close();
        con.Open();
        cmd = new SqlCommand("select qid,question,marks,subject from questions join subjects on subjects.subid=questions.subid where ismcq=0 and uid=@uid",con);
        cmd.Parameters.AddWithValue("@uid", uid);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        con.Close();
        GridView1.DataSource = ds;
        GridView1.DataBind();
    }

    public void UpdateGridViewMcqs()
    {
        string uid = Session["uid"].ToString();
        con.Open();
        cmd = new SqlCommand("select question,marks,optiona,optionb,optionc,optiond,subject from questions join subjects on subjects.subid=questions.subid where ismcq=1 and uid=@uid", con);
        cmd.Parameters.AddWithValue("@uid", uid);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        con.Close();
        GridView2.DataSource = ds;
        GridView2.DataBind();
    }

    protected void SubjectsDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        con.Open();
        cmd = new SqlCommand("select branch,semester,subid from subjects join branch on branch.branchid=subjects.branchid where subject=@subject", con);
        cmd.Parameters.AddWithValue("@subject", SubjectsDropDownList.SelectedValue);
        reader = cmd.ExecuteReader();
        string branch = ""; string semester = ""; string subid = "";
        while (reader.Read())
        {
            branch = (string)reader[0];
            semester = (string)reader[1];
            subid = ((int)reader[2]).ToString();
        }
        Branch.Text = branch;
        Semester.Text = semester;
        Session["subid"] = subid;
    }

    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView1.EditIndex = -1;
        UpdateGridView();
    }

    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView1.EditIndex = e.NewEditIndex;
        UpdateGridView();
    }

    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string uid = Session["uid"].ToString();
        Label qid = GridView1.Rows[e.RowIndex].FindControl("lbl_qid2") as Label;
        TextBox question = GridView1.Rows[e.RowIndex].FindControl("txt_question") as TextBox;
        TextBox marks = GridView1.Rows[e.RowIndex].FindControl("txt_marks") as TextBox;
        DropDownList subjects = GridView1.Rows[e.RowIndex].FindControl("SubjectsDropDownList2") as DropDownList;

        con.Open();
        SqlCommand cmd = new SqlCommand("update questions set question=@question, marks=@marks where qid=@qid", con);
        cmd.Parameters.AddWithValue("@question", question.Text);
        cmd.Parameters.AddWithValue("@marks", marks.Text);
        cmd.Parameters.AddWithValue("@qid", qid.Text);
        cmd.ExecuteNonQuery();

        cmd = new SqlCommand("update questions set subid=@subid where qid=@qid", con);
        cmd.Parameters.AddWithValue("@subid", GetSubjectId(subjects.SelectedValue));
        cmd.Parameters.AddWithValue("@qid", qid.Text);
        cmd.ExecuteNonQuery();

        con.Close();
        GridView1.EditIndex = -1;
        UpdateGridView();
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        UpdateGridView();
    }

    public string GetSubjectId(string subject)
    {
        con.Close();
        con.Open();
        SqlCommand cmd = new SqlCommand("select subid from subjects where subject=@subject", con);
        cmd.Parameters.AddWithValue("@subject", subject);
        reader = cmd.ExecuteReader();
        reader.Read();
        string subid=reader[0].ToString();
        reader.Close();
        return subid;
    }

    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int flag = 0;
        string uid = Session["uid"].ToString();
        Label qid = GridView1.Rows[e.RowIndex].FindControl("lbl_qid1") as Label;
        try
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("delete from questions where qid=@qid", con);

            cmd.Parameters.AddWithValue("@qid", qid.Text);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        catch
        {
            flag = 1;
            string username = Server.UrlDecode(Request.QueryString["Username"]);
            Session["flag"] = flag;
            Response.Redirect("home.aspx?Username=" + username);
        }
        GridView1.EditIndex = -1;
        UpdateGridView();
    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}