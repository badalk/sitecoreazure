#region Namespaces
using CMContants = Microsoft.CognitiveServices.ContentModerator.Constants;
#endregion

namespace Sitecore.ContentModerator.Helpers.Entities
{
    /// <summary>
    /// Text Moderation Api Model
    /// </summary>
    public class ModeratorApiTextModel
    {
        /// <summary>
        /// Text to be moderated
        /// </summary>
        public string textToScreen { get; set; }

        /// <summary>
        /// Content Type of the input text to be moderated
        /// </summary>
        public CMContants.MediaType contentType { get; set; }

        /// Language of the input text to be moderated
        /// </summary>
        public string language { get; set; }

        /// <summary>
        /// Enable Autocorrection on the text to be moderated
        /// </summary>
        public bool autoCorrect { get; set; }

        /// <summary>
        /// Enable Url identification on the text to be moderated
        /// </summary>
        public bool identifyUrls { get; set; }

        /// <summary>
        /// Detect Personal Identifiable Information on the text moderation
        /// </summary>
        public bool detectPii { get; set; }

        /// <summary>
        /// ListId for the Content Moderation API
        /// </summary>
        public string listId { get; set; }
    }
}
