using Microsoft.EntityFrameworkCore;
using NadekoBot.Core.Services.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NadekoBot.Core.Services.Database.Repositories.Impl
{
    public class LeaderboardRepository : ILeaderboardRepository
    {
        DbContext _context;
        DbSet<Leaderboard> _set;
        DbSet<VLeaderboard> _vset;
        public LeaderboardRepository(DbContext context)
        {
            _context = context;
            _set = context.Set<Leaderboard>();
            _vset = context.Set<VLeaderboard>();
        }

        public async Task<bool> AddAsync(ulong userId, LeaderboardType type, LeaderboardTimeType timeType, long scoreInc)
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
            var entity = await _set.AsQueryable().SingleOrDefaultAsync(p => p.UserId == userId && p.Type == type && p.TimeType == timeType && p.Date == date);
            if (entity == null)
            {
                entity = new Leaderboard() { UserId = userId, Type = type, TimeType = timeType, Date = date };
                _set.Add(entity);
            }
            entity.Score += scoreInc;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<VLeaderboard>> GetTop(LeaderboardType type, LeaderboardTimeType timeType, DateTime date, int page, int records)
        {
            switch (timeType)
            {
                case LeaderboardTimeType.AllTime:
                    date = DateTime.MinValue;
                    break;
                case LeaderboardTimeType.Yearly:
                    date = new DateTime(date.Year, 1, 1);
                    break;
                case LeaderboardTimeType.Monthly:
                    date = new DateTime(date.Year, date.Month, 1);
                    break;
                case LeaderboardTimeType.Daily:
                    date = date.Date;
                    break;
                default:
                    date = date.Date;
                    break;
            }
            var list = await _vset.AsQueryable().Where(p => p.Type == type && p.TimeType == timeType && p.Date == date).Skip(page*records).Take(records).ToListAsync();
            return list;
        }
    }
}
