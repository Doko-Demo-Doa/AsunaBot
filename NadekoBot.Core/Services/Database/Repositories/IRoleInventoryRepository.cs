using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NadekoBot.Core.Services.Database.Repositories
{
    public interface IRoleInventoryRepository
    {
        Task<bool> AddAsync(ulong userId, ulong roleId);
        Task<ulong[]> GetAsync(ulong userId);
    }
}
