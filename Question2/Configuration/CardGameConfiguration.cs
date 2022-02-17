namespace Question2.Configuration
{
    public class CardGameConfiguration
    {
        public CardGameConfiguration(
            string playerOneName, 
            string playerTwoName,
            int numberOfDecks,
            int numberOfCardsPerSuit, 
            string[] suits, 
            bool checkSuitForHighCard, 
            bool addJoker, 
            bool areTiesAllowed) 
        {
            PlayerOneName = playerOneName;
            PlayerTwoName = playerTwoName;
            NumberOfDecks = numberOfDecks;
            NumberOfCardsPerSuit = numberOfCardsPerSuit;
            Suits = suits;
            CheckSuitForHighCard = checkSuitForHighCard;
            AddJoker = addJoker;
            AreTiesAllowed = areTiesAllowed;
        }

        public string PlayerOneName { get; }
        public string PlayerTwoName { get; }
        public int NumberOfDecks { get; }
        public int NumberOfCardsPerSuit { get; }
        public string[] Suits { get; }
        public bool CheckSuitForHighCard { get; }
        public bool AddJoker { get; }
        public bool AreTiesAllowed { get; }
    }
}
