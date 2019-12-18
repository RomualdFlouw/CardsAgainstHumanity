using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CardsAgainstHumanity.Models.Data
{
    public static class ModelBuilderExtensions
    {
        public static IConfiguration _configuration { get; set; }
        public static string _contentRootPath { get; set; }
        public static CardsAgainstHumanityAPIContext _context { get; set; }
        public static ModelBuilder modelBuilder { get; set; }

        //Data/ModelBuilderExtensions.cs
        public static void Seed()
        {
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    CategoryID = 1,
                    CategoryName = "White Cards"
                },
                new Category
                {
                    CategoryID = 2,
                    CategoryName = "Black Cards"
                },
                new Category
                {
                    CategoryID = 3,
                    CategoryName = "Blank Cards"
                });

            modelBuilder.Entity<Deck>().HasData(
                new Deck
                {
                    DeckID = Guid.Parse("fe90fd43-c841-4dcd-a524-78ec08f5a1ad"),
                    DeckName = "Dutch Deck by Thijs",
                });
        }

        public static void SeedFromFile()
        {
            //plaats alle connection args in een configuratie file!
            var test1 = _contentRootPath;
            var test2 = _configuration.GetSection("AppSettings")["UploadPath"];
            var path = _contentRootPath + _configuration.GetSection("AppSettings")["UploadPath"];

            if (_context.Deck.Count() == 1)
            {
                using (StreamReader sr = new StreamReader(string.Format("{0}{1}", path, "Cards2.txt")))
                {
                    sr.ReadLine(); //skip header
                    while (sr.Peek() > 0)
                    {
                        string line = sr.ReadLine();
                        Card d = new Card
                        {
                            CardText = line.Split(";")[0],
                            CategoryID = Convert.ToInt32(line.Split(";")[1]),
                            PickAmount = Convert.ToInt32(line.Split(";")[2]),
                            DeckID = Guid.Parse(line.Split(";")[3]),
                        };
                        _context.Card.Add(d);
                        _context.SaveChanges();
                    }
                }

            }


        }
    }
}
