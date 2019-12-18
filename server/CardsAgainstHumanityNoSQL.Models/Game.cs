using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CardsAgainstHumanityNoSQL.Models
{
    public class Game
    {
        //must voor string mapping
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        [BsonElement("Players")]
        public List<Player> Players { get; set; }
        [BsonElement("WhitePositionIndex")]
        public int WhitePositionIndex { get; set; }
        [BsonElement("WhiteCards")]
        public List<Card> WhiteCards { get; set; }
        [BsonElement("BlackPositionIndex")]
        public int BlackPositionIndex { get; set; }
        [BsonElement("BlackCards")]
        public List<Card> BlackCards { get; set; }


    }
}
