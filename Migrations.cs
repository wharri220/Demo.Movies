using System;
using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Data;
using Demo.Movies.Models;

namespace Demo.Movies {
    public class Migrations : DataMigrationImpl {
        
        private readonly IRepository<ActorRecord> _actorRepository;

        public Migrations(IRepository<ActorRecord> actorRepository){
            _actorRepository = actorRepository;
        }

        public int Create()
        {
            ContentDefinitionManager.AlterTypeDefinition("Movie", builder => builder.WithPart("CommonPart").WithPart("TitlePart").WithPart("AutoroutePart"));
            return 1;
        }

        public int UpdateFrom1()
        {
            ContentDefinitionManager.AlterTypeDefinition("Movie", builder => builder.WithPart("BodyPart").Creatable().Draftable().Listable());
            return 2;
        }

        public int UpdateFrom2()
        {
            ContentDefinitionManager.AlterTypeDefinition("Movie", builder => builder.WithPart("BodyPart", partBuilder => 
                partBuilder.WithSetting("BodyTypePartSettings.Flavor", "text"))
                .WithPart("AutoroutePart", partBuilder => partBuilder
                    .WithSetting("AutorouteSettings.AllowCustomPattern", "true")
                    .WithSetting("AutorouteSettings.AutomaticAdjustmenOnEdit", "false")
                    .WithSetting("AutorouteSettings.PatternDefinitions", "[{Name: 'Movie Title', Pattern: 'movies/{Content.Slug}', Description: 'movies/movie-title'}, {Name: 'Film Title', Pattern: 'films/{Content.Slug}', Description: 'films/film-title'}]")
                    .WithSetting("AutorouteSettings.DefaultPatternIndex", "0"))
                );
            return 3;
        }

        public int UpdateFrom3()
        {
            SchemaBuilder.CreateTable("MoviePartRecord", table => 
               table.ContentPartRecord()
                    .Column<string>("IMDB_Id")
                    .Column<int>("YearReleased")
                    .Column<string>("Rating", col => col.WithLength(4))
            );

            ContentDefinitionManager.AlterTypeDefinition("Movie", builder => builder.WithPart("MoviePart"));
            return 4;
        }

        public int UpdateFrom4()
        {
            ContentDefinitionManager.AlterPartDefinition("MoviePart", builder => builder
                .Attachable()
                .WithField("Genre", field => field
                     .OfType("TaxonomyField")
                     .WithSetting("DisplayName", "Genre")
                     .WithSetting("Taxonomy", "Genre")
                     .WithSetting("LeavesOnly", "False")
                     .WithSetting("SingleChoice", "False")
                     .WithSetting("Hint", "Select as many genres as you want")));
            return 5;
        }

        public int UpdateFrom5()
        {
            SchemaBuilder.CreateTable("ActorRecord", table => table
                .Column<int>("Id", col => col.PrimaryKey().Identity())
                .Column<string>("Name"));

            SchemaBuilder.CreateTable("MovieActorRecord", table => table
                .Column<int>("Id", col => col.PrimaryKey().Identity())
                .Column<int>("MoviePartRecord_Id")
                .Column<int>("ActorRecord_Id"));

            return 6;
        }

        //Seed actors
        public int UpdateFrom6()
        {
            _actorRepository.Create(new ActorRecord { Name = "Mark Hamill" });
            _actorRepository.Create(new ActorRecord { Name = "Harrison Ford" });
            _actorRepository.Create(new ActorRecord { Name = "Carrie Fisher" });
            _actorRepository.Create(new ActorRecord { Name = "Anthony Daniels" });
            _actorRepository.Create(new ActorRecord { Name = "Kenny Baker" });
            _actorRepository.Create(new ActorRecord { Name = "Peter Mayhew" });

            return 7;
        }
    }
}