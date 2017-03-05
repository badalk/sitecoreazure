#region Namespaces
using System;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.Configuration;
using System.Threading;
using Microsoft.CognitiveServices.ContentModerator;
using Microsoft.CognitiveServices.ContentModerator.Contract;
using ImageContract = Microsoft.CognitiveServices.ContentModerator.Contract.Image;
using TextContract = Microsoft.CognitiveServices.ContentModerator.Contract.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Sitecore.ContentModerator.Helpers.Entities;
#endregion

namespace Sitecore.ContentModerator.Helpers.Utilities
{
    public class ContentModeratorApiHelper
    {
        /// <summary>
        /// Character limit before which the API throws an error
        /// </summary>
        private const int APICHARACTERLIMIT = 1024;

        /// <summary>
        /// Subscription Constant to default to
        /// </summary>
        private const string SUBSCRIPTIONKEY = "285428d0a06d4a668c82b5b25cf5dc7e";

        /// <summary>
        /// Moderates the text to check for inappropriate content
        /// </summary>
        /// <param name="textToScreen">Text to be moderated</param>
        /// <param name="contentType">Content Type of the input text to be moderated</param>
        /// <param name="language">Language of the input text to be moderated</param>
        /// <param name="autoCorrect">Enable Autocorrection on the text to be moderated</param>
        /// <param name="identifyUrls">Enable Url identification on the text to be moderated</param>
        /// <param name="detectPii">Detect Personal Identifiable Information on the text moderation</param>
        /// <param name="listId">ListId for the Content Moderation API</param>
        /// <returns>Moderated string with the inappropriate words highlighted</returns>
        public static async Task<String> ModerateText(ModeratorApiTextModel moderationTextApiInput)
        {
            var inputText = moderationTextApiInput.textToScreen;
            var responseText = string.Empty;
            var wrapTemplate = "<span data-cls='moderator-highlight'>{0}</span>";
            var chrCount = 0;
            List<string> convertedItem = new List<string>();

            // Invoke the Moderation Client API
            ModeratorClient moderatorClient = new ModeratorClient(SUBSCRIPTIONKEY);

            // Split the job into batches of character limit
            var lines = inputText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                .GroupBy(w => (chrCount += w.Length + 1) / APICHARACTERLIMIT)
                                .Select(g => string.Join(" ", g));

            // Perform the text operation based on the character limit the API supports
            foreach (var line in lines)
            {
                var splitText = line;
                var moderatedTextResponse = await moderatorClient.ScreenTextAsync(splitText, moderationTextApiInput.contentType, moderationTextApiInput.language, moderationTextApiInput.autoCorrect, moderationTextApiInput.identifyUrls, moderationTextApiInput.detectPii, moderationTextApiInput.listId);

                // Analyze the reponse object for inappropriate content
                foreach (TextContract.MatchTerm item in ((TextContract.ScreenTextResult)moderatedTextResponse).Terms)
                {
                    // Process the current inappropriate content, if not already processed
                    if (!convertedItem.Contains(item.Term))
                    {
                        // Keep Track of the inappropriate word
                        convertedItem.Add(item.Term);

                        // Wrap it with a span tag and class to highlight it
                        splitText = Regex.Replace(splitText, item.Term, string.Format(wrapTemplate, item.Term), RegexOptions.Singleline | RegexOptions.IgnoreCase);
                    }
                }

                // Merge the moderated text back into the final response
                responseText += splitText.Replace("data-cls", "class");
            }

            // Return the final response with the inappropriate words highlighted
            Console.WriteLine(responseText);
            return responseText;
        }

        /// <summary>
        /// Moderates the text to check for inappropriate content
        /// </summary>
        /// <param name="textToScreen">Text to be moderated</param>
        /// <param name="contentType">Content Type of the input text to be moderated</param>
        /// <param name="language">Language of the input text to be moderated</param>
        /// <param name="autoCorrect">Enable Autocorrection on the text to be moderated</param>
        /// <param name="identifyUrls">Enable Url identification on the text to be moderated</param>
        /// <param name="detectPii">Detect Personal Identifiable Information on the text moderation</param>
        /// <param name="listId">ListId for the Content Moderation API</param>
        /// <returns>Moderated string with the inappropriate words highlighted</returns>
        public static async Task<ImageContract.EvaluateImageResult> ModerateImage(ModeratorApiImageModel moderationImageApiInput)
        {
            // Invoke the Moderation Client API
            ModeratorClient moderatorClient = new ModeratorClient(SUBSCRIPTIONKEY);

            // Return the status as inappropriate image            
            return await moderatorClient.EvaluateImageAsync(moderationImageApiInput.content, ImageContract.DataRepresentationType.Url, moderationImageApiInput.cacheImage);
        }
    }
}