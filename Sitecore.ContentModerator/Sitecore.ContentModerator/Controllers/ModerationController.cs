using Sitecore.ContentModerator.Helpers;
using Sitecore.ContentModerator.Helpers.Entities;
using Sitecore.ContentModerator.Helpers.Extensions;
using Sitecore.Services.Infrastructure.Web.Http;
using System.Web.Mvc;

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
        public object Start(string itemId, string fieldName, string isImage)
        {
            var item = SitecoreHelpers.GetItemById(itemId);

            if (isImage.ToBoolean())
            {
                var imgStream = SitecoreHelpers.GetImageStream(item, fieldName);
            }
            else
            {
                var text = item.Fields[fieldName].Value.StripHtml();
            }

            return true;
        }
        
	}
}