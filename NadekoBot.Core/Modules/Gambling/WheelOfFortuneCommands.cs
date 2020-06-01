using Discord;
using NadekoBot.Common.Attributes;
using NadekoBot.Extensions;
using NadekoBot.Core.Services;
using System.Threading.Tasks;
using Wof = NadekoBot.Modules.Gambling.Common.WheelOfFortune.WheelOfFortuneGame;
using NadekoBot.Modules.Gambling.Services;
using NadekoBot.Core.Modules.Gambling.Common;
using NadekoBot.Core.Common;
using System.Collections.Immutable;
using NadekoBot.Core.Services.Database.Repositories;
using NadekoBot.Core.Services.Database.Models;

namespace NadekoBot.Modules.Gambling
{
    public partial class Gambling
    {
        public class WheelOfFortuneCommands : GamblingSubmodule<GamblingService>
        {
            private static readonly ImmutableArray<string> _emojis = new string[] {
            "⬆",
            "↖",
            "⬅",
            "↙",
            "⬇",
            "↘",
            "➡",
            "↗" }.ToImmutableArray();

            private readonly ICurrencyService _cs;
            private readonly DbService _db;
            private readonly ILeaderboardService _lb;

            public WheelOfFortuneCommands(ICurrencyService cs, ILeaderboardService lb, DbService db)
            {
                _cs = cs;
                _db = db;
                _lb = lb;
            }

            [NadekoCommand, Usage, Description, Aliases]
            public async Task WheelOfFortune(ShmartNumber amount)
            {
                if (!await CheckBetMandatory(amount).ConfigureAwait(false))
                    return;

                if (!await _cs.RemoveAsync(ctx.User.Id, "Wheel Of Fortune - bet", amount, gamble: true).ConfigureAwait(false))
                {
                    await ReplyErrorLocalizedAsync("not_enough", Bc.BotConfig.CurrencySign).ConfigureAwait(false);
                    return;
                }

                await _lb.AddAsync(ctx.User.Id, LeaderboardType.WheelSpent, LeaderboardTimeType.AllTime, amount.Value);
                var result = await _service.WheelOfFortuneSpinAsync(ctx.User.Id, amount).ConfigureAwait(false);

                await ctx.Channel.SendConfirmAsync(
Format.Bold($@"{ctx.User.ToString()} won: {result.Amount + Bc.BotConfig.CurrencySign}

   『{Wof.Multipliers[1]}』   『{Wof.Multipliers[0]}』   『{Wof.Multipliers[7]}』

『{Wof.Multipliers[2]}』      {_emojis[result.Index]}      『{Wof.Multipliers[6]}』

     『{Wof.Multipliers[3]}』   『{Wof.Multipliers[4]}』   『{Wof.Multipliers[5]}』")).ConfigureAwait(false);
            }
        }
    }
}