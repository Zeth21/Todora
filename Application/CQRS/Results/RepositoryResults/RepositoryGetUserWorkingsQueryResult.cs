using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Results.RepositoryResults
{
    public class RepositoryGetUserWorkingsQueryResult
    {
        public int RepositoryId {  get; set; }
        public string RepositoryTitle { get; set; } = null!;
        public string RepositoryDescription { get; set; } = null!;
    }
}
