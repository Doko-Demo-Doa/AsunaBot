using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using NadekoBot.Modules;
using NLog;

namespace NadekoBot.Core.Modules.PinMessage
{
    [Group]
    public class PinMessage : NadekoTopLevelModule
    {
        private readonly DiscordSocketClient _client;
        private readonly int PIN_THRESHOLD = 3;
        private readonly string PIN_EMOTE = "\uD83D\uDCCC";

        private static Logger _log;

        public PinMessage(DiscordSocketClient client)
        {
            _client = client;
            _client.ReactionAdded += this.OnReactionAdded;
            _client.ReactionRemoved += this.OnReactionRemoved;
            _log = LogManager.GetCurrentClassLogger();
        }

        private async Task OnReactionAdded(Cacheable<IUserMessage, ulong> msg, ISocketMessageChannel chan, SocketReaction reaction)
        {
            if (reaction.Emote.Name.Equals(PIN_EMOTE))
            {
                IUserMessage m = (IUserMessage)await chan.GetMessageAsync(msg.Id);
                var ReactionList = m.Reactions;
                foreach (KeyValuePair<IEmote, ReactionMetadata> item in ReactionList)
                {
                    if (item.Key.Name.Equals(PIN_EMOTE) && item.Value.ReactionCount >= PIN_THRESHOLD)
                    {
                        await m.PinAsync();
                    }
                }
            }
        }

        private async Task OnReactionRemoved(Cacheable<IUserMessage, ulong> msg, ISocketMessageChannel chan, SocketReaction reaction)
        {
            if (reaction.Emote.Name.Equals(PIN_EMOTE))
            {
                IUserMessage m = (IUserMessage)await chan.GetMessageAsync(msg.Id);
                var ReactionList = m.Reactions;
                foreach (KeyValuePair<IEmote, ReactionMetadata> item in ReactionList)
                {
                    if (item.Key.Name.Equals(PIN_EMOTE) && item.Value.ReactionCount <= PIN_THRESHOLD - 2)
                    {
                        await m.UnpinAsync();
                    }
                }
            }
        }
    }
}
