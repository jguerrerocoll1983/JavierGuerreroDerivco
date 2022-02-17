namespace Question2.Interfaces
{
    public interface ICardGame
    {
        int CurrentRound { get; }
        int RemainingCards { get; }
    }
}
