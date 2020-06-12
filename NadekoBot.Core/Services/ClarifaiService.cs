using System;
using Clarifai.API;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Predictions;
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

        private static readonly decimal SFW_THRESHOLD = 0.69f;

        public ClarifaiService(DiscordSocketClient _client, IBotCredentials creds)
        {
            _log = LogManager.GetCurrentClassLogger();
            _creds = creds;
            _clarifaiClient = new ClarifaiClient(_creds.ClarifaiKey);
        }

        private bool DetermineNsfw(string url)
        {
            var res = _clarifaiClient.PublicModels.NsfwModel
                  .Predict(new ClarifaiURLImage(url))
                  .ExecuteAsync()
                  .Result;

            // There are two concepts, stored in a single array: nsfw and sfw.
            // One with higher value is stored at index 0.
            Concept dc = res.Get().Data[0];
            if (dc.Name != null && dc.Name.Equals("nsfw"))
            {
                return true;
            }
            return dc.Name != null && dc.Name.Equals("nsfw") && dc.Value < SFW_THRESHOLD;
        }

        public void HandlePotentialNsfwMessage(SocketMessage msg)
        {

            string ExtractedUrl = URLUtils.ExtractUrl(msg.Content);

            if (URLUtils.IsImageUrl(msg.Content) && msg.Attachments.Count == 0)
            {
                return;
            }

            if (msg.Content.StartsWith("!nsfw") || msg.Content.StartsWith("!ns"))
            {
                ProcessNsfwMessage(msg);
            }

            if (URLUtils.IsImageUrl(ExtractedUrl))
            {
            
            }

        }

        public void ProcessNsfwImageLink(String link, SocketMessage m)
        {
            if (m.Content.StartsWith("||")) return;
        }

        public void ProcessNsfwMessage(SocketMessage m)
        {
            // TODO
        }

        public void PrintOut(SocketMessage msg)
        {
            _log.Error(msg.Content);
        }
    }
}
