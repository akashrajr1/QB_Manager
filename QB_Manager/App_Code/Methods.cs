using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Exceptions;

/// <summary>
/// Summary description for Methods
/// </summary>
namespace Methods
{
    public class Alert
    {
        public static void Generate(Page page,string msg)
        {
            page.Response.Write(@"<script language='javascript'>alert('"+msg+"');</script>");
        }
    }

}