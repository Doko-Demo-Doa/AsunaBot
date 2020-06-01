using NadekoBot.Core.Services.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NadekoBot.Core.Services
{
    public interface ILeaderboardService : INService
    {
        Task<bool> AddAsync(ulong userId, LeaderboardType type, LeaderboardTimeType timeType, long scoreIncrease);
    }
}
