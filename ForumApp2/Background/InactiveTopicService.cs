using ForumApp2.Data;
using ForumApp2.Models;
using Microsoft.EntityFrameworkCore;



namespace ForumApp2.Background
{
    public class InactiveTopicService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public InactiveTopicService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var thresholdDate = DateTime.UtcNow.AddDays(-30);

                    var topicsToUpdate = context.Topics
                        .Include(t => t.Comments)
                        .Where(t => t.Comments.OrderByDescending(c => c.CreationDate).FirstOrDefault().CreationDate < thresholdDate && t.Status == TopicStatus.Active);

                    foreach (var topic in topicsToUpdate)
                    {
                        topic.Status = TopicStatus.Inactive;
                    }

                    await context.SaveChangesAsync();
                }

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }

}
