using Microsoft.Extensions.DependencyInjection;

namespace RomanNumerals.Console {
    /// <summary>
    /// The main entry point for the application.
    /// This project uses the DragonFruit application model
    /// (https://github.com/dotnet/command-line-api/wiki/DragonFruit-overview)
    /// in order to provide a strongly-types main method.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Formats a number in roman numerals.
        /// </summary>
        /// <param name="argument">
        /// The number to format. Valid numbers are between 1 and 3000, inclusively.
        /// If no value is provided, it is read from stdin.
        /// </param>
        /// <returns>Exit Code 0 if the number was formatted successfully, Exit Code 1 otherwise.</returns>
        static int Main(int? argument = null) {
            var serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection);

            return serviceCollection
                .BuildServiceProvider()
                .GetRequiredService<Application>()
                .Run(argument);
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
            => serviceCollection
                .AddTransient<INumeralConverter, RomanNumeralConverter2>()
                .AddSingleton<Application>()
            ;
    }
}
