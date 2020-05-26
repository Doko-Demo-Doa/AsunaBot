using Clarifai.API;

namespace NadekoBot.Core.Services
{
  public interface IClarifaiService : INService
    {
        ClarifaiClient ClarifaiClient { get; }
    }
}
