namespace Question2
{
    public interface ICardDeck
    {
        public static readonly string JOKER_SUIT_NAME = "JOKER";

        int RemainingCards { get; }
        ICard GetCard();
    }
}
