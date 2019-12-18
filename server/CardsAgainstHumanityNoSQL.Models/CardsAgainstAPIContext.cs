using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using static CardsAgainstHumanityNoSQL.Models.MongoSetting;

namespace CardsAgainstHumanityNoSQL.Models
{
    public class CardsAgainstAPIContext
    {
        public IMongoDatabase Database;

        public CardsAgainstAPIContext(IMongoSettings settings)
        {
            MongoClient client = new MongoClient(settings.ConnectionString);
            Database = client.GetDatabase(settings.DatabaseName);
        }

        public IMongoCollection<Game> Games =>
            Database.GetCollection<Game>("Games");
    }
}
