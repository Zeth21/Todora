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

        //User Values
        public const string LoginFail = "Invalid password or email!";
        public const string LoginSuccess = "You have logged in successfully!";
        public const string EmailHasTaken = "Email has already taken!";
        public const string UserNameHasTaken = "Username has already taken!";
        public const string CreateUserSuccess = "You have been registered successfully! A confirmation link has been sent to your email!";

        //Repository Values
        public const string RepositoryCreateFailTitleSame = "You already have another repository with same name!";
        public const string RepositoryCreateFailTitleLength = "Repository title cannot exceed 75 characters!";
        public const string RepositoryCreateFailDescriptionLength = "Repository title cannot exceed 200 characters!";
        public const string RepositoryCreateSuccess = "Repository has created successfully!";

        //API Response Values
        public const string Unauthorized = "Unauthorized!";

        //Common Values
        public const string NameCannotBeNull = "Name value cannot be null!";
        public const string TitleCannotBeNull = "Title value cannot be null!";
        public const string DescriptionCannotBeNull = "Description value cannot be null!";
        public const string AllHasFoundSuccessfully = "All has found successfully!";
    }
}
