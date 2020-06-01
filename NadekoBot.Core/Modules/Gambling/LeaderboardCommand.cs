using NadekoBot.Common.Attributes;
using NadekoBot.Core.Services;
using NadekoBot.Core.Services.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NadekoBot.Modules.Gambling
{
    public partial class Gambling
    {
        public class LeaderboardCommand
        {
            ILeaderboardService _lb;

            public LeaderboardCommand(ICurrencyService cs, ILeaderboardService lb, DbService db)
            {
                _lb = lb;
            }

            [NadekoCommand, Usage, Description, Aliases]
            public async Task Leaderboard(LeaderboardCommandType type)
            {
                
            }

            public enum LeaderboardCommandType
            {
                AllTime = 1,
                AllTimeSpent = 2,
            }
        }
    }
}
