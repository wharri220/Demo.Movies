using Orchard.UI.Resources;

namespace Demo.Movies
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            var manifest = builder.Add();
            manifest.DefineStyle("Movies").SetUrl("Demo-Movies.css");
        }
    }
}