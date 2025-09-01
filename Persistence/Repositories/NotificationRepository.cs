using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Notification>> GetAllNotifications(string userId, CancellationToken cancellation)
        {
            var notifications = await _context.Notifications.
                Where(x => x.UserId == userId)
                .ToListAsync(cancellation);
            return notifications;
        }
    }
}
