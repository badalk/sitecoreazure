#region Namespaces
using Microsoft.CognitiveServices.ContentModerator;
using Microsoft.CognitiveServices.ContentModerator.Contract.Image;
#endregion

namespace ContentModeratorAPI
{
    /// <summary>
    /// Image Moderation Api Model
    /// </summary>
    public class ModeratorApiImageModel
    {
        /// <summary>
        /// Text to be moderated
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// Content Type of the input text to be moderated
        /// </summary>
        public DataRepresentationType contentType { get; set; }

        /// <summary>
        /// If the image is a Cached Image
        /// </summary>
        public bool cacheImage { get; set; }

        /// <summary>
        /// Type of the image being passed: Url or Binary stream
        /// </summary>
        public ImageType imageType { get; set; }
    }

    /// <summary>
    /// Type of the image being passed: Url or Binary stream
    /// </summary>
    public enum ImageType
    {
        // Image is passed as a url
        Url,

        // Image is passed as a binary
        Binary
    }
}
