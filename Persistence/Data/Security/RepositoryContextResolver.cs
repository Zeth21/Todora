using Application.Interfaces.Data.Security;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Security
{
    public class RepositoryContextResolver : IRepositoryContextResolver
    {
        private readonly ApplicationDbContext _context;
        public RepositoryContextResolver(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int?> GetRepositoryIdForResourceAsync(object resource)
        {
            return resource switch
            {
                WorkTask task => task.RepositoryId,
                RepositoryRole repositoryRole => repositoryRole.RepositoryId,
                Repository repository => repository.RepositoryId,
                StageNote stageNote => await GetStageNoteRepositoryId(stageNote),
                TaskStage taskStage => await GetTaskStageRepositoryId(taskStage),
                TaskOwning taskOwning => await GetTaskOwningRepositoryId(taskOwning),
                _ => null
            };
        }

        private async Task<int?> GetStageNoteRepositoryId(StageNote stageNote) 
        {
            if (stageNote.TaskStage?.Task != null)
            {
                return stageNote.TaskStage.Task.RepositoryId;
            }

            await _context.Entry(stageNote).Reference(sn => sn.TaskStage).LoadAsync();
            if (stageNote.TaskStage == null)
            {
                return null;
            }

            await _context.Entry(stageNote.TaskStage).Reference(ts => ts.Task).LoadAsync();
            return stageNote.TaskStage.Task?.RepositoryId;
        }

        private async Task<int?> GetTaskStageRepositoryId(TaskStage stage) 
        {
            if (stage.Task != null)
                return stage.Task.RepositoryId;

            await _context.Entry(stage).Reference(sn => sn.Task).LoadAsync();
            if(stage.Task == null)
                return null;

            return stage.Task.RepositoryId;
        }

        private async Task<int?> GetTaskOwningRepositoryId(TaskOwning taskOwning) 
        {
            if(taskOwning.Task != null)
                return taskOwning.Task.RepositoryId;

            await _context.Entry(taskOwning).Reference(sn => sn.Task).LoadAsync();
            if(taskOwning.Task == null)
                return null;

            return taskOwning.Task.RepositoryId;
        }
    }
}
