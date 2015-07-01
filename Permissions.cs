using System.Collections.Generic;
using Orchard.Security.Permissions;
using Orchard.Environment.Extensions.Models;

namespace Demo.Movies
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission LookupMovie = new Permission
        {
            Description = "Lookup Movies through the TMDb Api",
            Name = "LookupMovie"
        };

        public Feature Feature { get; set; }

        public IEnumerable<Permission> GetPermissions()
        {
            return new[]{
                LookupMovie
            };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[]{
                new PermissionStereotype{
                    Name = "Administrator",
                    Permissions = new []{ LookupMovie }
                }
            };
        }
    }
}