using Microsoft.Extensions.Logging;
using Question2.Configuration;
using Question2.Interfaces;

namespace Question2.Core
{
    public class HighCardGame : ICardGame
    {
        private readonly ILogger _logger;
        private readonly Random _random = new();

        public HighCardGame(CardGameConfiguration cardGameConfiguration, ILogger logger)
        {
            _logger = logger;

            if (cardGameConfiguration.NumberOfDecks < 1)
            {
                throw new ArgumentException("There should be at least 1 deck.");
            }

            CardGameConfiguration = cardGameConfiguration;
            CardDecks = new List<CardDeck>();

            InitCardDeck();
        }

        public int CurrentRound { get; private set; } = 1;
        public int RemainingCards => CardDecks.Sum(d => d.RemainingCards);

        private CardGameConfiguration CardGameConfiguration { get; }
        private List<CardDeck> CardDecks { get; }

        public void Play()
        {
            var playerOneCurrentCard = GetCard();
            var playerTwoCurrentCard = GetCard();

            _logger.LogInformation("\nRound {CurrentRound}", CurrentRound);
            _logger.LogInformation("'{CardGameConfiguration.PlayerOneName}' got a '{playerOneCurrentCard.GetCardDisplayName()}'", CardGameConfiguration.PlayerOneName, playerOneCurrentCard.GetCardDisplayName());
            _logger.LogInformation("'{CardGameConfiguration.PlayerTwoName}' got a '{playerTwoCurrentCard.GetCardDisplayName()}'", CardGameConfiguration.PlayerTwoName, playerTwoCurrentCard.GetCardDisplayName());

            var result = RoundWinner(playerOneCurrentCard, playerTwoCurrentCard);
            switch (result)
            {
                case RoundWinnerEnum.PlayerOneWins:
                    _logger.LogInformation("{CardGameConfiguration.PlayerOneName} wins!", CardGameConfiguration.PlayerOneName);
                    break;

                case RoundWinnerEnum.PlayerTwoWins:
                    _logger.LogInformation("{CardGameConfiguration.PlayerTwoName} wins!", CardGameConfiguration.PlayerTwoName);
                    break;

                case RoundWinnerEnum.Tie:
                    _logger.LogInformation("It is a tie!");
                    if (CardGameConfiguration.AreTiesAllowed)
                    {
                        _logger.LogInformation("Draw is enabled. The game ends in a tie!");
                        return;
                    }
                    ResolveTie();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private ICard GetCard()
        {
            var selectedDeckIndex = _random.Next(CardDecks.Count);
            var selectedDeck = CardDecks[selectedDeckIndex];
            var selectedCard = selectedDeck.GetCard();
            
            if (selectedDeck.RemainingCards < 1)
            {
                CardDecks.RemoveAt(selectedDeckIndex);
            }

            return selectedCard;
        }

        private RoundWinnerEnum RoundWinner(ICard playerOneCard, ICard playerTwoCard)
        {
            if (playerOneCard.Number == playerTwoCard.Number)
            {
                if (CardGameConfiguration.CheckSuitForHighCard)
                {
                    var indexOfSuitPlayerOne = Array.IndexOf(CardGameConfiguration.Suits, playerOneCard.Suit);
                    var indexOfSuitPlayerTwo = Array.IndexOf(CardGameConfiguration.Suits, playerTwoCard.Suit);
                    if (indexOfSuitPlayerOne > indexOfSuitPlayerTwo)
                    {
                        return RoundWinnerEnum.PlayerOneWins;
                    }
                    if (indexOfSuitPlayerOne < indexOfSuitPlayerTwo)
                    {
                        return RoundWinnerEnum.PlayerTwoWins;
                    }
                }
                return RoundWinnerEnum.Tie;
            }

            return playerOneCard.Number > playerTwoCard.Number ? 
                RoundWinnerEnum.PlayerOneWins : 
                RoundWinnerEnum.PlayerTwoWins;
        }

        private void ResolveTie()
        {
            if (RemainingCards < 2)
            {
                _logger.LogInformation("There are not enough cards to keep playing. The game ends in a tie!");
                return;
            }

            PlayNextRound();
        }

        private void PlayNextRound()
        {
            CurrentRound++;
            Play();
        }

        private void InitCardDeck()
        {
            for (var i = 0; i < CardGameConfiguration.NumberOfDecks; i++)
            {
                CardDecks.Add(new CardDeck(CardGameConfiguration.Suits, CardGameConfiguration.NumberOfCardsPerSuit, CardGameConfiguration.AddJoker));
            }
        }
    }
}
