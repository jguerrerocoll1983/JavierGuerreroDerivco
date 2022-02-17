# JavierGuerrero - Derivco Exam
-------------------------------

Next you have all the assessment with my answer for last exercise at the bottom
-------------------------------------------------------------------------------

Refactoring and bug fixing for Question #1, done by Javier Guerrero Coll, 

Question 1
==========

Please take a look at the following code. Please fix, refactor and improve the code to the level that you would consider to be production standard.

```
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Question1
{    
    class Program
    {
        static char[] transcode = new char[64];

        private static void prep()
        {                        
            for (int i = 0; i < 64; i++)  
            {    
                transcode[i] = (char)((int)'A' + i);                
                if (i > 25) transcode[i] = (char)((int)transcode[i] + 6 );
                if (i > 51) transcode[i] = (char)((int)transcode[i] - 0x4b);
            }
            transcode[62] = '+';
            transcode[63] = '/';            
            transcode[64] = '=';        
        }

        static void Main(string[] args) 
        {            
            prep();
  
            string test_string = "This is a test string";

            if ( Convert.ToBoolean( String.Compare( test_string, decode(encode(test_string))))) 
            {
                Console.WriteLine("Test succeeded");                         
            }
            Console.WriteLine("Test failed");                              
        }
       

        private static string encode(string input)
        {
            int l = input.Length;
            int cb = (l/3 + ( Convert.ToBoolean(l % 3) ? 1 : 0) ) * 4;   

            char[] output = new char[cb];
            for ( int i = 0; i < cb; i++ )
            {
                output[i] = '=';
            }
 
            int c = 0;
            int reflex = 0;
            const int s = 0x3f;             
 
            for (int j = 0; j < l; j++)
            {
                reflex <<= 8;
                reflex &= 0x00ffff00;         
                reflex += input[j];
 
                int x = ((j%3)+1)*2;          
                int mask = s << x;
                while (mask >= s)
                {
                    int pivot =  (reflex & mask) >> x;
                    output[c++] = transcode[pivot];
                    int invert = ~mask;
                    reflex &= invert;
                    mask >>= 6;
                    x -= 6;
                }
            }
 
            switch (l%3)
            {
                case 1:
                    reflex <<= 4;
                    output[c++] = transcode[reflex];   
                
                case 2:
                    reflex <<= 2;
                    output[c++] = transcode[reflex];
                    break;
                
            }
            Console.WriteLine("{0} --> {1}\n", input, new string( output) );
            return new string( output );
        }


        private static string decode(string input)
        {
            int l = input.Length;
            int cb = (l/4 + ((Convert.ToBoolean(l%4))?1:0))*3+1;   
            char[] output = new char[cb];        
            int c = 0;
            int bits = 0;
            int reflex = 0;
            for (int j = 0; j < l; j++)
            {
                reflex <<= 6;
                bits += 6;
                bool fTerminate = ('=' == input[j]);
                if (!fTerminate)
                    reflex += indexOf(input[j]);
    
                while (bits >= 8)
                {
                    int mask = 0x000000ff << (bits % 8);                                        
                    output[c++] = (reflex & mask) >> (bits % 8);    
                    int invert = ~mask;
                    reflex &= invert;
                    bits -= 8;
                }
 
                if (fTerminate)
                    break;
            }
            Console.WriteLine("{0} --> {1}\n", input, new string( output ));
            return new string( output );                    
        }

        private static int indexOf(char ch)
        {
            int index;
            for (index = 0; index < transcode.Length; index++)
            if (ch == transcode[index])
                break;    
            return index;
        }
    }
}
```


Question 2
==========
Below is an implementation for a game of HighCard, where two cards are drawn from a 52 card deck, and the highest card wins.

Please can you refactor this code to add in the ability to:

 1) Support ties when the face value of the cards are the same.
 2) Allow for the ties to be resolved as a win by giving the different suits precedence.
 3) Support for Multiple Decks. 
 4) Support the abilty to never end in a tie, by dealing a further card to each player.
 5) Make one of the cards in the deck a wild card ( beats all others ).
 6) Now make the game work for a deck with 20 cards per suit
 
Please apply all the best practices you would in what you consider to be "production ready code"
```
using System;
using System.Text;

namespace Question2
{
  class HighCard
  {
    public HighCard()
    {
    }

    public bool Play()
    {
      Random rnd = new Random();

      int i = rnd.Next() % 52 + 1;
      int j = rnd.Next() % 52 + 1;

      return i < j;
    }
  }
  
  class Program
  {
    static void Main(string[] args)
    {
      HighCard card = new HighCard();
      if (card.Play())
      {
        Console.WriteLine("win");
      }
      else
      {
        Console.WriteLine("lose");
      }
    }
  }
}
```

Question 3
==========

Please can you give a written prose on your design choices in question 2

Done by Javier Guerrero Coll: 
----------------------------------------------------------------------------
I tried to taking into account the SOLID principles, I would start my explanation by talking about them in regard my implementation:
- Single reponsibility principle, each class has only a reason for change, for instance separating logging responsibility by using an abstraction instead directly calling Console.WriteLine.
- Open/Close principle, I could have implemented Card differently maybe using inheritance to define different kind of cards, but I thought the entity I have defined for Card is a good approach.
- Liskov substitution principle, in this case it does not apply as I did not define entities at the same level, as could be different card classes, for instance the wildcard.
- Interface segragation principle, I have created interfaces for the current classes, trying to only take what is extrictly necessary to not make the clients depend on methods it does not use. In this case being a new project the concern about extending instead of creating new interfaces (being this last one the advised one) did not apply from my point of view.
- Dependency Inversion principle, this was followed to avoid to make details depend on abstraction, again a good example is the use of the ILogger instead creating the instance and handling in the child class. I would like to have setup dependency injection to properly dispatch instances setup for interfaces, instead of passing them directly, but for this small project felt a bit too much.

Now talking about the implementation itself, I have defined the next classes and interfaces which I will describe one by one:
- Program: It directly starts the game with the configuration set up on cardGameConfiguration instance, I could have implemented something more sofisticated allowing to take the configuration from command line, but I preferred to put more attention on other details.
- Configuration/CardGameConfiguration: It is a model for the different configurations available for the game, this could also be used to retrieve the configuration from a config file.
- Core/Card: A model for representing the entity of a card, it has two properties
	- Suit: It expectes to receive a array of strings, it should be ordered from less to higher value for tie break. For WildCard, which I called JOKER, I made it to be one number more than the highest card (but that is part of CardDeck implementation).
	- Number: an integer
- Core/CardDeck: It is implemented a deck which can be built with different attributes (numberOfCards, suits and addJoker) to define those aspects, the card drawn is always the first one, cards are shuffled during initialisation.
- Core/HighCardGame: I thought at first of creating an abstract "AbstractCardGame", and define different games inheriting from it, only implementing the minimum code necessary for each game, but I ended up going with a single HighCardGame which is configurable attending to all the rules defined for this exam.
- Core/RoundWinnerEnum: An enum to indicate the different possible outcomes of a round.
- Interfaces.

So this is explanation about my approach and a bit of the reasoning behing the implementations.
 
