using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Exceptions
/// </summary>
namespace Exceptions
{
    public class UserNameIsWrongException: Exception
    {
        public UserNameIsWrongException(string msg): base(msg) { }
    }

    public class PasswordIsWrongException : Exception
    {
        public PasswordIsWrongException(string msg) : base(msg) { }
    }
}