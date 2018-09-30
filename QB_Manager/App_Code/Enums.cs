using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Enums
/// </summary>
namespace Enums
{
    public enum Roles
    {
        Blocked=-1, Admin, Incharge, Faculty
    }

    public enum AdminPower
    {
        CreateNewUser, DisplayAllUsers, ChangeUserPassword, ChangeUserRole, BlockUser, DeleteUser
    }
}