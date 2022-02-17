namespace Question2
{
    public interface ICard
    {
        int Number { get; }
        string Suit { get; }
        string GetCardDisplayName();
    }
}
