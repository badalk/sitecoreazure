using Newtonsoft.Json;
using Sitecore.ContentModerator.Helpers;
using Sitecore.ContentModerator.Helpers.Entities;
using Sitecore.ContentModerator.Helpers.Extensions;
using Sitecore.Services.Infrastructure.Web.Http;
//using System.Web.Mvc;
using System.Web.Http;

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
        public bool Start([FromBody]ItemData itemData)
        {
            //var itemData = JsonConvert.DeserializeObject<ItemData>(itemInfo);

            var item = SitecoreHelpers.GetItemById(itemData.ItemId);

            if (itemData.IsImage)
            {
                var imgStream = SitecoreHelpers.GetImageStream(item, itemData.FieldName);
            }
            else
            {
                var text = item.Fields[itemData.FieldName].Value.StripHtml();
            }

            return true;
        }
        
	}
}