#region Namespaces
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
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
#endregion

namespace ContentModeratorAPI
{
    /// <summary>
    /// Static class to test the Text and Image Moderation
    /// </summary>
    static class Program
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
        /// Placeholder to store the data
        /// </summary>
        private const string DATACLASSPLACEHOLDER = "data-cls";

        /// <summary>
        /// Initiator Method
        /// </summary>
        static void Main()
        {
            // Setting the input values for invoking the Text Moderation method
            ModeratorApiTextModel moderationTextApiInput = new ModeratorApiTextModel()
            {
                textToScreen = "Testing WTF!! Shit Piss Off Ass Lorem Ipsum dolor shit amet consectur.",
                contentType = (Constants.MediaType)Enum.Parse(typeof(Constants.MediaType), "Plain"),
                language = "eng",
                autoCorrect = true,
                identifyUrls = true,
                detectPii = true,
                listId = "class"
            };

            // Invoke the ModerateText method passing the required inputs for a text
            ModerateText(moderationTextApiInput);

            // Setting the input values for invoking the Image Moderation method
            ModeratorApiImageModel moderationImageApiInput = new ModeratorApiImageModel()
            {
                imageType = ImageType.Url,
                cacheImage = false,
                content = @"http://www.oddee.com/_media/imgs/articles2/a97756_g268_3-sexist.jpg",
                contentType = (ImageContract.DataRepresentationType)Enum.Parse(typeof(ImageContract.DataRepresentationType), "Url")
            };

            // Invoke the ModerateImage method passing the required inputs for an image
            ModerateImage(moderationImageApiInput);

            // Await operation
            Console.ReadLine();
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
        static async Task<String> ModerateText(ModeratorApiTextModel moderationTextApiInput)
        {
            var inputText = moderationTextApiInput.textToScreen;
            var responseText = string.Empty;
            var wrapTemplate = "<span data-class='moderator-highlight'>{0}</span>";
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
                        //textToScreen = textToScreen.Replace(item.Term, string.Format(wrapTemplate, item.Term));
                        splitText = Regex.Replace(splitText, item.Term, string.Format(wrapTemplate, item.Term), RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    }
                }

                // Merge the moderated text back into the final response
                responseText += splitText;
            }

            // Return the final response with the inappropriate words highlighted
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
        static async Task<bool> ModerateImage(ModeratorApiImageModel moderationImageApiInput)
        {
            bool responseStatus = false;

            // Invoke the Moderation Client API
            ModeratorClient moderatorClient = new ModeratorClient(SUBSCRIPTIONKEY);
            var matchAfterDeleteRes = await moderatorClient.EvaluateImageAsync(moderationImageApiInput.content, ImageContract.DataRepresentationType.Url, moderationImageApiInput.cacheImage);

            // Analyze the reponse object for inappropriate content
            responseStatus = ((ImageContract.EvaluateImageResult)matchAfterDeleteRes).IsImageAdultClassified || ((ImageContract.EvaluateImageResult)matchAfterDeleteRes).IsImageRacyClassified;

            // Return the status as inappropriate image
            return responseStatus;
        }
    }
}
