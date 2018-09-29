using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enums;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Web.Configuration;



public partial class admin_home : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch ( (AdminPower) Convert.ToInt32(RadioButtonList1.SelectedValue))
        {
            case AdminPower.CreateNewUser: Panel1.Visible = true;
                    break;
        }
    }

    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["QB_Manager"].ConnectionString);
    SqlCommand cmd;

    protected void Button1_Click(object sender, EventArgs e)
    {
        string username = TextBox1.Text;
        string password = TextBox2.Text;
        int role = DropDownList1.SelectedIndex + 1;

        try
        {
            con.Open();

            cmd = new SqlCommand("insert into Users (username,password,role) values (@username,@password,@role)", con);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@role", role.ToString());
            int flag=cmd.ExecuteNonQuery();
            if (flag!=0)
                Response.Write(@"<script language='javascript'>alert('New User Created!!');</script>");

        }
        catch { }
        finally { con.Close(); }
    }
}