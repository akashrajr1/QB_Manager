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
using Enums;
using Methods;

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
        int role = 2;
        try
        {
            con.Open();
            cmd = new SqlCommand("select username,password,role from users",con);
            reader = cmd.ExecuteReader();
            bool userFound = false, passwordFound = false;
            while (reader.Read())
            {
                if(username == reader[0].ToString())
                {
                    userFound = true;
                    if (password == reader[1].ToString()) passwordFound = true;
                    role = (int)reader[2];
                    break;
                }
                
            }
            if (!userFound) throw new UserNameIsWrongException("User Name not found!!");
            if (!passwordFound) throw new PasswordIsWrongException("Password is incorrect!!");
            switch ((Roles)role)
            {
                case Roles.Admin: Response.Redirect("admin/home.aspx");
                    break;
                case Roles.Incharge: Response.Redirect("incharge/home.aspx");
                    break;
                case Roles.Faculty : Response.Redirect("faculty/home.aspx");
                    break;
                case Roles.Blocked: Response.Redirect("blocked.aspx");
                    break;
            }
        }
        catch(UserNameIsWrongException err)
        {
            Alert.Generate(this,err.Message);
        }
        catch (PasswordIsWrongException err)
        {
            Alert.Generate(this, err.Message);
        }
        finally
        {
            con.Close();
        }
    }
}