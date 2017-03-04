using Sitecore.Data.Fields;
using Sitecore.ContentModerator.Helpers.Entities;

namespace Sitecore.ContentModerator.Helpers.Extensions
{
    public static class SitecoreExtensions
    {
        public static FieldTypes GetFieldType(this Field field)
        {
            if (FieldTypeManager.GetField(field) is TextField)
                return FieldTypes.TextField;
            else if (FieldTypeManager.GetField(field) is HtmlField)
                return FieldTypes.HTMLField;
            else if (FieldTypeManager.GetField(field) is LookupField)
                return FieldTypes.LookUpField;
            else if (FieldTypeManager.GetField(field) is MultilistField)
                return FieldTypes.MultilistField;
            else if (FieldTypeManager.GetField(field) is ImageField)
                return FieldTypes.Image;
            else
                return FieldTypes.TextField;

        }
    }
}