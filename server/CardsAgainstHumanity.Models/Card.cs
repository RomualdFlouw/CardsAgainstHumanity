using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace CardsAgainstHumanity.Models
{

    public class Card
    {
        [Key]
        public Guid CardID { get; set; } = Guid.NewGuid();
        [Required]
        public string CardText { get; set; }
        [Required]
        public int CategoryID { get; set; }
        //[Required]
        public Guid DeckID { get; set; }
        public int PickAmount { get; set; }


        //een virtuele collectie voor relatie aanmaak
        public virtual Category Category { get; set; }
        //public virtual Deck Deck { get; set; }
    }
}
