using AngleSharp.Common;
using Discord.WebSocket;
using Microsoft.Extensions.Caching.Memory;
using NadekoBot.Core.Services.Database.Repositories;
using NadekoBot.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NadekoBot.Core.Services
{
    public class TrackingService : INService
    {
        DiscordSocketClient _client;
        DbService _db;
        private readonly IPostTrackingRepository _pr;
        private ConcurrentDictionary<(ulong, string), uint> _cachedScore = new ConcurrentDictionary<(ulong, string), uint>();
        private IMemoryCache _cache;
        public TrackingService(DiscordSocketClient client, DbService db, IMemoryCache cache)
        {
            _client = client;
            _db = db;
            _pr = _db.GetDbContext().PostTracking;
            _cache = cache;
        }

        public async Task<bool> TrackPost(string channel, ulong userId, uint score)
        {
            DateTime date = DateTime.UtcNow.Date;
            var lastCache = _cache.GetOrCreate("last_track_post", entry =>
            {
                SaveCacheToDb();
                entry.AbsoluteExpiration = DateTime.Now.AddSeconds(60);
                return DateTime.Now;
            });
            if (_cachedScore.ContainsKey((userId, channel)))
                _cachedScore[(userId, channel)] += score;
            else
                _cachedScore.TryAdd((userId, channel), score);
            //await _pr.AddAsync(date, channel, userId, score);
            return true;
        }

        void SaveCacheToDb()
        {
            DateTime date = DateTime.UtcNow.Date;
            var temp = _cachedScore.ToDictionary(p => p.Key, p => p.Value);
            _pr.SaveBulk(temp);            
            foreach (var item in temp)
            {
                _cachedScore[item.Key] = 0;
            }
        }
    }
}
