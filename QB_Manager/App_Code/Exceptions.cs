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

    public class NewUserNotCreated : Exception
    {
        public NewUserNotCreated(string msg) : base(msg) { }
    }

    public class PasswordNotChanged : Exception
    {
        public PasswordNotChanged(string msg) : base(msg) { }
    }

    public class RoleNotChanged : Exception
    {
        public RoleNotChanged(string msg) : base(msg) { }
    }

    public class UserNotBlocked : Exception
    {
        public UserNotBlocked(string msg) : base(msg) { }
    }

    public class UserNotDeleted : Exception
    {
        public UserNotDeleted(string msg) : base(msg) { }
    }

    public class QuestionNotUpdated : Exception
    {
        public QuestionNotUpdated(string msg) : base(msg) { }
    }

    public class UserNotFound : Exception
    {
        public UserNotFound(string msg) : base(msg) { }
    }
}