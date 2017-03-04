using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Sitecore.ContentModerator.Helpers;
using Sitecore.ContentModerator.Helpers.Entities;
using Sitecore.ContentModerator.Helpers.Extensions;

namespace Sitecore.ContentModerator.Admin
{
    public partial class ContentModerator : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string itemId = Request.QueryString["itemId"];

            var currentItem = SitecoreHelpers.GetItemById(itemId);

            //itemIdlbl.Text = itemId;
            itemNamelbl.Text = currentItem.Name;
            //itemPathlbl.Text = currentItem.Paths.Path;

            var repeaterData = new List<ItemData>();

            var allRequiredFields = currentItem.Fields.Where(x => !x.Name.StartsWith("__"));
            foreach (var field in allRequiredFields)
            {
                switch (field.GetFieldType())
                {
                    case FieldTypes.TextField:
                        repeaterData.Add(new ItemData { FieldName=field.Name, ItemId=field.Item.ID.ToString(), IsImage=false,  Name = field.Name, DisplayName = field.DisplayName, Type = field.Type, Value = field.Value, NonHtmlValue = field.Value });
                        break;
                    case FieldTypes.Image:
                        repeaterData.Add(new ItemData { FieldName = field.Name, ItemId = field.Item.ID.ToString(), IsImage = true, Name = field.Name, DisplayName = field.DisplayName, Type = field.Type, Value = String.Format("<img src=\"{0}\"/>", SitecoreHelpers.GetImageLink(currentItem, field.Name)), NonHtmlValue = SitecoreHelpers.GetImageLink(currentItem, field.Name) });
                        break;
                    case FieldTypes.HTMLField:
                        repeaterData.Add(new ItemData { FieldName = field.Name, ItemId = field.Item.ID.ToString(), IsImage = false, Name = field.Name, DisplayName = field.DisplayName, Type = field.Type, Value = field.Value, NonHtmlValue = field.Value.StripHtml() });
                        break;
                    default:
                        repeaterData.Add(new ItemData { FieldName = field.Name, ItemId = field.Item.ID.ToString(), IsImage = false, Name = field.Name, DisplayName = field.DisplayName, Type = field.Type, Value = field.Value, NonHtmlValue = field.Value.StripHtml() });
                        break;
                };
            }
            fieldRepeater.DataSource = repeaterData;
            fieldRepeater.DataBind();
        }
    }
}