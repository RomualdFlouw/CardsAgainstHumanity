using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CardsAgainstHumanity.Models
{
    public class Deck
    {
        [Key]
        public Guid DeckID { get; set; } = Guid.NewGuid();
        [Required]
        public string DeckName { get; set; }

        //een virtuele collectie voor relatie aanmaak
        public ICollection<Card> Cards { get; set; }
    }
}
