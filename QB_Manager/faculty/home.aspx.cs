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
            if (cookie == null || username != cookie["username"])
                throw new UserNotFound("Invalid User!!");
            else
            {
                Session["username"] = cookie["username"];
                Session["uid"] = cookie["uid"];
                Session["role"] = cookie["role"];
            }
        }
        catch (UserNotFound err)
        {
            Response.Redirect("http://localhost:60561/404.aspx?Error=" + Server.UrlEncode(err.Message));
        }
        Label1.Text = (string)Session["username"];
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

    protected void Button1_Click(object sender, EventArgs e)
    {
        string question = TextBox1.Text;
        string[] options = { TextBox2.Text, TextBox3.Text, TextBox4.Text, TextBox5.Text };
        string marks = TextBox6.Text;
        int ismcq=0;
        string uid = Session["uid"].ToString();
        if (CheckBoxList1.SelectedIndex == 0) ismcq = 1;

        try
        {
            con.Open();
            if (ismcq == 1)
            {
                cmd = new SqlCommand("insert into questions (ismcq,question,optiona,optionb,optionc,optiond,marks,uid) values(1,@question,@optiona,@optionb,@optionc,@optiond,@marks,@uid)",con);
                cmd.Parameters.AddWithValue("@question", question);
                cmd.Parameters.AddWithValue("@optiona", options[0]);
                cmd.Parameters.AddWithValue("@optionb", options[1]);
                cmd.Parameters.AddWithValue("@optionc", options[2]);
                cmd.Parameters.AddWithValue("@optiond", options[3]);
                cmd.Parameters.AddWithValue("@marks", marks);
                cmd.Parameters.AddWithValue("@uid", uid);
            }
            else
            {
                cmd = new SqlCommand("insert into questions (ismcq,question,marks,uid) values(0,@question,@marks,@uid)",con);
                cmd.Parameters.AddWithValue("@question", question);
                cmd.Parameters.AddWithValue("@marks", marks);
                cmd.Parameters.AddWithValue("@uid", uid);
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
        finally { con.Close(); }
    }

    public void UpdateGridView()
    {   
        string uid = Session["uid"].ToString();
        con.Open();
        cmd = new SqlCommand("select question,marks from questions where ismcq=0 and uid=@uid",con);
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
        cmd = new SqlCommand("select question,marks,optiona,optionb,optionc,optiond from questions where ismcq=1 and uid=@uid", con);
        cmd.Parameters.AddWithValue("@uid", uid);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        con.Close();
        GridView2.DataSource = ds;
        GridView2.DataBind();
    }
}