using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace NadekoBot.Core.Services.Database.Models
{
    public class Leaderboard
    {
        public ulong UserId { get; set; }
        public LeaderboardType Type { get; set; }
        public LeaderboardTimeType TimeType { get; set; }
        public DateTime Date { get; set; }
        public long Score { get; set; }
    }

    public enum LeaderboardType
    {
        Gambling = 1,
        Wheel = 2,
        Blackjack = 3,
        GamblingSpent = 101,
        WheelSpent = 102
    }

    public enum LeaderboardTimeType
    {
        AllTime = 1,
        Yearly = 2,
        Monthly = 3,
        Daily = 4,
    }
}
