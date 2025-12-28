using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Values
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
        public const string RepositoryCreateSuccess = "Repository has created successfully!";

        //API Response Values
        public const string Unauthorized = "Unauthorized!";
        public const string InternalServerError = "Internal server error!";
        //Common Values
        public const string NameCannotBeNull = "Name value cannot be null!";
        public const string TitleCannotBeNull = "Title value cannot be null!";
        public const string DescriptionCannotBeNull = "Description value cannot be null!";
        public const string AllHasFoundSuccessfully = "All has found successfully!";
        public const string NothingHasFound = "Nothing has found!";
        public const string InvalidUser = "Invalid user!";
        public const string InvalidRoleEnum = "Invalid role!";
        public const string InvalidRepository = "Invalid repository!";
        public const string CreateSuccess = "Created successfully!";
        public const string CreateFailHasRecord = "Already recorded!";
        public const string SaveFail = "Could'nt save the data!";
        public const string ValidationError = "One or more validation errors occured!";
        public const string InvalidFail = "Invalid values!";
        public const string InvalidTitleLength75 = "Title cannot exceed 75 characters!";
        public const string InvalidDescriptionLength75 = "Description cannot exceed 75 characters!";
        public const string InvalidTitleSame = "There is already another record that has same title!";

        //File Values
        public const string RepositoryPhotoFolder = "Repository";
        public const string UserPhotoFolder = "User";
        public const string TaskPhotoFolder = "Task";
        public const string StageNotePhotoFolder = "StageNote";
    }
}
