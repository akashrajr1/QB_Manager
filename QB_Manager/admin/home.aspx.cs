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
using Exceptions;
using Methods;


public partial class admin_home : System.Web.UI.Page
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
        Panel3.Visible = false;
        Panel4.Visible = false;
        Panel5.Visible = false;
        Panel6.Visible = false;
    }

    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetToFalse();
        switch ( (AdminPower) Convert.ToInt32(RadioButtonList1.SelectedValue))
        {
            case AdminPower.CreateNewUser:
                PopulateSubjects();
                Panel1.Visible = true;
                break;
            case AdminPower.DisplayAllUsers:
                PopulateUserDetails();
                Panel2.Visible = true;
                break;
            case AdminPower.ChangeUserPassword:
                PopulateUserDropDownList1();
                Panel3.Visible = true;
                break;
            case AdminPower.ChangeUserRole:
                PopulateUserDropDownList2(UserDropDownList2);
                Panel4.Visible = true;
                break;
            case AdminPower.BlockUser:
                PopulateUserDropDownList3();
                Panel5.Visible = true;
                break;
            case AdminPower.DeleteUser:
                PopulateUserDropDownList2(UserDropDownList4);
                Panel6.Visible = true;
                break;
        }
    }

    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["QB_Manager"].ConnectionString);
    SqlCommand cmd;
    SqlDataReader reader;

    public void PopulateSubjects()
    {
        con.Open();
        cmd = new SqlCommand("select subject,branch,semester,subid from subjects join branch on branch.branchid=subjects.branchid", con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        con.Close();
        SubjectGridView.DataSource = ds;
        SubjectGridView.DataBind();
    }

    public void PopulateUserDetails()
    {
        con.Open();
        cmd = new SqlCommand("select username,Roles.role from users join roles on users.role=roles.roleid where roles.roleid <> 0", con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        con.Close();
        UserGridView.DataSource = ds;
        UserGridView.DataBind();
    }

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

            int count = 0;
            foreach (GridViewRow row in SubjectGridView.Rows)
            {
                CheckBox box = (CheckBox)row.Cells[1].FindControl("CheckBox1");
                if (box.Checked == true)
                    count++;
            }
            if (count == 0)
                throw new NoSubjectChecked("Please Select A Subject!!");

            int flag = cmd.ExecuteNonQuery();
            cmd = new SqlCommand("select uid from users where username=@username", con);
            cmd.Parameters.AddWithValue("@username", username);
            reader = cmd.ExecuteReader();
            string uid = "";
            while (reader.Read())
                uid = ((int)reader[0]).ToString();
            con.Close();
            con.Open();
            foreach (GridViewRow row in SubjectGridView.Rows)
            {
                CheckBox box = (CheckBox)row.Cells[1].FindControl("CheckBox1");
                if (box.Checked == true)
                {
                    cmd = new SqlCommand("insert into Teaches(uid,subid) values(@uid,@subid)", con);
                    cmd.Parameters.AddWithValue("@uid", uid);
                    cmd.Parameters.AddWithValue("@subid", row.Cells[0].Text);
                    flag=cmd.ExecuteNonQuery();
                }
            }
            if (flag != 0)
                Alert.Generate(this, "New User Created!!");
            else
                throw new NewUserNotCreated("User Not Created");

        }
        catch (NewUserNotCreated err)
        {
            Alert.Generate(this, err.Message);
        }
        catch (NoSubjectChecked err)
        {
            Alert.Generate(this, err.Message);
        }
        catch (SqlException err)
        {
            Alert.Generate(this, "Username alreadys exists!!");
        }
        finally { con.Close(); }
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        string username = UserDropDownList.SelectedValue;
        string password = TextBox3.Text;
        try
        {
            con.Open();
            cmd = new SqlCommand("update users set password=@password where username=@username", con);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@username", username);
            int flag = cmd.ExecuteNonQuery();
            if (flag != 0)
                Alert.Generate(this, "Password Changed!!");
            else
                throw new PasswordNotChanged("Password didn't change!!");
        }
        catch (PasswordNotChanged err)
        {
            Alert.Generate(this, err.Message);
        }
        finally { con.Close(); }
    }

    protected void UserDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        string username = UserDropDownList2.SelectedValue;
        try
        {
            con.Open();
            cmd = new SqlCommand("select role from users where username=@username", con);
            cmd.Parameters.AddWithValue("@username", username);
            reader = cmd.ExecuteReader();

            while (reader.Read())
                Label2.Text = ((Roles) reader[0]).ToString();
        }
        catch (Exception err)
        {
            Alert.Generate(this, err.Message);
        }
        finally { con.Close(); }
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        string username = UserDropDownList2.SelectedValue;
        int role = DropDownList4.SelectedIndex+1;
        try
        {
            con.Open();
            cmd = new SqlCommand("update users set role=@role where username=@username", con);
            cmd.Parameters.AddWithValue("@role", role.ToString());
            cmd.Parameters.AddWithValue("@username", username);
            if (cmd.ExecuteNonQuery() < 1)
                throw new RoleNotChanged("The role change did not go through!!");
            else
            {
                Alert.Generate(this, "Role Updated!!");
                Label2.Text = ((Roles)role).ToString();
            }
                
        }
        catch(Exception err)
        {
            Alert.Generate(this, err.Message);
        }
        finally { con.Close(); }
    }

    protected void Button4_Click(object sender, EventArgs e)
    {
        string username = UserDropDownList3.SelectedValue;
        try
        {
            con.Open();
            cmd = new SqlCommand("update users set role=-1 where username=@username", con);
            cmd.Parameters.AddWithValue("@username", username);
            if (cmd.ExecuteNonQuery() < 1)
                throw new UserNotBlocked("User wasn't blocked!!");
            else
            {
                Alert.Generate(this, username + " is blocked now!!");
            }
        }
        catch(UserNotBlocked err)
        {
            Alert.Generate(this, err.Message);
        }
        finally { con.Close(); }
    }

    protected void Button5_Click(object sender, EventArgs e)
    {
        string username = UserDropDownList4.SelectedValue;
        try
        {
            con.Open();
            cmd = new SqlCommand("select uid from users where username=@username", con);
            cmd.Parameters.AddWithValue("@username", username);
            reader = cmd.ExecuteReader();
            string uid = "";
            while (reader.Read())
                uid = ((int)reader[0]).ToString();
            reader.Close();
            cmd = new SqlCommand("delete from teaches where uid=@uid", con);
            cmd.Parameters.AddWithValue("@uid", uid);
            cmd.ExecuteNonQuery();

            cmd = new SqlCommand("delete from users where username=@username", con);
            cmd.Parameters.AddWithValue("@username", username);
            int x = cmd.ExecuteNonQuery();
            if (x < 1)
                throw new UserNotDeleted("User was not deleted!!");
            else
                Alert.Generate(this, username + " Account is now Deleted!!");
        }
        catch(UserNotDeleted err)
        {
            Alert.Generate(this, err.Message);
        }
        finally { con.Close(); }
    }

    protected void SubjectGridViewPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SubjectGridView.PageIndex = e.NewPageIndex;
        PopulateSubjects();
    }

    protected void UserGridViewPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        UserGridView.PageIndex = e.NewPageIndex;
        PopulateUserDetails();
    }

    public void PopulateUserDropDownList1()
    {
        con.Open();
        cmd = new SqlCommand("select username from users", con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        UserDropDownList.DataSource = ds;
        UserDropDownList.DataTextField = "username";
        UserDropDownList.DataBind();
    }

    public void PopulateUserDropDownList2(DropDownList list)
    {
        con.Open();
        cmd = new SqlCommand("select username from users where role<>0", con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        list.DataSource = ds;
        list.DataTextField = "username";
        list.DataBind();
    }

    public void PopulateUserDropDownList3()
    {
        con.Open();
        cmd = new SqlCommand("select username from users where role>0", con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        UserDropDownList3.DataSource = ds;
        UserDropDownList3.DataTextField = "username";
        UserDropDownList3.DataBind();
    }
}