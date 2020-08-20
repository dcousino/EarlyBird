using EarlyBird.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EarlyBird.Services
{
    public class SchedulingService : CronService
    {
        private readonly IServiceProvider _services;

        public SchedulingService(IServiceProvider services, string cronExpression) : base(cronExpression)
        {
            _services = services;
        }

        public override async Task DoWork(CancellationToken cancellationToken = default)
        {
            using (var scope = _services.CreateScope())
            {
                var context =
                    scope.ServiceProvider
                        .GetRequiredService<ScheduleDbContext>();

                var reservations = context.Reservations.ToList().Where(r => (DateTime.Now - r.StartTime).TotalHours <= 24).GroupBy(x => new { x.ClubId, x.UserName });

                foreach (var groups in reservations)
                {
                    var svc = scope.ServiceProvider.GetRequiredService<Four25Service>();
                    await svc.Login(groups.Key.UserName);
                    foreach (var g in groups)
                    {
                    }
                }
            }
        }
    }
}