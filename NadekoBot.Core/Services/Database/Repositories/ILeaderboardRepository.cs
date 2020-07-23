using NadekoBot.Core.Services.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NadekoBot.Core.Services.Database.Repositories
{
    public interface ILeaderboardRepository
    {
        Task<bool> AddAsync(ulong userId, Models.LeaderboardType type, Models.LeaderboardTimeType timeType, long scoreInc);
        Task<List<VLeaderboard>> GetTop(LeaderboardType type, LeaderboardTimeType timeType, DateTime date, int page, int records);
    }
}
