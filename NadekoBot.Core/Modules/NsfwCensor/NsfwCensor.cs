using NadekoBot.Core.Services;

namespace NadekoBot.Modules.NsfwCensor
{
    public class NsfwCensor : NadekoTopLevelModule<IClarifaiService>
    {
        private readonly IBotCredentials _creds;
        public NsfwCensor()
        {
        }
    }
}
