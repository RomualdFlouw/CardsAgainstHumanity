using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CardsAgainstHumanityNoSQL.Models.Repositories
{
    public interface IGameRepo
    {
        Task<Game> NewGame(Game game);
        Task<List<Card>> DrawTenWhites(ObjectId gameId);
        Task<Card> DrawOneWhite(ObjectId gameId);
        Task<Card> DrawOneBlack(ObjectId gameId);
    }
}