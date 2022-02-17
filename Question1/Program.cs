using Microsoft.Extensions.Logging;
using Question1.Core;

namespace Question1
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var questionOneConverterVerifier = new QuestionOneConverterVerifier(Logger);

            var stringToAnalyse = GetStringToAnalyse(args);

            if (questionOneConverterVerifier.Verify(stringToAnalyse))
            {
                Logger.LogInformation("Test succeeded");
                return 0;
            }

            Logger.LogError("Test failed");
            return -1;
        }

        private static readonly ILogger Logger = GetConsoleLogger();

        private static string GetStringToAnalyse(string[] args)
        {
            var testString = args.Length == 1 ? args[0] : "This is a test string";
            Logger.LogInformation("Analysing test_string: {test_string}", testString);

            return testString;
        }

        private static ILogger GetConsoleLogger()
        {
            using ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            return loggerFactory.CreateLogger<Program>();
        }
    }
}
