using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Strings
{
    public static class StringValues
    {
        //Role Values
        public const string UserRole = "User";
        public const string AdminRole = "Admin";

        public const string LoginFail = "Invalid password or email!";
        public const string LoginSuccess = "You have logged in successfully!";
        public const string EmailHasTaken = "Email has already taken!";
        public const string UserNameHasTaken = "Username has already taken!";
        public const string CreateUserSuccess = "You have been registered successfully! A confirmation link has been sent to your email!";
    }
}
