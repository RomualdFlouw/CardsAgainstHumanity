using System.Collections.Generic;
using System.Threading.Tasks;
using CardsAgainstHumanity.Models;
using CardsAgainstHumanityNoSQL.Models;

namespace CardsAgainstHumanity.API.Controllers
{
    public interface IGameController
    {
        void AddPlayer(Player player);
        Task SetUpNewGame(List<User> users);
        bool HasEveryoneJoined();
        Task DrawStartingHands();
        List<CardsAgainstHumanityNoSQL.Models.Card> GetCardsForId(string connectionId);
        Task SetUpRound();
        List<Player> RemovePlayer(string id);
        Player GetPlayerInfo(string id);
        GameRoundInfo GetGameRoundInfo();
        List<CardsAgainstHumanityNoSQL.Models.Card> GetSelectedCards();
        bool ClientPickedCard(string conId, CardsAgainstHumanityNoSQL.Models.Card card);
    }
}