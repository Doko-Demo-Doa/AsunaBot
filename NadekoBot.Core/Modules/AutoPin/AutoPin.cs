using Discord;
using Discord.WebSocket;
using NadekoBot.Modules;
using System.Threading.Tasks;

namespace NadekoBot.Modules.AutoPin
{
    class AutoPin : NadekoTopLevelModule
    {
        private readonly DiscordSocketClient _client;

        public AutoPin(DiscordSocketClient client)
        {
            _client = client;
            _client.ReactionAdded += this.OnReactionAdded;
        }

        private Task OnReactionAdded(Cacheable<IUserMessage, ulong> msg, ISocketMessageChannel chan, SocketReaction reaction)
        {
            return null;
        }
    }
}
