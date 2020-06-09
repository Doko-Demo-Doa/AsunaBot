using System;
using Clarifai.API;
using Clarifai.DTOs.Inputs;
using Discord.WebSocket;
using NadekoBot.Core.Common;
using NLog;
using SixLabors.ImageSharp;

namespace NadekoBot.Core.Services
{
    public class ClarifaiService : INService
    {
        private readonly Logger _log;

        private readonly IBotCredentials _creds;

        private readonly ClarifaiClient _clarifaiClient;

        private static float SFW_THRESHOLD = 0.63f;

        public ClarifaiService(DiscordSocketClient _client, IBotCredentials creds)
        {
            _log = LogManager.GetCurrentClassLogger();
            _creds = creds;
            _clarifaiClient = new ClarifaiClient(_creds.ClarifaiKey);
        }

        public void HandlePotentialNsfwMessage(SocketMessage msg)
        {

            string ExtractedUrl = URLUtils.ExtractUrl(msg.Content);

            if (URLUtils.IsImageUrl(ExtractedUrl))
            {
                var res = _clarifaiClient.PublicModels.NsfwModel
                    .Predict(new ClarifaiURLImage(ExtractedUrl))
                    .ExecuteAsync()
                    .Result;

                foreach (var concept in res.Get().Data)
                {
                    Console.WriteLine($"{concept.Name}: {concept.Value}");
                }
            }

        }

        public void PrintOut(SocketMessage msg)
        {
            _log.Error(msg.Content);
        }
    }
}
