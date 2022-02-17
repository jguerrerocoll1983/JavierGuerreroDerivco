using Microsoft.Extensions.Logging;
using Question1.Interfaces;

namespace Question1.Core
{
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
        /// <param name="testString"></param>
        /// <returns></returns>
        public bool Verify(string testString)
        {
            var result = _questionOneConverter.Decode(_questionOneConverter.Encode(testString));
            return testString == result;
        }
    }
}
