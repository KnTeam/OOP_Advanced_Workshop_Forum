﻿namespace Forum.App.Commands
{
    using Forum.App.Contracts;

    public class LogOutMenuCommand : ICommand
    {
        private ISession session;
        private IMenuFactory menuFactory;

        public LogOutMenuCommand(ISession session, IMenuFactory menuFactory)
        {
            this.session = session;
            this.menuFactory = menuFactory;
        }

        public IMenu Execute(params string[] args)
        {
            this.session.Reset();

            IMenu menu = this.menuFactory.CreateMenu("MainMenu");

            return menu;
        }
    }
}
