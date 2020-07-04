using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
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

        [RequireContext(ContextType.Guild)]
        private async Task OnVoiceStateUpdated(SocketUser usr, SocketVoiceState oldState, SocketVoiceState newState)
        {
            ulong RoleId = _creds.StreamingRoleId;
            var user = (IGuildUser)usr;
            var Role = user.Guild.GetRole(RoleId);

            if (newState.IsStreaming)
            {
                await user.AddRoleAsync(Role);
            }
            else
            {
                await user.RemoveRoleAsync(Role);
            }
        }
    }
}
