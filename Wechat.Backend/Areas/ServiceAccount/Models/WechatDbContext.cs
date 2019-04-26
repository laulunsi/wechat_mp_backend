using Microsoft.EntityFrameworkCore;

namespace Wechat.Backend.Areas.ServiceAccount.Models
{
    public class WechatDbContext : DbContext
    {
        public WechatDbContext(DbContextOptions<WechatDbContext> options)
            : base(options)
        {
        }

        public DbSet<Log> Logs { get; set; }
    }
}