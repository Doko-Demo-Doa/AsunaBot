using Clarifai.API;
using System;
using System.Collections.Generic;
using System.Text;

namespace NadekoBot.Core.Services
{
    public interface IClarifaiService : INService
    {
        ClarifaiClient ClarifaiClient { get; }
    }
}
