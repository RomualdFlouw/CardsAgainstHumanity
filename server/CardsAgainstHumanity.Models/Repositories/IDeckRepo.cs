using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CardsAgainstHumanity.Models.Repositories
{
    public interface IDeckRepo : IGenericRepo<Deck>
    {
        Task<int> CountDecks();

        Task<Deck> GetDecksWithCards(Guid id);
    }
}