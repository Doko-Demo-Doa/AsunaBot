using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using NadekoBot.Modules;

namespace NadekoBot.Core.Modules.StreamingDetector
{
    public class StreamingDetectorModule : NadekoTopLevelModule
    {
        private readonly DiscordSocketClient _client;

        public StreamingDetectorModule(DiscordSocketClient client)
        {
            _client = client;
            _client.UserVoiceStateUpdated += OnVoiceStateUpdated;
        }

        private Task OnVoiceStateUpdated(SocketUser usr, SocketVoiceState oldState, SocketVoiceState newState)
        {
            return Task.CompletedTask;
        }
    }
}
