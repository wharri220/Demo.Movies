using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement;
using Demo.Movies.Models;

namespace Demo.Movies.Drivers
{
    public class MovieSettingsPartDriver : ContentPartDriver<MovieSettingsPart>
    {
        protected override string Prefix
        {
            get{ return "MovieSettings";}
        }

        protected override DriverResult Editor(MovieSettingsPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_Movie_SiteSettings",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts/MovieSettings",
                    Model: part,
                    Prefix: Prefix)
                ).OnGroup("Movies");
        }

        protected override DriverResult Editor(MovieSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }

    }
}