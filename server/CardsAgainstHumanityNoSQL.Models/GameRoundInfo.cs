using System;
using System.Collections.Generic;
using System.Text;

namespace CardsAgainstHumanityNoSQL.Models
{
    public class GameRoundInfo
    {
        public int RoundNumber { get; set; }
        public Card BlackCard { get; set; }
        public string Czar { get; set; }
        public Player Player { get; set; }
        public List<Card> SelectedCards { get; set; }
    }
}
