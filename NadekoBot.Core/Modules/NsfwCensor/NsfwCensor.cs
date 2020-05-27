using Discord.Commands;
using NadekoBot.Core.Services;
using NLog;

namespace NadekoBot.Modules.NsfwCensor
{
    [Group]
    public class NsfwCensor : NadekoTopLevelModule<ClarifaiService>
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();

        private readonly IBotCredentials _creds;
        public NsfwCensor()
        {
            _log.Error("Test test");
        }
    }
}
