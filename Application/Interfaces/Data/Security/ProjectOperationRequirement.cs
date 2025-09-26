using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Data.Security
{
    public class ProjectOperationRequirement : IAuthorizationRequirement
    {
    }

    //This class holds the operations
    public static class Operations
    {
        public static readonly ProjectOperationRequirement Create = new();
        public static readonly ProjectOperationRequirement Read = new();
        public static readonly ProjectOperationRequirement Update = new();
        public static readonly ProjectOperationRequirement Delete = new();
    }
}
