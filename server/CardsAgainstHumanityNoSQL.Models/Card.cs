using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CardsAgainstHumanityNoSQL.Models
{
    public class Card
    {
        [BsonElement("CardID")]
        public Guid CardID { get; set; }
        [BsonElement("CardText")]
        public string CardText { get; set; }
        [BsonElement("Position")]
        public int Position { get; set; }
    }
}
