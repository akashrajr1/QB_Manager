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
    }

    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetToFalse();
        switch ((InChargePower)RadioButtonList1.SelectedIndex)
        {
            case InChargePower.ChooseFinalQuestions:
                Panel1.Visible = true;
                UpdateGridViewMcqs();
                UpdateGridView();
                SetCheckBoxes();
                break;
            case InChargePower.ViewQuestionPaper:
                SaveCheckBoxState(null,null);
                GeneratePaper();
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
        cmd = new SqlCommand("select question,marks,optiona,optionb,optionc,optiond from questions where ismcq=1 order by marks asc", con);
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
        cmd = new SqlCommand("select question,marks from questions where ismcq=0 order by marks asc", con);
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
            Alert.Generate(this, "Questions Saved!!");
            HttpCookie cookie = Request.Cookies["userdata"];
            cookie["code1"] = code1;
            cookie["code2"] = code2;
            Response.SetCookie(cookie);
        }
    }

    public string GeneratePaper()
    {
        int mcqCount = GridView1.Rows.Count;
        HttpCookie cookie = Request.Cookies["userdata"];
        string questions = "";
        string code1 = cookie["code1"];
        if (code1 == null) return questions;

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
}