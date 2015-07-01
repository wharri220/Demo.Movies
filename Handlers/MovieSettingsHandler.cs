using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Localization;
using Orchard.Data;
using Demo.Movies.Models;

namespace Demo.Movies.Handlers
{
    public class MovieSettingsHandler : ContentHandler
    {
        public Localizer T { get; set; }

        public MovieSettingsHandler(IRepository<MovieSettingsPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(new ActivatingFilter<MovieSettingsPart>("Site"));
        }

        protected override void GetItemMetadata(GetContentItemMetadataContext context)
        {
            if (context.ContentItem.ContentType != "Site")
            {
                return;
            }

            base.GetItemMetadata(context);
            context.Metadata.EditorGroupInfo.Add(new GroupInfo(T("Movies")));
        }
    }
}