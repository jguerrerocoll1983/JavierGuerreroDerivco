
using Microsoft.Extensions.Logging;
using Question2.Configuration;
using Question2.Core;

namespace Question2
{
    public class Program
    {
        public static void Main()
        {
            var cardGameConfiguration = new CardGameConfiguration(
                playerOneName: "Player One",
                playerTwoName: "Player Two",
                numberOfDecks: 3,
                numberOfCardsPerSuit: 20,
                suits: new string[] { "Clubs", "Diamonds", "Hearts", "Spades" },
                checkSuitForHighCard: true,
                addJoker: true,
                areTiesAllowed: true);

            var card = new HighCardGame(cardGameConfiguration, Logger);
            card.Play();
        }

        private static readonly ILogger Logger = GetConsoleLogger();

        private static ILogger GetConsoleLogger()
        {
            using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            return loggerFactory.CreateLogger<Program>();
        }
    }
}
