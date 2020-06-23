using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using NadekoBot.Core.Services;
using NLog;

namespace NadekoBot.Modules.NsfwCensor
{
    [Group]
    public class NsfwCensor : NadekoTopLevelModule<ClarifaiService>
    {
        private readonly DiscordSocketClient _client;

        private readonly ClarifaiService _clarifaiService;

        private static Logger _log;

        private readonly IBotCredentials _creds;

        public NsfwCensor(DiscordSocketClient client, ClarifaiService clarifaiService)
        {
            _client = client;
            _clarifaiService = clarifaiService;
            _client.MessageReceived += OnMessageReceived;
            _log = LogManager.GetCurrentClassLogger();
        }

        private Task OnMessageReceived(SocketMessage arg)
        {
            _clarifaiService.HandlePotentialNsfwMessage(arg);
            return Task.CompletedTask;
        }
    }
}
