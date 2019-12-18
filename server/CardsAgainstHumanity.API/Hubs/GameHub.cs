using CardsAgainstHumanity.API.Controllers;
using CardsAgainstHumanityNoSQL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardsAgainstHumanity.API.Hubs
{
    [Authorize]
    public class GameHub : Hub
    {
        private readonly IGameController _gameController;
        public GameHub(IGameController gameController)
        {
            this._gameController = gameController;
        }

        public async Task JoinGame(string userName)
        {
            try
            {
                // Add the player to gamecontroller
                Player p = new Player();
                p.ConnectionId = Context.ConnectionId;
                p.Name = userName;
                _gameController.AddPlayer(p);
                // Check if everyone has joined
                // if not -> return
                if (!_gameController.HasEveryoneJoined())
                    return;

                // If so -> draw 10 cards for everyone
                await _gameController.DrawStartingHands();
                //_gameController.GetAllPlayerInfo();
                await _gameController.SetUpRound();


                // Tell all clients their hands
            }
            catch
            {
                Console.WriteLine("Something went wrong joining a user to the game");
            }

        }

        public async Task GetStartingHand()
        {
            // Get connection Id of the client
            string connectionId = Context.ConnectionId;
            // Find his cards
            List<Card> hand = _gameController.GetCardsForId(connectionId);
            // Send them back
            await Clients.Caller.SendAsync("ReceiveStartingHand", hand);
        }

        public async Task GetRoundInfo()
        {
            var gameInfo = _gameController.GetGameRoundInfo();
            gameInfo.Player = _gameController.GetPlayerInfo(Context.ConnectionId);
            await Clients.Caller.SendAsync("ReceiveRoundInfo", gameInfo);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var users = _gameController.RemovePlayer(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);

        }

        public async Task ClientSelectedCard(Card card)
        {
            Console.WriteLine(card);

            if (!_gameController.ClientPickedCard(Context.ConnectionId, card))
                return;

            await Clients.All.SendAsync("PickWinner", _gameController.GetSelectedCards());

        }
    }
}
