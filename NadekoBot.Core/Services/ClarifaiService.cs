using Clarifai.API;
using NLog;

namespace NadekoBot.Core.Services
{
    public class ClarifaiService : INService
    {
        private readonly Logger _log;

        private readonly IBotCredentials _creds;

        private readonly ClarifaiClient _clarifaiClient;

        public ClarifaiService(IBotCredentials creds)
        {
            _log = LogManager.GetCurrentClassLogger();
            _creds = creds;
            _clarifaiClient = new ClarifaiClient(_creds.ClarifaiKey);

            _log.Error(_creds.ClarifaiKey);
        }
    }
}
