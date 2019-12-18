using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CardsAgainstHumanityNoSQL.Models
{
    public class Player
    {
        [BsonElement("Name")]
        public string Name { get; set; }
        [BsonElement("Score")]
        public int Score { get; set; }
        [BsonIgnore]
        public List<Card> Hand { get; set; }
        [BsonIgnore]
        public string ConnectionId { get; set; }
        [BsonIgnore]
        public bool IsCzar { get; set; }
        [BsonIgnore]
        public bool HasPickedACard { get; set; }
        [BsonIgnore]
        public Card SelectedCard { get; set; }

    }
}
