using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Clarifai.API;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Predictions;
using Discord;
using Discord.WebSocket;
using NadekoBot.Core.Common;
using NLog;

namespace NadekoBot.Core.Services
{
    public class ClarifaiService : INService
    {
        private readonly Logger _log;

        private readonly IBotCredentials _creds;

        private readonly ClarifaiClient _clarifaiClient;

        private static readonly decimal SFW_THRESHOLD = 0.69m;

        public static readonly List<string> ImageExtensions = new List<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };

        public ClarifaiService(DiscordSocketClient _client, IBotCredentials creds)
        {
            _log = LogManager.GetCurrentClassLogger();
            _creds = creds;
            _clarifaiClient = new ClarifaiClient(_creds.ClarifaiKey);
        }

        private bool IsNsfwImage(string url)
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
                return;
            }

            if (URLUtils.IsImageUrl(ExtractedUrl))
            {
                ProcessNsfwImageLink(ExtractedUrl, msg);
                return;
            }

            ProcessAttachment(msg);
        }

        public void ProcessNsfwImageLink(String link, SocketMessage m)
        {
            if (m.Content.StartsWith("||")) return;
            try
            {
                if (URLUtils.IsImageUrl(link) && IsNsfwImage(link))
                {
                    string builder = "||Nguyên văn từ " +
                        "<@" +
                        m.Author.Id +
                        "> : " +
                        m.Content + "||";

                    var CurrentChannel = m.Channel;
                    CurrentChannel.SendMessageAsync(builder).ConfigureAwait(false);
                    m.DeleteAsync().ConfigureAwait(false);
                }
            }
            catch (WebException)
            {
                // Code...
            }
        }

        public void ProcessAttachment(SocketMessage m)
        {
            bool IsGeneralChannel = (m.Channel.Id == _creds.GeneralChannelId);
            if (!IsGeneralChannel) return;

            if (m.Attachments.Count < 1) return;
            if (m.Author.IsBot) return;

            foreach (Attachment attachment in m.Attachments)
            {
                if (!URLUtils.IsImageUrl(attachment.Url)) return;
                if (attachment.Filename.StartsWith("SPOILER_")) return;

                if (IsNsfwImage(attachment.Url))
                {
                    ProcessNsfwMessage(m);
                }
            }
        }

        public void ProcessNsfwMessage(SocketMessage m)
        {
            if (m.Author.IsBot) return;
            if ((m.Attachments.Count > 0) && m.Attachments.First().Filename.StartsWith("SPOILER_")) return;

            var CurrentChannel = m.Channel;
            string NewContent = m.Content.Replace("!nsfw", "").Replace("!ns", "");

            string builder = "Nguyên văn từ " +
                "<@" +
                m.Author.Id +
                "> : " +
                NewContent;

            // Compose message
            Attachment MsgAttachment = m.Attachments.First();
            if (m.Attachments.Count > 0 && MsgAttachment != null)
            {
                var ImgStream = URLUtils.GetStreamFromUrl(MsgAttachment.Url);

                CurrentChannel.SendFileAsync(ImgStream, "SPOILER_" + MsgAttachment.Filename, text: builder, false, null, null, true).ConfigureAwait(false);
                m.DeleteAsync();
            }
        }
    }
}
