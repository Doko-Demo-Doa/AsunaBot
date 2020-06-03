using Microsoft.EntityFrameworkCore;
using NadekoBot.Core.Services.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NadekoBot.Core.Services.Database.Repositories.Impl
{
    public class RoleInventoryRepository : IRoleInventoryRepository
    {
        DbContext _context;
        DbSet<RoleInventory> _set;

        public RoleInventoryRepository(DbContext context)
        {
            _context = context;
            _set = context.Set<RoleInventory>();
        }
        public async Task<bool> AddAsync(ulong userId, ulong roleId)
        {
            var entity = await _set.AsQueryable().SingleOrDefaultAsync(p => p.UserId == userId && p.RoleId == roleId);
            if (entity == null)
            {
                entity = new RoleInventory() { RoleId = roleId, UserId = userId };
                _set.Add(entity);
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public Task<ulong[]> GetAsync(ulong userId)
        {
            return _set.AsQueryable().Where(p => p.UserId == userId).Select(p => p.RoleId).ToArrayAsync();
        }
    }
}
