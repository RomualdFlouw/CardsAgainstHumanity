using CardsAgainstHumanity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CardsAgainstHumanity.API.Models
{
    public class DeckDTO
    {
        public Guid DeckID { get; set; } = Guid.NewGuid();
        public string DeckName { get; set; }

        //een virtuele collectie voor relatie aanmaak
        public ICollection<Card> Cards { get; set; }
    }
}
