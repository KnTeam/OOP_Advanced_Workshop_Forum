namespace Forum.App
{
    using System;

    using Microsoft.Extensions.DependencyInjection;

    using Contracts;
    using Factories;
    using Forum.App.Models;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            var serviceProvider = ConfigureServices();
            var forumViewEngine = new ForumViewEngine();
            var session = new Session();
            var commandFactory = new CommandFactory(serviceProvider);
            // IMainController menu = new MenuController(new LabelFactory(), new ForumViewEngine());
            IMainController menu = new MenuController(serviceProvider, forumViewEngine, session, commandFactory);

            Engine engine = new Engine(menu);
            engine.Run();
        }

        private static IServiceProvider ConfigureServices()
        {
            throw new System.NotImplementedException();
        }
    }
}
