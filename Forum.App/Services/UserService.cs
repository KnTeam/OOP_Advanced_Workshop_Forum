namespace Forum.App.Services
{
    using Forum.App.Contracts;
    using System;
    using Forum.DataModels;
    using Forum.Data;
    using System.Linq;

    public class UserService : IUserService
    {
        private ForumData forumData;
        private ISession session;

        public UserService(ForumData forumData, ISession session)
        {
            this.forumData = forumData;
            this.session = session;
        }

        public User GetUserById(int userId)
        {
            throw new NotImplementedException();
        }

        public string GetUserName(int userId)
        {
            return this.forumData.Users.FirstOrDefault(u => u.Id == userId).Username;
        }

        public bool TryLogInUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            User userToLog = forumData.Users.FirstOrDefault(x => x.Username == username && x.Password == password);

            session.Reset();
            session.LogIn(userToLog);

            return true;
        }

        public bool TrySignUpUser(string username, string password)
        {
            bool validUsername = !string.IsNullOrWhiteSpace(username) && username.Length > 3;
            bool validPassword = !string.IsNullOrWhiteSpace(password) && password.Length > 3;

            if (!validUsername || !validPassword)
            {
                throw new ArgumentException("Username and Password must be longer than 3 symbols!");
            }

            bool userAlreadyExists = forumData.Users.Any(x => x.Username == username);

            if (userAlreadyExists)
            {
                throw new InvalidOperationException("Username taken!");
            }

            int newUserId = forumData.Users.LastOrDefault()?.Id + 1 ?? 1;

            User newUser = new User(newUserId, username, password);

            forumData.Users.Add(newUser);
            forumData.SaveChanges();

            this.TryLogInUser(username, password);

            return true;
        }
    }
}
