using Application.Interfaces.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class StageRepository : BaseRepository<Stage>, IStageRepository
    {
        public StageRepository(ApplicationDbContext context) : base (context)
        {
            
        }
    }
}
