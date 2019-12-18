using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardsAgainstHumanity.Models.Repositories
{
    public class DeckRepo : GenericRepo<Deck>, IDeckRepo
    {
        //Door GenericRepo te erven worden de CRUD methodes beschikbaar
        //Repo heeft enkel als required format de context nodig.
        //Deze context ophalen via erven van de base

        public DeckRepo(CardsAgainstHumanityAPIContext context) : base(context)
        {
        }

        public async Task<int> CountDecks()
        {
            return await _context.Deck.CountAsync();
        }

        public async Task<Deck> GetDecksWithCards(Guid id)
        {
            Deck deck = new Deck();
            try
            {
                return await _context.Deck
                    .Include(c => c.Cards)
                    .FirstAsync(d => d.DeckID == id);
            }
            catch
            {
                return null;
            }
        }
    }
}
