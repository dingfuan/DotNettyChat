using ChatServer.DataModel;
using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.DataSource
{
    public class ChatDbContext : DbContext
    {
        public ChatDbContext()
            : base("chatdb")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChatRecord>();
            Database.SetInitializer(new SqliteCreateDatabaseIfNotExists<ChatDbContext>(modelBuilder));
        }

        public DbSet<ChatRecord> ChatRecords { get; set; }
    }
}
