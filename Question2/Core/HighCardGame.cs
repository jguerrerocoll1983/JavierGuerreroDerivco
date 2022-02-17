
using Microsoft.Extensions.Logging;

namespace Question2
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
        private List<CardDeck> CardDecks { get; set; }

        public void Play()
        {
            var playerOneCurrentCard = GetCard();
            var playerTwoCurrentCard = GetCard();

            _logger.LogInformation($"\nRound {CurrentRound}");
            _logger.LogInformation($"'{CardGameConfiguration.PlayerOneName}' got a '{playerOneCurrentCard.GetCardDisplayName()}'");
            _logger.LogInformation($"'{CardGameConfiguration.PlayerTwoName}' got a '{playerTwoCurrentCard.GetCardDisplayName()}'");

            var result = RoundWinner(playerOneCurrentCard, playerTwoCurrentCard);
            switch (result)
            {
                case RoundWinnerEnum.PlayerOneWins:
                    _logger.LogInformation($"{CardGameConfiguration.PlayerOneName} wins!");
                    break;

                case RoundWinnerEnum.PlayerTwoWins:
                    _logger.LogInformation($"{CardGameConfiguration.PlayerTwoName} wins!");
                    break;

                case RoundWinnerEnum.Tie:
                    _logger.LogInformation($"It is a tie!");
                    if (CardGameConfiguration.AreTiesAllowed)
                    {
                        _logger.LogInformation("Draw is enabled. The game ends in a tie!");
                        return;
                    }
                    ResolveTie();
                    break;
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
                    else if (indexOfSuitPlayerOne < indexOfSuitPlayerTwo)
                    {
                        return RoundWinnerEnum.PlayerTwoWins;
                    }
                }
                return RoundWinnerEnum.Tie;
            }
            else if (playerOneCard.Number > playerTwoCard.Number)
            {
                return RoundWinnerEnum.PlayerOneWins;
            }
            else
            {
                return RoundWinnerEnum.PlayerTwoWins;
            }
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
