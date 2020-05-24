using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NadekoBot.Core.Services.Database;
using System;
using System.IO;
using System.Linq;
using Npgsql;

namespace NadekoBot.Core.Services
{
    public class DbService
    {
        private readonly DbContextOptions<NadekoContext> options;
        private readonly DbContextOptions<NadekoContext> migrateOptions;

        private static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder => { builder.AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Information); });

        public DbService(IBotCredentials creds)
        {            
            var optionsBuilder = new DbContextOptionsBuilder<NadekoContext>()
                //.UseLoggerFactory(_loggerFactory)
                ;

            if(creds.Db.Type == "postgre")
            {
                optionsBuilder.UseNpgsql(creds.Db.ConnectionString);
                NadekoContext.DbType = "postgre";
                options = optionsBuilder.Options;
            }
            else // sqlite
            {
                var builder = new SqliteConnectionStringBuilder(creds.Db.ConnectionString);
                builder.DataSource = Path.Combine(AppContext.BaseDirectory, builder.DataSource);

                optionsBuilder.UseSqlite(builder.ToString());
                options = optionsBuilder.Options;

                optionsBuilder = new DbContextOptionsBuilder<NadekoContext>();
                optionsBuilder.UseSqlite(builder.ToString());
            }
            
            migrateOptions = optionsBuilder.Options;
        }

        public void Setup()
        {
            using (var context = new NadekoContext(options))
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    var mContext = new NadekoContext(migrateOptions);
                    mContext.Database.Migrate();
                    mContext.SaveChanges();
                    mContext.Dispose();
                }
                if(NadekoContext.IsSqlite)
                    context.Database.ExecuteSqlRaw("PRAGMA journal_mode=WAL");
                context.EnsureSeedData();
                context.SaveChanges();
            }
        }

        private NadekoContext GetDbContextInternal()
        {
            var context = new NadekoContext(options);
            context.Database.SetCommandTimeout(60);
            var conn = context.Database.GetDbConnection();
            conn.Open();
            if (NadekoContext.IsSqlite)
            {
                using (var com = conn.CreateCommand())
                {
                    com.CommandText = "PRAGMA journal_mode=WAL; PRAGMA synchronous=OFF";
                    com.ExecuteNonQuery();
                }
            }
            return context;
        }

        public IUnitOfWork GetDbContext() => new UnitOfWork(GetDbContextInternal());
    }
}