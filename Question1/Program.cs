using Microsoft.Extensions.Logging;

namespace Question1
{
    /// <summary>
    /// Refactoring and bug fixing for Question #1, done by Javier Guerrero Coll, a few considerations about my implementation:
    /// - separated in different classes to follow the single resposibility priniciple, 
    /// - avoided to have console.Logs in QuestionOneConverter and used the abstraction ILogger for it, related with Dependency Inversion Principle.
    /// - didn't setup dependency injection as seemed a bit overkill for this simple exercise.
    /// - didn't separate the classes into different files, folders. To make it more readable, but ideally at least every class would live in different files.
    /// </summary>
    class Program
    {        
        static int Main(string[] args)
        {
            var questionOneConverterVerifier = new QuestionOneConverterVerifier(_logger);

            var stringToAnalyse = GetStringToAnalyse(args);

            if (questionOneConverterVerifier.Verify(stringToAnalyse))
            {
                _logger.LogInformation("Test succeeded");
                return 0;
            }

            _logger.LogError("Test failed");
            return -1;
        }

        private readonly static ILogger _logger = GetConsoleLogger();

        private static string GetStringToAnalyse(string[] args)
        {
            var test_string = args?.Length == 1 ? args[0] : "This is a test string";
            _logger.LogInformation("Analysing test_string: {test_string}", test_string);

            return test_string;
        }


        private static ILogger GetConsoleLogger()
        {
            using ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            return loggerFactory.CreateLogger<Program>();
        }
    }

    public interface IStringConverter
    {
        string Encode(string input);
        string Decode(string input);
    }

    public class QuestionOneConverter: IStringConverter
    {
        private readonly char[] _transcode = new char[65];
        private readonly ILogger _logger;

        public QuestionOneConverter(ILogger logger)
        {
            _logger = logger;
            Init();
        }

        public string Encode(string input)
        {
            var l = input.Length;
            var cb = (l / 3 + (Convert.ToBoolean(l % 3) ? 1 : 0)) * 4;

            var output = new char[cb];
            for (var i = 0; i < cb; i++)
            {
                output[i] = '=';
            }

            var c = 0;
            var reflex = 0;
            const int s = 0x3f;

            for (var j = 0; j < l; j++)
            {
                reflex <<= 8;
                reflex &= 0x00ffff00;
                reflex += input[j];

                var x = ((j % 3) + 1) * 2;
                var mask = s << x;
                while (mask >= s)
                {
                    var pivot = (reflex & mask) >> x;
                    output[c++] = _transcode[pivot];
                    var invert = ~mask;
                    reflex &= invert;
                    mask >>= 6;
                    x -= 6;
                }
            }

            switch (l % 3)
            {
                case 1:
                    reflex <<= 4;
                    output[c++] = _transcode[reflex];
                    break;
                case 2:
                    reflex <<= 2;
                    output[c++] = _transcode[reflex];
                    break;

            }

            _logger.LogInformation("Encoding: {input} --> {output}", input, new string(output));
            
            return new string(output);
        }


        public string Decode(string input)
        {
            var l = input.Length;
            var cb = (l / 4 + ((Convert.ToBoolean(l % 4)) ? 1 : 0)) * 3;
            var output = new char[cb];
            var c = 0;
            var bits = 0;
            var reflex = 0;
            for (var j = 0; j < l; j++)
            {
                reflex <<= 6;
                bits += 6;
                var fTerminate = ('=' == input[j]);
                if (!fTerminate)
                    reflex += IndexOf(input[j]);

                while (bits >= 8)
                {
                    int mask = 0x000000ff << (bits % 8);
                    output[c++] = (char)((reflex & mask) >> (bits % 8));
                    int invert = ~mask;
                    reflex &= invert;
                    bits -= 8;
                }

                if (fTerminate)
                    break;
            }

            _logger.LogInformation("Decoding: {input} --> {output}", input, new string(output));
            
            return new string(output);
        }

        private int IndexOf(char ch)
        {
            int index;
            for (index = 0; index < _transcode.Length; index++)
            {
                if (ch == _transcode[index]) { break; }
            }

            return index;
        }

        private void Init()
        {
            for (var i = 0; i < 64; i++)
            {
                _transcode[i] = (char)('A' + i);
                if (i > 25) _transcode[i] = (char)(_transcode[i] + 6);
                if (i > 51) _transcode[i] = (char)(_transcode[i] - 75);
            }
            _transcode[62] = '+';
            _transcode[63] = '/';
            _transcode[64] = '=';
        }
    }

    public interface IStringConversionVerifier
    {
        bool Verify(string input);
    }

    /// <summary>
    /// Class for verifying that a string does NOT change if it's encoded and decoded.
    /// </summary>
    public class QuestionOneConverterVerifier : IStringConversionVerifier
    {
        private readonly IStringConverter _questionOneConverter;

        public QuestionOneConverterVerifier(ILogger logger)
        {
            _questionOneConverter = new QuestionOneConverter(logger);
        }

        /// <summary>
        /// Given a string returns true if after encoding a string and decoding the encoded version, the result is the same as the input string.
        /// </summary>
        /// <param name="test_string"></param>
        /// <returns></returns>
        public bool Verify(string test_string)
        {
            var result = _questionOneConverter.Decode(_questionOneConverter.Encode(test_string));
            return test_string == result;
        }
    }
}
