using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NadekoBot.Core.Services.Database.Models
{
    [Table("PostTracking")]
    public class PostTracking
    {
        public DateTime Date { get; set; }
        public string Channel { get; set; }
        public ulong UserId { get; set; }
        public uint Score { get; set; }
    }
}
