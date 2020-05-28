using Microsoft.EntityFrameworkCore;
using NadekoBot.Core.Services.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NadekoBot.Core.Services.Database.Repositories.Impl
{
    public class LeaderboardRepository : ILeaderboardRepository
    {
        DbContext _context;
        DbSet<Leaderboard> _set;
        public LeaderboardRepository(DbContext context)
        {
            _context = context;
            _set = context.Set<Leaderboard>();
        }

        public async Task<bool> AddAsync(ulong userId, LeaderboardType type, LeaderboardTimeType timeType, int scoreInc)
        {
            DateTime date = DateTime.Now.Date;
            switch (timeType)
            {
                case LeaderboardTimeType.AllTime:
                    date = DateTime.MinValue;
                    break;
                case LeaderboardTimeType.Yearly:
                    date = new DateTime(DateTime.Now.Year, 1, 1);
                    break;
                case LeaderboardTimeType.Monthly:
                    date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    break;
                case LeaderboardTimeType.Daily:
                    date = DateTime.Now.Date;
                    break;
                default:
                    break;
            }
            var entity = await _set.SingleOrDefaultAsync(p => p.UserId == userId && p.Type == type && p.TimeType == timeType && p.Date == date);
            if (entity == null)
            {
                entity = new Leaderboard() { UserId = userId, Type = type, TimeType = timeType, Date = date };
                _set.Add(entity);
            }
            entity.Score += scoreInc;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
