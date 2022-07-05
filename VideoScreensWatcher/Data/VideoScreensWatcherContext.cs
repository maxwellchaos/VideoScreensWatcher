using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VideoScreensWatcher.Models;

namespace VideoScreensWatcher.Data
{
    public class VideoScreensWatcherContext : DbContext
    {
        public VideoScreensWatcherContext (DbContextOptions<VideoScreensWatcherContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<VideoScreensWatcher.Models.Computer>? Computer { get; set; }
        public DbSet<VideoScreensWatcher.Models.OnlineLog>? OnlineLog { get; set; }
    }
}
