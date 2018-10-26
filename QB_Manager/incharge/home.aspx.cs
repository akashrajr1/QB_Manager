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

public partial class incharge_home : System.Web.UI.Page
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
        }
        catch (UserNotFound err)
        {
            Response.Redirect("http://localhost:60561/404.aspx?Error=" + Server.UrlEncode(err.Message));
        }
    }

    public void SetToFalse()
    {
        Panel1.Visible = false;
        Panel2.Visible = false;
        OutsidePanel.Visible = false;
    }

    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetToFalse();
        switch ((InChargePower)RadioButtonList1.SelectedIndex)
        {
            case InChargePower.ChooseFinalQuestions:
                PopulateSubjects();
                UpdateGridViewMcqs();
                UpdateGridView();
                SetCheckBoxes();
                Panel1.Visible = true;
                break;
            case InChargePower.ViewQuestionPaper:
                SaveCheckBoxState(null,null);
                GeneratePaper();
                OutsidePanel.Visible = true;
                Panel2.Visible = true;
                break;
        }
    }

    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["QB_Manager"].ConnectionString);
    SqlDataReader reader;
    SqlCommand cmd;

    public void UpdateGridViewMcqs()
    {
        con.Open();
        cmd = new SqlCommand("select question,marks,optiona,optionb,optionc,optiond,qid from questions join subjects on questions.subid=subjects.subid where ismcq=1 and subject=@subject order by marks asc", con);
        cmd.Parameters.AddWithValue("@subject", SubjectDropDownList.SelectedValue);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        con.Close();
        GridView1.DataSource = ds;
        GridView1.DataBind();
    }

    public void UpdateGridView()
    {
        con.Open();
        cmd = new SqlCommand("select question,marks,qid from questions join subjects on questions.subid=subjects.subid where ismcq=0 and subject=@subject order by marks asc", con);
        cmd.Parameters.AddWithValue("@subject", SubjectDropDownList.SelectedValue);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        con.Close();
        GridView2.DataSource = ds;
        GridView2.DataBind();
    }

    public void SetCheckBoxes()
    {
        string code1 = (string) Session["code1"];
        string code2 = (string) Session["code2"];
        int i = 0;


        try
        {
            if (code1 == null || code2 == null) ;
            else
            {
                i = 0;
                foreach (GridViewRow row in GridView1.Rows)
                {
                    CheckBox box = (CheckBox)row.Cells[0].FindControl("CheckBox1");
                    if (code1[i].Equals('1'))
                        box.Checked = true;
                    i++;
                }
                i = 0;
                foreach (GridViewRow row in GridView2.Rows)
                {
                    CheckBox box = (CheckBox)row.Cells[0].FindControl("CheckBox2");
                    if (code2[i].Equals('1'))
                        box.Checked = true;
                    i++;
                }
            }
        }
        catch(Exception err)
        {
        }

    }

    protected void SaveCheckBoxState(object sender, EventArgs e)
    {
        string code1 = "";
        string code2 = "";
        foreach (GridViewRow row in GridView1.Rows)
        {
                CheckBox box = (CheckBox) row.Cells[0].FindControl("CheckBox1");
                if (box.Checked)
                    code1 += "1";
                else
                    code1 += "0";
        }
        foreach (GridViewRow row in GridView2.Rows)
        {
            CheckBox box = (CheckBox)row.Cells[0].FindControl("CheckBox2");
            if (box.Checked)
                code2 += "1";
            else
                code2 += "0";
        }
        Session["code1"] = code1;
        Session["code2"] = code2;

        Button button = (Button)sender;
        if (button != null)
        {
            Alert.Generate(this, "Questions Paper Generated!!");
            HttpCookie cookie = Request.Cookies["userdata"];
            cookie["code1"] = code1;
            cookie["code2"] = code2;
            Response.SetCookie(cookie);
        }

        Session["subject"] = SubjectDropDownList.SelectedValue;
    }

    public string GeneratePaper()
    {
        int mcqCount = GridView1.Rows.Count;
        HttpCookie cookie = Request.Cookies["userdata"];
        string questions = "";
        string code1 = cookie["code1"];
        if (code1 == null) return questions;

        Subject.Text = Session["subject"].ToString();
        Branch.Text = GetData(0);
        Semester.Text = GetData(1);

        questions += "<div style='margin-left: 20px;'><b>Select the correct MCQs</b><br><br>";
        int rowIndex = 0, count=1;
        foreach (GridViewRow row in GridView1.Rows)
        {
            if (code1[rowIndex].Equals('1'))
            {
                string sno = count.ToString() + ". ";
                string question = row.Cells[1].Text;
                string optiona = row.Cells[2].Text;
                string optionb = row.Cells[3].Text;
                string optionc = row.Cells[4].Text;
                string optiond = row.Cells[5].Text;
                string marks = row.Cells[6].Text;
                questions += sno + "  " + question + "<br>a. " + optiona + "<br>b. " + optionb + "<br>c. " + optionc + "<br>d. " + optiond + "<br>(" + marks + ")<br><br>";
                count++;
            }
            rowIndex++;
        }

        string code2 = cookie["code2"];
        if (code2 == null) return questions += "</div>";

        questions += "<b>Answer in brief</b><br><br>";
        rowIndex = 0; count = 1;
        foreach (GridViewRow row in GridView2.Rows)
        {
            if (code2[rowIndex].Equals('1'))
            {
                string sno = count.ToString() + ". ";
                string question = row.Cells[1].Text;
                string marks = row.Cells[2].Text;
                questions += sno + "  " + question +"<br>(" + marks + ")<br><br>";
                count++;
            }
            rowIndex++;
        }

        questions += "</div>";
        return questions;
    }

    public void PopulateSubjects()
    {
        string uid = Session["uid"].ToString();
        cmd = new SqlCommand("select subject from subjects join teaches on subjects.subid=teaches.subid where uid=@uid", con);
        cmd.Parameters.AddWithValue("@uid", uid);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        SubjectDropDownList.DataSource = ds;
        SubjectDropDownList.DataValueField = "subject";
        SubjectDropDownList.DataBind();
    }

    protected void SubjectDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateGridViewMcqs();
        UpdateGridView();
    }

    public string GetData(int select)
    {
        string subject = Session["subject"].ToString();
        con.Close();
        con.Open();
        cmd = new SqlCommand("select branch,semester,subid from branch join subjects on branch.branchid=subjects.branchid where subject=@subject", con);
        cmd.Parameters.AddWithValue("@subject", subject);
        reader = cmd.ExecuteReader();
        string branch = "", semester = "", subid = "";
        while (reader.Read())
        {
            branch = reader[0].ToString();
            semester = reader[1].ToString();
            subid = reader[2].ToString();
        }
        reader.Close();
        switch (select)
        {
            case 0: return branch;
            case 1: return semester;
            case 2: return subid;
        }
        return "null";
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        string uid = Session["uid"].ToString();
        string subid = GetData(2);
        cmd = new SqlCommand("insert into Papers (subid) output inserted.paperid values(@subid)", con);
        cmd.Parameters.AddWithValue("@subid", subid);
        int paperid = (int)cmd.ExecuteScalar();
        foreach(GridViewRow row in GridView1.Rows)
        {
            CheckBox box = (CheckBox)row.Cells[0].FindControl("CheckBox1");
            if (box.Checked)
            {
                HiddenField hidden = (HiddenField)row.Cells[0].FindControl("HiddenField1");
                string questionid = hidden.Value.ToString();
                cmd = new SqlCommand("insert into PaperDb (paperid,questionid) values(@paperid,@questionid)", con);
                cmd.Parameters.AddWithValue("@paperid", paperid);
                cmd.Parameters.AddWithValue("@questionid", questionid);
                cmd.ExecuteNonQuery();
            }
        }
        foreach (GridViewRow row in GridView2.Rows)
        {
            CheckBox box = (CheckBox)row.Cells[0].FindControl("CheckBox2");
            if (box.Checked)
            {
                HiddenField hidden = (HiddenField)row.Cells[0].FindControl("HiddenField1");
                string questionid = hidden.Value.ToString();
                cmd = new SqlCommand("insert into PaperDb (paperid,questionid) values(@paperid,@questionid)", con);
                cmd.Parameters.AddWithValue("@paperid", paperid);
                cmd.Parameters.AddWithValue("@questionid", questionid);
                cmd.ExecuteNonQuery();
            }
        }
        Alert.Generate(this, "Question Paper is Saved");
    }
}