namespace Question2
{
    public class CardDeck : ICardDeck
    {
        private readonly int _numberOfCardsPerSuit;
        private readonly string[] _suits;
        private readonly bool _addJoker;
        private readonly Random _random = new();

        private List<Card> _listOfCards;

        public CardDeck(string[] suits, int numberOfCardsPerSuit = 12, bool addJoker = true)
        {
            ValidateArguments(suits, numberOfCardsPerSuit, addJoker);

            _suits = suits;
            _numberOfCardsPerSuit = numberOfCardsPerSuit;
            _addJoker = addJoker;
            _listOfCards = new List<Card>();

            Init();
        }

        public int RemainingCards => _listOfCards.Count;

        public ICard GetCard()
        {           
            var selectedCard = _listOfCards.First();            
            _listOfCards.RemoveAt(0);

            return selectedCard;
        }

        private static void ValidateArguments(string[] suits, int numberOfCardsPerSuit, bool addJoker)
        {
            if (numberOfCardsPerSuit < 1 || suits.Length < 1 || numberOfCardsPerSuit * suits.Length < 2 && !addJoker)
            {
                throw new ArgumentException("Invalid arguments for CardDeck constructor, the should be at least a suit, and at least to card (can include the joker)");
            }

            if (suits.Contains(ICardDeck.JOKER_SUIT_NAME))
            {
                throw new ArgumentException("There cannot be a suit called joker");
            }

            if (suits.Length != suits.Distinct().Count())
            {
                throw new ArgumentException("There cannot be repated names for suits");
            }
        }

        private void Init()
        {
            foreach (var suit in _suits)
            {                
                for (var cardNumber = 1; cardNumber <= _numberOfCardsPerSuit; cardNumber++)
                {
                    _listOfCards.Add(new Card(suit, cardNumber));
                }
            }

            if (_addJoker)
            {
                _listOfCards.Add(new Card(ICardDeck.JOKER_SUIT_NAME, _numberOfCardsPerSuit + 1));
            }

            Shuffle();
        }

        private void Shuffle()
        {
            _listOfCards = _listOfCards.OrderBy(c => _random.Next()).ToList();
        }
    }
}
