using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Results.RepositoryResults
{
    public class RepositoryGetResult
    {
        public List<RepositoryItem> Repositories { get; set; } = null!;
        public int TotalRecordCount { get; set; }
        public int TotalPageCount { get; set; }
    }

    public class RepositoryItem
    {
        public int RepositoryId { get; set; }
        public string RepositoryTitle { get; set; } = null!;
        public string RepositoryDescription { get; set; } = null!;
    }
}
