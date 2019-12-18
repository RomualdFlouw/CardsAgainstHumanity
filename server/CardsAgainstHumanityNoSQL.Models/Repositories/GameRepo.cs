using CardsAgainstHumanity.Models;
using CardsAgainstHumanity.Models.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardsAgainstHumanityNoSQL.Models.Repositories
{
    public class GameRepo : IGameRepo
    {
        private readonly CardsAgainstAPIContext _cardsAgainstAPIContext;
        private readonly IDeckRepo _deckRepo;
        // Using hard coded deck
        // Makes it possible to upgrade the app with other decks
        private Guid USEDDECK = Guid.Parse("fe90fd43-c841-4dcd-a524-78ec08f5a1ad");


        public GameRepo(CardsAgainstAPIContext cardsAgainstAPIContext, IDeckRepo deckRepo)
        {
            this._cardsAgainstAPIContext = cardsAgainstAPIContext;
            this._deckRepo = deckRepo;
        }

        public async Task<Game> NewGame(Game game)
        {
            try
            {
                await _cardsAgainstAPIContext.Games.InsertOneAsync(game);
                return game;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public async Task<List<Card>> DrawTenWhites(ObjectId gameId)
        {
            try
            {
                // Get the collection
                IMongoCollection<Game> collection = _cardsAgainstAPIContext.Database.GetCollection<Game>("Games");
                // Filter to get the game object with the id of the current game
                var filterResult = Builders<Game>.Filter.Eq(b => b.Id, gameId);
                Game currentGame = await collection.Find(filterResult).FirstOrDefaultAsync();

                // Sort by location (Order of shuffled deck)
                List<Card> WhiteDeck = currentGame.WhiteCards.OrderBy(wc => wc.Position).ToList();

                // Grab the first 10 cards
                List<Card> Hand = WhiteDeck.Take(10).ToList();
                List<Card> NewWhiteDeck = WhiteDeck.Skip(10).ToList();

                // Create updated game List
                currentGame.WhiteCards = NewWhiteDeck;
                await collection.ReplaceOneAsync(filterResult, currentGame);

                return Hand;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public async Task<Card> DrawOneWhite(ObjectId gameId)
        {
            try
            {
                // Get the collection
                IMongoCollection<Game> collection = _cardsAgainstAPIContext.Database.GetCollection<Game>("Games");
                // Filter to get the game object with the id of the current game
                var filterResult = Builders<Game>.Filter.Eq(b => b.Id, gameId);
                Game currentGame = await collection.Find(filterResult).FirstOrDefaultAsync();

                // Sort by location (Order of shuffled deck)
                List<Card> WhiteDeck = currentGame.WhiteCards.OrderBy(wc => wc.Position).ToList();

                // Grab the first 10 cards
                Card Hand = WhiteDeck.First();
                List<Card> NewWhiteDeck = WhiteDeck.Skip(1).ToList();

                // Create updated game List
                currentGame.WhiteCards = NewWhiteDeck;
                await collection.ReplaceOneAsync(filterResult, currentGame);

                return Hand;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public async Task<Card> DrawOneBlack(ObjectId gameId)
        {
            try
            {
                // Get the collection
                IMongoCollection<Game> collection = _cardsAgainstAPIContext.Database.GetCollection<Game>("Games");
                // Filter to get the game object with the id of the current game
                var filterResult = Builders<Game>.Filter.Eq(b => b.Id, gameId);
                Game currentGame = await collection.Find(filterResult).FirstOrDefaultAsync();

                // Sort by location (Order of shuffled deck)
                List<Card> BlackDeck = currentGame.BlackCards.OrderBy(wc => wc.Position).ToList();

                // Grab the first 10 cards
                Card BlackCard = BlackDeck.First();
                List<Card> NewBlackDeck = BlackDeck.Skip(1).ToList();

                // Create updated game List
                currentGame.WhiteCards = NewBlackDeck;
                await collection.ReplaceOneAsync(filterResult, currentGame);

                return BlackCard;
            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}
