﻿using Orchard.UI.Navigation;
using Orchard.Localization;
using Demo.Movies;

namespace Demo.Movies
{
    public class AdminMenu : INavigationProvider
    {
        public Localizer T { get; set; }

        public string MenuName
        {
            get { return "admin"; }
        }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(T("Movies"), "5", BuildMenu);
        }

        private void BuildMenu(NavigationItemBuilder menu)
        {
            menu.Add(T("List"), "1.1", item => item.Action("List", "Admin", new { area = "Contents", id = "Movie" }));
            menu.Add(T("New Movie"), "1.2", item => item.Action("Create", "Admin", new { area = "Contents", id = "Movie" }));
            menu.Add(T("Movie Lookup"), "1.3", item => item.Action("Index", "MovieLookup", new { area = "Demo.Movies" }).Permission(Permissions.LookupMovie));
            menu.Add(T("Actors"), "2.0", item => item.Action("Index", "ActorsAdmin", new { area = "Demo.Movies" }));
        }
    }
}