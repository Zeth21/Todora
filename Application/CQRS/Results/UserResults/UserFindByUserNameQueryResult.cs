using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Results.UserResults
{
    public class UserFindByUserNameQueryResult
    {
        public string Id { get; set; } = null!;
        public string UserPhotoPath { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Email { get; set; } = null!;


    }
}
