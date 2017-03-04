
namespace Sitecore.ContentModerator.Helpers.Entities
{
    public class ItemData
    {
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