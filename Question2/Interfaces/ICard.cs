namespace Question2.Interfaces
{
    public interface ICard
    {
        int Number { get; }
        string Suit { get; }
        string GetCardDisplayName();
    }
}
