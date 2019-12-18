using CardsAgainstHumanity.Models;
using CardsAgainstHumanity.Models.Repositories;
using CardsAgainstHumanityNoSQL.Models;
using CardsAgainstHumanityNoSQL.Models.Repositories;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardsAgainstHumanity.API.Controllers
{
    public class GameController : IGameController
    {
        private readonly IGameRepo _gameRepo;
        private readonly IDeckRepo _deckRepo;
        static ObjectId currentGameId;
        static List<User> lobbyUsers = new List<User>();
        static List<Player> gamePlayers = new List<Player>();
        static int CzarIndex;
        static GameRoundInfo gameRound;
        static List<CardsAgainstHumanityNoSQL.Models.Card> SelectedCards = new List<CardsAgainstHumanityNoSQL.Models.Card>();

        public GameController(IGameRepo gameRepo, IDeckRepo deckRepo)
        {
            this._gameRepo = gameRepo;
            this._deckRepo = deckRepo;
        }

        public void AddPlayer(Player player)
        {
            gamePlayers.Add(player);
        }

        public bool HasEveryoneJoined()
        {
            // If there are just as many players as users, everyone has joined
            // Not checking on names specifically right now
            return gamePlayers.Count() == lobbyUsers.Count();
        }

        public List<CardsAgainstHumanityNoSQL.Models.Card> GetCardsForId(string connectionId)
        {
            try
            {
                var player = gamePlayers.Find(p => p.ConnectionId == connectionId);
                return player.Hand;
            }
            catch (Exception err)
            {
                throw (err);
            }
        }

        public async Task DrawStartingHands()
        {
            try
            {
                // Draw 10 cards for every player
                foreach (var player in gamePlayers.ToList())
                {
                    player.Hand = new List<CardsAgainstHumanityNoSQL.Models.Card>();
                    player.Hand = await _gameRepo.DrawTenWhites(currentGameId);
                }
            }
            catch (Exception err)
            {
                throw (err);
            }
        }

        public async Task SetUpNewGame(List<User> users)
        {
            try
            {
                // Set Czar index to 0
                CzarIndex = 0;
                SelectedCards = new List<CardsAgainstHumanityNoSQL.Models.Card>();
                // Save users in this controller
                lobbyUsers = users;
                // Create a game document
                Game game = new Game();
                // Give it an id
                // We generate the id in c# so that we can refer to it during the game
                currentGameId = ObjectId.GenerateNewId();
                game.Id = currentGameId;
                game.Players = new List<Player>();
                // Cycle through the users and add them as players to the game
                // Set their score to 0
                foreach (var user in users)
                {
                    game.Players.Add(new Player()
                    {
                        Name = user.Name,
                        Score = 0
                    });
                }
                // Fetch the deck we're going to play with
                // Using hard coded deck
                // Makes it possible to upgrade the app with other decks
                var id = Guid.Parse("fe90fd43-c841-4dcd-a524-78ec08f5a1ad");
                Deck deck = await _deckRepo.GetDecksWithCards(id);
                // Convert ICollection to List
                List<CardsAgainstHumanity.Models.Card> cards = deck.Cards.ToList();
                // Split the deck in a list of white cards and black cards
                List<CardsAgainstHumanityNoSQL.Models.Card> WhiteCards = new List<CardsAgainstHumanityNoSQL.Models.Card>();
                List<CardsAgainstHumanityNoSQL.Models.Card> BlackCards = new List<CardsAgainstHumanityNoSQL.Models.Card>();
                foreach (var card in cards)
                {
                    // Category 1 are black cards
                    if (card.CategoryID == 1)
                    {
                        WhiteCards.Add(new CardsAgainstHumanityNoSQL.Models.Card()
                        {
                            CardID = card.CardID,
                            CardText = card.CardText
                        });
                    }
                    // Category 2 are black cards
                    if (card.CategoryID == 2)
                    {
                        BlackCards.Add(new CardsAgainstHumanityNoSQL.Models.Card()
                        {
                            CardID = card.CardID,
                            CardText = card.CardText
                        });
                    }
                    // Category 3 are blank cards and aren't implemented yet
                    // They are not mandatory to play
                }

                // Generate a array of integers the length of the amount of cards
                // This is used to shuffle the cards
                // Ex: 200 cards will generate a list from 0 to 199.
                // We then randomly assign these values as the index of the card in the deck
                int[] valuesWC = Enumerable.Range(0, WhiteCards.Count()).ToArray();
                int[] valuesBC = Enumerable.Range(0, BlackCards.Count()).ToArray();
                // We keep track of the position index so we can sort the deck
                // In the NoSQL docs it was said to not rely on the order in the db as a way to sort them
                game.WhitePositionIndex = WhiteCards.Count();
                game.BlackPositionIndex = BlackCards.Count();

                foreach (var card in WhiteCards)
                {
                    // Get random number out of the list of indexes
                    Random rnd = new Random();
                    int indexOfPosition = rnd.Next(valuesWC.Length);
                    int positionValue = valuesWC[indexOfPosition];
                    // Assign this to the card
                    card.Position = positionValue;
                    // Delete the index out of the list so it can't be used anymore
                    valuesWC = valuesWC.Where(val => val != positionValue).ToArray();

                }

                foreach (var card in BlackCards)
                {
                    // Get random number out of the list of indexes
                    Random rnd = new Random();
                    int indexOfPosition = rnd.Next(valuesBC.Length);
                    int positionValue = valuesBC[indexOfPosition];
                    // Assign this to the card
                    card.Position = positionValue;
                    // Delete the index out of the list so it can't be used anymore
                    valuesBC = valuesBC.Where(val => val != positionValue).ToArray();

                }

                game.WhiteCards = WhiteCards;
                game.BlackCards = BlackCards;


                var obj = await _gameRepo.NewGame(game);
            }
            catch (Exception err)
            {
                throw (err);
            }
            
        }
        public List<Player> RemovePlayer(string id)
        {
            var userToDelete = gamePlayers.Find(u => u.ConnectionId == id);
            gamePlayers.Remove(userToDelete);
            return gamePlayers;
        }

        public Player GetPlayerInfo(string id)
        {
            return gamePlayers.Find(u => u.ConnectionId == id);
        }

        public async Task SetUpRound()
        {
            // Set up round info
            gameRound = new GameRoundInfo();
            // Draw a black Card
            gameRound.BlackCard = await _gameRepo.DrawOneBlack(currentGameId);
            // Choose Czar
            gamePlayers[CzarIndex].IsCzar = true;
            gameRound.Czar = gamePlayers[CzarIndex].Name;
            gameRound.RoundNumber += 1;

            // Reset all players to not having picked yet
            foreach (var player in gamePlayers)
            {
                player.HasPickedACard = false;
            }

            // Shift Czar 
            //if (CzarIndex == gamePlayers.Count() - 1)
            //{
            //    CzarIndex = 0;
            //}
            //else
            //{
            //    CzarIndex += 1;
            //}
            // 
        }

        public GameRoundInfo GetGameRoundInfo()
        {
            return gameRound;
        }

        public List<CardsAgainstHumanityNoSQL.Models.Card> GetSelectedCards()
        {
            return SelectedCards;
        }

        public bool ClientPickedCard(string conId, CardsAgainstHumanityNoSQL.Models.Card card)
        {
            // Select his card, and save that he has picked a card
            var p = gamePlayers.Find(u => u.ConnectionId == conId);
            p.HasPickedACard = true;
            p.SelectedCard = card;

            SelectedCards.Add(card);

            // Check if all users have picked
            return gamePlayers.All(x => x.HasPickedACard == true);
        }
    }
}
