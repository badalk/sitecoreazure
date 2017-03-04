
namespace Sitecore.ContentModerator.Helpers.Entities
{
    public class ItemData
    {
        public string FieldName { get; set; }
        public bool IsImage { get; set; }
        public string ItemId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public string DisplayName { get; set; }
        public string NonHtmlValue { get; set; }
    }

    public enum FieldTypes
    {
        TextField = 0,
        HTMLField = 1,
        LookUpField = 2,
        MultilistField = 3,
        Image = 4
    }
}