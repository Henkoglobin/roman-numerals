using System;

namespace RomanNumerals.Console {
    using Console = System.Console;

    class Application {
        private readonly INumeralConverter numeralConverter;

        public Application(
            INumeralConverter numeralConverter
        ) {
            this.numeralConverter = numeralConverter;
        }

        internal int Run(int? number) {
            try {
                Console.WriteLine(
                    numeralConverter.ToNumeral(
                        number ?? Int32.Parse(Console.In.ReadToEnd())
                    )
                );
                return 0;
            } catch (ArgumentException) {
                Console.Error.WriteLine($"Could not format number {number} to numeral. "
                    + "See --help for details on which numbers can be formatted.");
                return 1;
            } catch (FormatException) {
                Console.Error.WriteLine("Could not parse input. Please only enter integral numbers. "
                    + "See --help for details on which numbers can be formatted.");
                return 1;
            }
        }
    }
}
