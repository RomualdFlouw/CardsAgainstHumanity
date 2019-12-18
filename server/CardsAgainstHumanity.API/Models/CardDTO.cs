using CardsAgainstHumanity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CardsAgainstHumanity.API.Models
{
    public class CardDTO
    {
        public Guid CardID { get; set; } = Guid.NewGuid();
        public string CardText { get; set; }
        public int CategoryID { get; set; }
        public Guid DeckID { get; set; }
        public int PickAmount { get; set; }
    }
}
