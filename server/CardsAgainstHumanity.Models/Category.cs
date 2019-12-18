using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CardsAgainstHumanity.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        [Required]
        public string CategoryName { get; set; }

        //een virtuele collectie voor relatie aanmaak
        public ICollection<Card> Cards { get; set; }

    }
}
