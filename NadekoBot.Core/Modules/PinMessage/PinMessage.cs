using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using NadekoBot.Modules;

namespace NadekoBot.Core.Modules.PinMessage
{
    [Group]
    public class PinMessage : NadekoTopLevelModule
    {
        private readonly DiscordSocketClient _client;
        private readonly int PIN_THRESHOLD = 5;
        private readonly string PIN_EMOTE = "\uD83D\uDCCC";

        public PinMessage(DiscordSocketClient client)
        {
            _client = client;
            _client.ReactionAdded += this.OnReactionAdded;
            _client.ReactionRemoved += this.OnReactionRemoved;
        }

        private async Task OnReactionAdded(Cacheable<IUserMessage, ulong> msg, ISocketMessageChannel chan, SocketReaction reaction)
        {
            if (reaction.Emote.Name == "")
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
            if (reaction.Emote.Name == "")
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
