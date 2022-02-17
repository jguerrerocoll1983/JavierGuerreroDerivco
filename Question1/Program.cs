using Microsoft.Extensions.Logging;
using Question1.Core;

namespace Question1
{
    /// <summary>
    /// Refactoring and bug fixing for Question #1, done by Javier Guerrero Coll, a few considerations about my implementation:
    /// - separated in different classes to follow the single resposibility priniciple, 
    /// - avoided to have console.Logs in QuestionOneConverter and used the abstraction ILogger for it, related with Dependency Inversion Principle.
    /// - didn't setup dependency injection as seemed a bit overkill for this simple exercise.
    /// </summary>
    public class Program
    {        
        public static int Main(string[] args)
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
}
