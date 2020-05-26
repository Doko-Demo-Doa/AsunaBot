using NadekoBot.Core.Services;
using NLog;

namespace NadekoBot.Modules.NsfwCensor
{
    public class NsfwCensor : NadekoTopLevelModule<IClarifaiService>
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();

        private readonly IBotCredentials _creds;
        public NsfwCensor()
        {
            _log.Error("Test test");
        }
    }
}
