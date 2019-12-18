using CardsAgainstHumanity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardsAgainstHumanity.API.Controllers
{
    public class UserController : IUserController
    {
        static List<User> users = new List<User>();
        static bool gameStarting = false;

        public List<User> GetAllUsers()
        {
            return users;
        }

        public void AddUser(User user)
        {
            users.Add(user);
        }

        public List<User> RemoveUser(string id)
        {
            var userToDelete = users.Find(u => u.ConnectionId == id);
            users.Remove(userToDelete);
            return users;
        }

        public bool IsNameAvailable(string name)
        {
            var matches = users.Where(u => u.Name == name);
            return (matches == null || matches.Count() < 1);
        }

        public int AmountOfUsers()
        {
            return users.Count();
        }

        public List<User> ChangeReadyState(string connectionId)
        {
            var userToChange = users.Find(u => u.ConnectionId == connectionId);
            if (userToChange != null)
            {
                userToChange.ReadyState = !userToChange.ReadyState;
            }

            return users;
        }

        public bool isGameStarting()
        {
            return gameStarting;
        }

        public bool ShouldGameStart()
        {
            if (!users.All(u => u.ReadyState == true) || users.Count() < 3)
            {
                gameStarting = false;
                return false;
            }

            gameStarting = true;
            return true;
        }
    }
}
