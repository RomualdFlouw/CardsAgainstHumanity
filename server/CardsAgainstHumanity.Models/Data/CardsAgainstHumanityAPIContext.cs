using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CardsAgainstHumanity.Models;
using CardsAgainstHumanity.Models.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CardsAgainstHumanity.Models
{
    //public class CardsAgainstHumanityAPIContext : DbContext
    public class CardsAgainstHumanityAPIContext : DbContext
    {
        private static ModelBuilder _modelBuilder { get; set; }

        public CardsAgainstHumanityAPIContext (DbContextOptions<CardsAgainstHumanityAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Deck> Deck { get; set; }
        public DbSet<Card> Card { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Seeden
            _modelBuilder = modelBuilder;
            base.OnModelCreating(_modelBuilder);

            ModelBuilderExtensions.modelBuilder = _modelBuilder;
            ModelBuilderExtensions.Seed();
        }
    }
}
