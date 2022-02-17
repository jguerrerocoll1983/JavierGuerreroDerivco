namespace Question2
{

    public class Card : ICard
    {
        public Card(string suit, int number)
        {
            Suit = suit;
            Number = number;
        }

        public int Number { get; }
        public string Suit { get; }

        public string GetCardDisplayName()
        {
            return Suit == ICardDeck.JOKER_SUIT_NAME ?
                ICardDeck.JOKER_SUIT_NAME : 
                $"{Number} of {Suit}";
        }
    }
}
