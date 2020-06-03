using Discord.WebSocket;
using NadekoBot.Core.Services;
using NadekoBot.Core.Services.Database.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NadekoBot.Modules.Gambling.Services
{
    public class LeaderboardService : ILeaderboardService
    {
        private readonly DbService _db;
        private readonly ICurrencyService _cs;
        private readonly IBotConfigProvider _bc;
        private readonly NadekoBot _bot;
        private readonly Logger _log;
        private readonly DiscordSocketClient _client;
        private readonly IDataCache _cache;

        public LeaderboardService(DbService db, NadekoBot bot, ICurrencyService cs, IBotConfigProvider bc,
            DiscordSocketClient client, IDataCache cache)
        {
            _db = db;
            _cs = cs;
            _bc = bc;
            _bot = bot;
            _log = LogManager.GetCurrentClassLogger();
            _client = client;
            _cache = cache;
        }

        public async Task<bool> AddAsync(ulong userId, LeaderboardType type, LeaderboardTimeType timeType, long scoreIncrease)
        {
            using (var ctx = _db.GetDbContext())
            {
                return await ctx.Leaderboards.AddAsync(userId, type, timeType, scoreIncrease);
            }
        }

        public async Task<List<VLeaderboard>> GetTop(LeaderboardType type, LeaderboardTimeType timeType, DateTime date, int page, int records)
        {
            using (var ctx = _db.GetDbContext())
            {
                return await ctx.Leaderboards.GetTop(type, timeType, date, page, records);
            }
        }
    }
}
