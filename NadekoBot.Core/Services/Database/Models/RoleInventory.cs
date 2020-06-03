using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NadekoBot.Core.Services.Database.Models
{
    [Table("RoleInventory")]
    public class RoleInventory
    {
        public ulong UserId { get; set; }
        public ulong RoleId { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
    }

    //public class RoleForSale
    //{
    //    public ulong RoleId { get; set; }
    //}
}
