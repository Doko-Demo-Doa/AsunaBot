using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using NadekoBot.Core.Services;
using NadekoBot.Modules;

namespace NadekoBot.Core.Modules.StreamingDetector
{
    public class StreamingDetectorModule : NadekoTopLevelModule
    {
        private readonly DiscordSocketClient _client;
        private readonly IBotCredentials _creds;

        public StreamingDetectorModule(IBotCredentials creds, DiscordSocketClient client)
        {
            _creds = creds;
            _client = client;
            _client.UserVoiceStateUpdated += OnVoiceStateUpdated;
        }

        private async Task OnVoiceStateUpdated(SocketUser usr, SocketVoiceState oldState, SocketVoiceState newState)
        {
            if (newState.IsStreaming)
            {
                ulong RoleId = _creds.StreamingRoleId;
                var Role = Context.Guild.GetRole(RoleId);

                if (!oldState.IsStreaming && newState.IsStreaming)
                {
                    await ((SocketGuildUser)Context.User).AddRoleAsync(Role);
                }
                if (oldState.IsStreaming && !newState.IsStreaming)
                {
                    await ((SocketGuildUser)Context.User).RemoveRoleAsync(Role);
                }
            }
        }
    }
}
