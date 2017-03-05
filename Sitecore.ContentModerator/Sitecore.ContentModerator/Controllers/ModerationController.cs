using Sitecore.ContentModerator.Helpers;
using Sitecore.ContentModerator.Helpers.Entities;
using Sitecore.ContentModerator.Helpers.Extensions;
using Sitecore.Services.Infrastructure.Web.Http;
using CMConstants = Microsoft.CognitiveServices.ContentModerator.Constants;
using Microsoft.CognitiveServices.ContentModerator; 
//using System.Web.Mvc;
using System.Web.Http;
using System;
using System.Threading.Tasks;

namespace Sitecore.ContentModerator.Controllers
{
    public class ModerationController : ServicesApiController
    {
        //
        // GET: /Moderation/

        public ModeratorEntity ModeratorSettings;
        public ModerationController()
        {
            ModeratorSettings = new ModeratorEntity() { API="", Lang="eng", Secret="" };
        }

        [HttpPost]        
        public string Start([FromBody]ItemData itemData)
        {
            string moderatedOutput = "";
            var item = SitecoreHelpers.GetItemById(itemData.ItemId);

            if (itemData.IsImage)
            {
                var imgStream = SitecoreHelpers.GetImageStream(item, itemData.FieldName);
            }
            else
            {
                var text = item.Fields[itemData.FieldName].Value.StripHtml();
                var mod = new ModeratorApiTextModel()
                {
                    autoCorrect = true,
                    contentType = (CMConstants.MediaType)Enum.Parse(typeof(CMConstants.MediaType), "Plain"),
                    language = "eng",
                    detectPii = true,
                    textToScreen = text
                };

               var tempmoderatedOutput = GetModeratedOutput(mod);
               Task.WaitAll(tempmoderatedOutput);
               moderatedOutput = tempmoderatedOutput.Result;
            }
            return moderatedOutput;
        }

        async Task<string> GetModeratedOutput(ModeratorApiTextModel toModerate)
        {
            return await Helpers.Utilities.ContentModeratorApiHelper.ModerateText(toModerate);
        }
        
	}
}