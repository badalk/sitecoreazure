using System;
using System;
using Sitecore.Diagnostics;
using Sitecore.Resources;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Text;
using Sitecore.Web;
using Sitecore.Web.UI.Framework.Scripts;
using Sitecore.Web.UI.Sheer;
using Sitecore.Web.UI.XamlSharp.Continuations;

namespace Sitecore.ContentModerator.Helpers
{
    public class ModerateCommand : Command, ISupportsContinuation
    {
        public override void Execute(CommandContext context)
        {
            try
            {
                Assert.ArgumentNotNull(context, "context");
                if (context.Items.Length == 1)
                {
                    //Check if the tab is already open and refresh it is needed
                    if (WebUtil.GetFormValue("scEditorTabs").Contains("ModerateContent:ModerateContentCommand"))
                    {

                    }
                    else //Open a new tab
                    {
                        UrlString urlString = new UrlString("/Admin/ContentModerator.aspx");
                        urlString.Append("itemId", context.Items[0].ID.ToString());
                        context.Items[0].Uri.AddToUrlString(urlString);
                        UIUtil.AddContentDatabaseParameter(urlString);
                        SheerResponse.Eval(new ShowEditorTab
                        {
                            Command = "ModerateContent:ModerateContentCommand",
                            Header = "Reviewing Content",
                            Icon = Images.GetThemedImageSource("Applications/32x32/document_ok.png"),
                            Url = urlString.ToString(),
                            Id = "ModerateContent",
                            Closeable = true
                        }.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error setting context menu for moderation", ex, "ContentModerator");
            }
        }
    }
}