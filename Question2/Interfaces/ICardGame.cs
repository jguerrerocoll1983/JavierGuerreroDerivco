namespace Question2
{
    public interface ICardGame
    {
        int CurrentRound { get; }
        int RemainingCards { get; }
    }
}
