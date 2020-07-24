using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NadekoBot.Core.Services.Database.Repositories
{
    public interface IPostTrackingRepository
    {
        Task<bool> AddAsync(DateTime date, string channel, ulong userId, uint score);
        Task<bool> SaveBulk(Dictionary<(ulong, string), uint> dict);
    }
}
