using CardsAgainstHumanity.API.Controllers;
using CardsAgainstHumanity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardsAgainstHumanity.API.Hubs
{
    [Authorize]
    public class LobbyHub: Hub
    {
        private readonly IUserController UsersController;
        private readonly IGameController GameController;

        public LobbyHub(IUserController _usersController, IGameController _gameController)
        {
            UsersController = _usersController;
            GameController = _gameController;
        }

        // This function is for when a new player logs into the game
        // It will check if the lobby is full (6 players), if it's full they can't enter
        // It will check if the username is already taken, if so they're denied also
        // It will then find the connection id of the client and save that to a local (server) list together with some other info (name, score, etc)

        public async Task LoginUser(User user)
        {
            try
            {
                // Check if username is taken
                bool isUsernameAvailable = UsersController.IsNameAvailable(user.Name);

                if (UsersController.AmountOfUsers() >= 6 || UsersController.isGameStarting())
                {
                    await Clients.Caller.SendAsync("LobbyFull");
                    return;
                }

                if (!isUsernameAvailable)
                {
                    await Clients.Caller.SendAsync("UsernameChecked", isUsernameAvailable);
                    return;
                }

                // Add the hub connectionId to our user
                user.ConnectionId = Context.ConnectionId;

                // Add this new User to the list in UsersController
                // Add Name, Token, ClientId, ReadyState = false, Score = 0
                UsersController.AddUser(user);

                // Fetch the updated list of users
                var users = UsersController.GetAllUsers();

                // Give all players a updated list of the lobby
                await Clients.Others.SendAsync("UpdateLobby", users);
                await Clients.Caller.SendAsync("UsernameChecked", isUsernameAvailable);
                // This way you send a message to specific user based on id
                //await Clients.Client(user.ConnectionId).SendAsync("UsernameChecked", isUsernameAvailable);
            }
            catch
            {
                Console.WriteLine("Something went wrong logging in user");
            }
            
        }

        // This function is for when a new person joins the game.
        // They call this function to fill the frontend with all connected players
        // This should happen when they load the lobby page
        public async Task GetAllPlayers()
        {
            // Fetch the updated list of users
            var users = UsersController.GetAllUsers();
            await Clients.Caller.SendAsync("UpdateLobby", users);
        }

        public async Task ClientReadyStateChange()
        {
            try
            {
                var userId = Context.ConnectionId;
                // Set the user in the list to the given readystate
                var users = UsersController.ChangeReadyState(userId);

                // Tell all other users that this user is ready
                await Clients.All.SendAsync("UpdateLobby", users);
                // Tell the caller that their new readystate is updated so they can edit the frontend
                await Clients.Caller.SendAsync("UpdateReadyState");

                // Check if ALL users are ready (and minimum 3 players), if so send the countdown and start the game
                if (!UsersController.ShouldGameStart())
                {
                    return;
                }

                await StartGame();
            }
            catch
            { 
                Console.WriteLine("Something went wrong changing the readystate");
            }
            
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var users = UsersController.RemoveUser(Context.ConnectionId);
            await Clients.Others.SendAsync("UpdateLobby", users);

            // Check if ALL users are ready (and minimum 3 players), if so send the countdown and start the game
            // Just in case everyone but 1 person is ready and that person leaves AND THE GAME IS NOT STARTING YET
            if (!UsersController.isGameStarting() && UsersController.ShouldGameStart()) {
                await StartGame();
                return;
            }

            // In case the game shouldn't start anymore but is (If more people leave and the total playercount goes under the min requirement)
            // Eg. 3 (all) players are ready and 1 leaves. This leaves them under the min requirement of 3 so the game stops
            if (UsersController.isGameStarting() && !UsersController.ShouldGameStart())
            {
                await Clients.All.SendAsync("StopCountdownToGame");
                return;
            }
            await base.OnDisconnectedAsync(exception);
        }

        private async Task StartGame()
        {
            // This is the function called when a game is starting
            // First step is to tell the clients that the game is starting
            // They will connect to the gamehub which handles the game
            await Clients.All.SendAsync("StartCountdownToGame");
            var users = UsersController.GetAllUsers();
            await GameController.SetUpNewGame(users);
            // Start a new game in the game controller

        }


    }
}
