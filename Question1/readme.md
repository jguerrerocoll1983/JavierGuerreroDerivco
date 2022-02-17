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
