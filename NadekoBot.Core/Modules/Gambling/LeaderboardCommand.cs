using Discord;
using NadekoBot.Common.Attributes;
using NadekoBot.Core.Modules.Gambling.Common;
using NadekoBot.Core.Services;
using NadekoBot.Core.Services.Database.Models;
using NadekoBot.Extensions;
using NadekoBot.Modules.Gambling.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace NadekoBot.Modules.Gambling
{
    public partial class Gambling
    {
        public class LeaderboardCommand : GamblingSubmodule<GamblingService>
        {
            ILeaderboardService _lb;
            private string CurrencySign => Bc.BotConfig.CurrencySign;

            public LeaderboardCommand(ICurrencyService cs, ILeaderboardService lb, DbService db)
            {
                _lb = lb;
            }

            [NadekoCommand, Usage, Description, Aliases]
            public async Task Top(LeaderboardCommandType type)
            {
                List<VLeaderboard> list;
                string resName = string.Empty;
                switch (type)
                {
                    case LeaderboardCommandType.AllTime:
                        list = await _lb.GetTop(LeaderboardType.Gambling, LeaderboardTimeType.AllTime, DateTime.MinValue, 0, 10);
                        resName = "gambling_top_alltime";
                        break;
                    case LeaderboardCommandType.AllTimeSpent:
                        list = await _lb.GetTop(LeaderboardType.GamblingSpent, LeaderboardTimeType.AllTime, DateTime.MinValue, 0, 10);
                        resName = "gambling_top_alltimespent";
                        break;
                    case LeaderboardCommandType.Monthly:
                        list = await _lb.GetTop(LeaderboardType.Gambling, LeaderboardTimeType.AllTime, DateTime.MinValue, 0, 10);
                        resName = "gambling_top_monthly";
                        break;
                    default:
                        list = new List<VLeaderboard>();
                        break;
                }

                var embed = new EmbedBuilder()
                .WithOkColor()
                .WithTitle(GetText(resName));
                //.WithFooter(efb => efb.WithText(GetText("page", page)));                

                for (var i = 0; i < list.Count; i++)
                {
                    var x = list[i];
                    var usrStr = x.Username.ToString().TrimTo(20, true);

                    embed.AddField(efb => efb.WithName("#" + i + " " + usrStr)
                                             .WithValue(x.Score.ToString("N0") + " " + CurrencySign)
                                             .WithIsInline(true));                    
                }

                await ctx.Channel.EmbedAsync(embed).ConfigureAwait(false);
            }

            public enum LeaderboardCommandType
            {
                AllTime = 1,
                AllTimeSpent = 2,
                Monthly = 3
            }
        }
    }
}
