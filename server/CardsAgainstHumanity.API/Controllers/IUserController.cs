using System.Collections.Generic;
using CardsAgainstHumanity.Models;

namespace CardsAgainstHumanity.API.Controllers
{
    public interface IUserController
    {
        void AddUser(User user);
        int AmountOfUsers();
        List<User> ChangeReadyState(string connectionId);
        List<User> GetAllUsers();
        bool isGameStarting();
        bool IsNameAvailable(string name);
        List<User> RemoveUser(string id);
        bool ShouldGameStart();
    }
}