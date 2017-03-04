using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Data.Templates;
using Sitecore.Resources.Media;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sitecore.ContentModerator.Helpers
{
    public static class SitecoreHelpers
    {
        static Database sitecoredb = Factory.GetDatabase("master");
        public static IEnumerable<Item> GetChildItems(ID itemId, params ID[] templateIds)
        {
            string fastQueryPath = GetChildItemsSitecoreFastQuery(itemId, templateIds);
            Database db = GetDataBase(null);
            IEnumerable<Item> childItems = db.SelectItems(fastQueryPath).OrderBy(c => c.Appearance.Sortorder).ToList();
            return childItems;
        }

        public static string GetChildItemsSitecoreFastQuery(ID itemId, params ID[] templateIds)
        {
            string query = null;
            if (!itemId.IsNull && templateIds.Length > 0)
            {
                string templateId = string.Join(" or @@TemplateId=", templateIds.Select(x => x.Guid.ToString("B").ToUpper().Replace("{", "'{").Replace("}", "}'")));
                query = @"fast://*[@@id = '" + itemId + "']//*[@@TemplateId = " + templateId + "]";
            }
            return query;
        }
        private static Database GetDataBase(string database)
        {
            Database db = null;


            if (!string.IsNullOrEmpty(database))
            {
                db = Factory.GetDatabase(database);
            }

            if (db == null)
            {
                db = Factory.GetDatabase("master");
            }
            return db;
        }

        public static Item GetItemById(ID id)
        {
            return GetItemById(id, null);
        }
        public static Item GetItemById(string id)
        {
            if (!string.IsNullOrEmpty(id))
                return GetItemById(new ID(id), null);
            else
                return null;
        }
        public static Item GetItemById(ID id, string database)
        {
            Database db = GetDataBase(database);
            if (null != db)
            {
                var item = db.GetItem(id);
                return item;
            }
            return null;
        }

        public static TemplateField[] GetAllFields(string templateid)
        {
            return GetAllFields(new ID(templateid));
        }

        public static TemplateField[] GetAllFields(ID templateid)
        {
            Template template = TemplateManager.GetTemplate(templateid, sitecoredb);
            TemplateField[] allFields = template.GetFields(true);
            return allFields;
        }



        public static ImageField GetImageFieldValue(Item item, string fieldName)
        {
            if (item != null && !string.IsNullOrEmpty(fieldName))
            {
                ImageField imgField = item.Fields[fieldName];
                return imgField;
            }
            return null;
        }
        public static string GetImageLink(Item item, string fieldName)
        {
            return GetImageLink(item, fieldName, false);
        }
        public static string GetImageLink(Item item, string fieldName, bool absoluteUrl)
        {
            if (item != null && !string.IsNullOrEmpty(fieldName))
            {
                ImageField imgField = item.Fields[fieldName];
                if (null == imgField || null == imgField.MediaItem)
                    return "";
                var mediaItem = imgField.MediaItem;

                return GetImageLink(mediaItem, absoluteUrl);
            }
            return null;
        }
        public static Stream GetImageStream(Item item, string fieldName)
        {
            if (item != null && !string.IsNullOrEmpty(fieldName))
            {
                ImageField imgField = item.Fields[fieldName];
                if (null == imgField || null == imgField.MediaItem)
                    return null;
                var media = (MediaItem)imgField.MediaItem;
                return media.GetMediaStream();
            }
            return null;
        }

        public static string GetImageLink(MediaItem item)
        {
            return GetImageLink(item, false);
        }
        public static string GetImageLink(MediaItem item, bool absoluteUrl)
        {
            if (item == null) return null;
            if (absoluteUrl)
            {
                MediaUrlOptions options = new MediaUrlOptions()
                {
                    AlwaysIncludeServerUrl = true,
                    LowercaseUrls = true,
                    AbsolutePath = true,
                    RequestExtension = string.Empty
                };
                return MediaManager.GetMediaUrl(item, options);
            }
            return MediaManager.GetMediaUrl(item, DefaultMediaOption);
        }

        public static MediaUrlOptions DefaultMediaOption = new MediaUrlOptions
        {
            AbsolutePath = true,
            DisableBrowserCache = false,
            IncludeExtension = true,
            LowercaseUrls = true,
            UseItemPath = true,
            RequestExtension = string.Empty
        };

    }
}