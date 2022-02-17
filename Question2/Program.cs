
using Microsoft.Extensions.Logging;

namespace Question2
{
    public class Program
    {
        public static void Main(string[] args)
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

            var card = new HighCardGame(cardGameConfiguration, _logger);
            card.Play();
        }

        private readonly static ILogger _logger = GetConsoleLogger();

        private static ILogger GetConsoleLogger()
        {
            using ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            return loggerFactory.CreateLogger<Program>();
        }
    }
}
