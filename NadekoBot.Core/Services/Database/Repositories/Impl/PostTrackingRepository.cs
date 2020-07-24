using Google.Apis.YouTube.v3.Data;
using Microsoft.EntityFrameworkCore;
using NadekoBot.Core.Services.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NadekoBot.Core.Services.Database.Repositories.Impl
{
    public class PostTrackingRepository : IPostTrackingRepository
    {
        DbContext _context;
        DbSet<PostTracking> _set;
        
        public PostTrackingRepository(DbContext context)
        {
            _context = context;
            _set = context.Set<PostTracking>();
        }

        public async Task<bool> AddAsync(DateTime date, string channel, ulong userId, uint score)
        {
            date = date.Date; // only take the Date component
            var entity = await _set.AsQueryable().SingleOrDefaultAsync(p => p.Date == date && p.Channel == channel && p.UserId == userId);
            if (entity == null)
            {
                entity = new PostTracking() { Date = date, Channel = channel, UserId = userId, Score = 0 };
                _set.Add(entity);                
            }
            entity.Score += score;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SaveBulk(Dictionary<(ulong, string), uint> dict)
        {
            DateTime date = DateTime.UtcNow.Date;
            //var temp = _cachedScore.ToList();
            foreach (var item in dict)
            {
                var channel = item.Key.Item2;
                var userId = item.Key.Item1;
                var score = item.Value;
                var entity = await _set.AsQueryable().SingleOrDefaultAsync(p => p.Date == date && p.Channel == channel && p.UserId == userId);
                if (entity == null)
                {
                    entity = new PostTracking() { Date = date, Channel = channel, UserId = userId, Score = 0 };
                    _set.Add(entity);
                }
                entity.Score += score;
            }
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
