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
        /// Subscription Constant to default to
        /// </summary>
        private const string subscriptionKey = "285428d0a06d4a668c82b5b25cf5dc7e";

        /// <summary>
        /// Initiator Method
        /// </summary>
        static void Main()
        {
            // Setting the input values for invoking the Text Moderation method
            ModeratorApiTextModel moderationTextApiInput = new ModeratorApiTextModel() {
                textToScreen = "Testing WTF!! Shit Piss Off Ass",
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
            string responseText = moderationTextApiInput.textToScreen;
            string wrapTemplate = "<span data-cls='moderator-highlight'>{0}</span>";
            Dictionary<int, string> convertedItem = new Dictionary<int, string>();

            // Invoke the Moderation Client API
            ModeratorClient moderatorClient = new ModeratorClient(subscriptionKey);
            var matchAfterDeleteRes = await moderatorClient.ScreenTextAsync(moderationTextApiInput.textToScreen, moderationTextApiInput.contentType, moderationTextApiInput.language, moderationTextApiInput.autoCorrect, moderationTextApiInput.identifyUrls, moderationTextApiInput.detectPii, moderationTextApiInput.listId);
            
            // Analyze the reponse object for inappropriate content
            foreach (TextContract.MatchTerm item in ((TextContract.ScreenTextResult)matchAfterDeleteRes).Terms)
            {
                // Process the current inappropriate content, if not already processed
                if (!convertedItem.ContainsKey(item.Index))
                {
                    // Keep Track of the inappropriate word
                    convertedItem.Add(item.Index, item.Term);

                    // Wrap it with a span tag and class to highlight it
                    //textToScreen = textToScreen.Replace(item.Term, string.Format(wrapTemplate, item.Term));
                    responseText = Regex.Replace(responseText, item.Term, string.Format(wrapTemplate, item.Term), RegexOptions.Multiline | RegexOptions.IgnoreCase);
                }
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
            ModeratorClient moderatorClient = new ModeratorClient(subscriptionKey);
            var matchAfterDeleteRes = await moderatorClient.EvaluateImageAsync(moderationImageApiInput.content, ImageContract.DataRepresentationType.Url,moderationImageApiInput.cacheImage);

            // Analyze the reponse object for inappropriate content
            responseStatus = ((ImageContract.EvaluateImageResult)matchAfterDeleteRes).IsImageAdultClassified || ((ImageContract.EvaluateImageResult)matchAfterDeleteRes).IsImageRacyClassified;

            // Return the status as inappropriate image
            return responseStatus;
        }
    }    
}
