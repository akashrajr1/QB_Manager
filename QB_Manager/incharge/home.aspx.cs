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
using System.Diagnostics;

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
                Session["subject"] = SubjectDropDownList.SelectedValue;
            }
        }
        catch (UserNotFound err)
        {
            Response.Redirect("http://localhost:60561/404.aspx?Error=" + Server.UrlEncode(err.Message));
        }
        GridView1.PageSize = 4;
        GridView2.PageSize = 4;
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
                if(SubjectDropDownList.Items.Count==1)
                    Session["subject"] = SubjectDropDownList.SelectedItem.Value;
                else
                    Session["subject"] = SubjectDropDownList.SelectedValue;

                PopulateSubjects();
                UpdateGridViewMcqs();
                UpdateGridView();
                PopulateList1();
                PopulateList2();
                Panel1.Visible = true;
                break;
            case InChargePower.ViewQuestionPaper:
                SaveList1();
                SaveList2();
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
        con.Close();
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

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveList1();
        GridView1.PageIndex = e.NewPageIndex;
        UpdateGridViewMcqs();
        PopulateList1();
    }

    private void PersistRowIndex1(int index)
    {
        if (!SelectedList1.Exists(i => i == index))
            SelectedList1.Add(index);
    }

    private void RemoveRowIndex1(int index)
    {
        if (SelectedList1.Exists(i => i == index))
            SelectedList1.Remove(index);
    }

    public List<Int32> SelectedList1
    {
        get
        {
            if (Session["List1"] == null)
                Session["List1"] = new List<Int32>();
            return (List<Int32>)Session["List1"];
        }
    }

    private void PopulateList1()
    {
        foreach (GridViewRow row in GridView1.Rows)
        {
            CheckBox checkBox = row.FindControl("CheckBox1") as CheckBox;
            IDataItemContainer container = checkBox.NamingContainer as IDataItemContainer;
            if (SelectedList1 != null)
                if (SelectedList1.Exists(i => i == container.DataItemIndex))
                    checkBox.Checked = true;
        }

    }

    private void SaveList1()
    {
        foreach (GridViewRow row in GridView1.Rows)
        {
            CheckBox checkBox = row.FindControl("CheckBox1") as CheckBox;
            IDataItemContainer container = checkBox.NamingContainer as IDataItemContainer;
            if (checkBox.Checked)
                PersistRowIndex1(container.DataItemIndex);
            else
                RemoveRowIndex1(container.DataItemIndex);
        }
    }

    protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveList2();
        GridView2.PageIndex = e.NewPageIndex;
        UpdateGridView();
        PopulateList2();
    }

    public List<Int32> SelectedList2
    {
        get
        {
            if (Session["List2"] == null)
                Session["List2"] = new List<Int32>();
            return (List<Int32>)Session["List2"];
        }
    }

    private void PersistRowIndex2(int index)
    {
        if (!SelectedList2.Exists(i => i == index))
            SelectedList2.Add(index);
    }

    private void RemoveRowIndex2(int index)
    {
        if (SelectedList2.Exists(i => i == index))
            SelectedList2.Remove(index);
    }

    private void PopulateList2()
    {
        foreach (GridViewRow row in GridView2.Rows)
        {
            CheckBox checkBox = row.FindControl("CheckBox1") as CheckBox;
            IDataItemContainer container = checkBox.NamingContainer as IDataItemContainer;
            if (SelectedList2 != null)
                if (SelectedList2.Exists(i => i == container.DataItemIndex))
                    checkBox.Checked = true;
        }

    }

    private void SaveList2()
    {
        foreach (GridViewRow row in GridView2.Rows)
        {
            CheckBox checkBox = row.FindControl("CheckBox1") as CheckBox;
            IDataItemContainer container = checkBox.NamingContainer as IDataItemContainer;
            if (checkBox.Checked)
                PersistRowIndex2(container.DataItemIndex);
            else
                RemoveRowIndex2(container.DataItemIndex);
        }
    }

    public string GeneratePaper()
    {
        

        HttpCookie cookie = Request.Cookies["userdata"];
        string questions = "";

        Subject.Text = Session["subject"].ToString();
        Branch.Text = GetData(0);
        Semester.Text = GetData(1);

        int rowIndex = 0, count=1;
        if (Session["CurrentList1"] != null)
        {
            questions += "<div style='margin-left: 20px;'><b>Select the correct MCQs</b><br><br>";

            List<Int32> CurrentList1 = (List<Int32>) Session["CurrentList1"];
            GridView1.AllowPaging = false;
            UpdateGridViewMcqs();
            foreach (GridViewRow row in GridView1.Rows)
            {
                if (CurrentList1.Exists(i => i == row.DataItemIndex))
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
            UpdateGridViewMcqs();
            GridView1.AllowPaging = true;
        }

        if (Session["CurrentList2"] != null)
        {
            questions += "<b>Answer in brief</b><br><br>";
            rowIndex = 0; count = 1;

            List<Int32> CurrentList2 = (List<Int32>)Session["CurrentList2"];
            GridView2.AllowPaging = false;
            UpdateGridView();
            foreach (GridViewRow row in GridView2.Rows)
            {
                if (CurrentList2.Exists(i => i == row.DataItemIndex))
                {
                    string sno = count.ToString() + ". ";
                    string question = row.Cells[1].Text;
                    string marks = row.Cells[2].Text;
                    questions += sno + "  " + question + "<br>(" + marks + ")<br><br>";
                    count++;
                }
                rowIndex++;
            }
            UpdateGridView();
            GridView2.AllowPaging = true;
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
        Session["subject"] = SubjectDropDownList.SelectedValue;
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
        if (Session["CurrentList1"] == null && Session["CurrentList2"] == null) return;

        string uid = Session["uid"].ToString();
        string subid = GetData(2);
        cmd = new SqlCommand("insert into Papers (subid) output inserted.paperid values(@subid)", con);
        cmd.Parameters.AddWithValue("@subid", subid);
        int paperid = (int)cmd.ExecuteScalar();

        GridView1.AllowPaging = false;
        UpdateGridViewMcqs();
        if (Session["CurrentList1"] != null)
        {
            List<Int32> CurrentList1 = (List<Int32>)Session["CurrentList1"];

            foreach (GridViewRow row in GridView1.Rows)
            {
                if (CurrentList1.Exists(i => i == row.DataItemIndex))
                {
                    HiddenField hidden = (HiddenField)row.Cells[0].FindControl("HiddenField1");
                    string questionid = hidden.Value.ToString();
                    con.Open();
                    cmd = new SqlCommand("insert into PaperDb (paperid,questionid) values(@paperid,@questionid)", con);
                    cmd.Parameters.AddWithValue("@paperid", paperid);
                    cmd.Parameters.AddWithValue("@questionid", questionid);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

        }
        UpdateGridViewMcqs();
        GridView1.AllowPaging = false;

        GridView2.AllowPaging = false;
        UpdateGridView();
        if (Session["CurrentList2"] != null)
        {
            List<Int32> CurrentList2 = (List<Int32>)Session["CurrentList2"];
            foreach (GridViewRow row in GridView2.Rows)
            {
                if (CurrentList2.Exists(i => i == row.DataItemIndex))
                {
                    HiddenField hidden = (HiddenField)row.Cells[0].FindControl("HiddenField1");
                    string questionid = hidden.Value.ToString();
                    con.Open();
                    cmd = new SqlCommand("insert into PaperDb (paperid,questionid) values(@paperid,@questionid)", con);
                    cmd.Parameters.AddWithValue("@paperid", paperid);
                    cmd.Parameters.AddWithValue("@questionid", questionid);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        UpdateGridView();
        GridView2.AllowPaging = false;
        con.Close();
        Alert.Generate(this, "Question Paper is Saved");
    }

    protected void SaveSelection(object sender, EventArgs e)
    {
        SaveList1();
        SaveList2();
        Session["CurrentList1"] = new List<Int32>((List<Int32>)Session["List1"]);
        Session["CurrentList2"] = new List<Int32>((List<Int32>)Session["List2"]);
    }
}