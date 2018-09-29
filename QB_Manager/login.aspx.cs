using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using Exceptions;

public partial class Login : System.Web.UI.Page
{
    static string conString = WebConfigurationManager.ConnectionStrings["QB_Manager"].ConnectionString;
    SqlConnection con = new SqlConnection(conString);
    SqlDataReader reader;
    SqlCommand cmd;
    
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string username = TextBox1.Text;
        string password = TextBox2.Text;
        try
        {
            con.Open();
            cmd = new SqlCommand("select username,password from Users",con);
            reader = cmd.ExecuteReader();
            bool userFound = false, passwordFound = false;
            while (reader.Read())
            {
                if(username == reader[0].ToString())
                {
                    userFound = true;
                    if (password == reader[1].ToString()) passwordFound = true;
                }
                
            }
            if (!userFound) throw new Exceptions.UserNameIsWrongException("User Name not found!!");
            if (!passwordFound) throw new Exceptions.PasswordIsWrongException("Password is incorrect!!");
            Response.Redirect("home.aspx");
        }
        catch(UserNameIsWrongException err)
        {
            Response.Write(@"<script language='javascript'>alert('" + err.Message + " .');</script>");
        }
        catch (PasswordIsWrongException err)
        {
            Response.Write(@"<script language='javascript'>alert('" + err.Message + " .');</script>");
        }
        finally
        {
            con.Close();
        }
    }
}