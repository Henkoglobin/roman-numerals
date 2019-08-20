using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RomanNumerals {
    /// <inheritdoc />
    public class RomanNumeralConverter : INumeralConverter {
        private enum TallyMarkerType {
            Single, Five, Ten
        }

        /// <inheritdoc />
        public string ToNumeral(int value) {
            if (value <= 0) {
                throw new ArgumentException($"{nameof(value)} must greater than zero.");
            }

            // The available tally markers would actually suffice to represent numbers
            // up to 3999, but the specification specifies a range up to 3000.
            if (value > 3000) {
                throw new ArgumentException($"{nameof(value)} must not be greater than 3000");
            }

            return new StringBuilder()
                .Append(MakeTally(value, 3, 'M', null, null))
                .Append(MakeTally(value, 2, 'C', 'D', 'M'))
                .Append(MakeTally(value, 1, 'X', 'L', 'C'))
                .Append(MakeTally(value, 0, 'I', 'V', 'X'))
                .ToString();
        }

        /// <summary>
        /// Formats a single digit of the <paramref name="value"/> into its roman representation,
        /// depending on its position.
        /// </summary>
        /// <param name="value">The integer to format</param>
        /// <param name="power">
        /// The position in the number to format, counted from the right.
        /// </param>
        /// <param name="single">The character to represent the value one for the current digit</param>
        /// <param name="five">The character to represent the value five for the current digit</param>
        /// <param name="ten">
        /// The character to represent the value one for the next digit (i.e. ten for the current digit)
        /// </param>
        /// <returns>An array of characters to represent the indicated digit of the number.</returns>
        /// <example>
        /// To format the least-significant digit of the number 13 (i.e., 3), the following parameters
        /// would be passed:
        /// - value: 13 (the number to format)
        /// - power:  0 (indicating that the digit specifying the 10^0s should be formatted)
        /// - single: 'I' (because ones are counted with I)
        /// - five: 'V' (because fives are counted with V)
        /// - ten: 'X' (because tens are counted with X)
        ///
        /// When formatting the next digit, the parameters are changed accordingly:
        /// - value: 13
        /// - power:  1 (to format the next higher digit)
        /// - single: 'X' (because we're now formatting tens, not ones)
        /// - five: 'L' (because 50s are counted with L)
        /// - ten: 'C' (because 100s are counted with C)
        /// </example>
        private char[] MakeTally(
            int value, 
            int power,
            char single, 
            char? five,
            char? ten
        ) => FormatTallyMarkersImpl(
            ToTallyMarkers((value / IntPow(power)) % 10),
            single, five, ten
        ).ToArray();

        /// <summary>
        /// Calculates 10 to the power of <paramref name="power"/>.
        /// </summary>
        /// <param name="power">The power to which to raise ten.</param>
        /// <returns>Ten to the power of <paramref name="power"/> </returns>
        private static int IntPow(int power)
            => (int)Math.Pow(10, power);

        /// <summary>
        /// Determines the representation of a single digit in abstract 'tally markers'.
        /// Tally markers are later formatted according to the position of the digit in the number.
        ///
        /// Representing digits as abstract tally markers allows us to employ the same logic for all digits.
        /// </summary>
        /// <param name="singleDigit">The digit to format</param>
        /// <returns></returns>
        private IEnumerable<TallyMarkerType> ToTallyMarkers(int singleDigit) {
            if (singleDigit < 0 || singleDigit > 9) {
                throw new ArgumentException($"Required: 0 <= {nameof(singleDigit)} <= 9");
            }

            if (singleDigit == 9) {
                // Nine is a special case, as it's represented as IX - one less than ten.
                return new[] { TallyMarkerType.Single, TallyMarkerType.Ten };
            } else if (singleDigit >= 5) {
                // For values greater than (or equal to) five, just write a five-marker and add
                // more markers for any higher values
                return new[] { TallyMarkerType.Five }.Concat(ToTallyMarkers(singleDigit - 5));
            } else if (singleDigit == 4) {
                // Four is another special case, as it's just IV - one less than five.
                return new[] { TallyMarkerType.Single, TallyMarkerType.Five };
            } else {
                // Repeat the marker for single n-times
                return Enumerable.Repeat(TallyMarkerType.Single, singleDigit);
            }
        }

        /// <summary>
        /// Maps an abstract tally marker into the specified characters
        /// </summary>
        /// <param name="markers">The tally marker to format</param>
        /// <param name="single">The character to denote ones</param>
        /// <param name="five">The character to denote fives</param>
        /// <param name="ten">The character to denote tens</param>
        /// <returns>An enumeration of characters that represent the specified digit</returns>
        private IEnumerable<char> FormatTallyMarkersImpl(
            IEnumerable<TallyMarkerType> markers,
            char single,
            char? five,
            char? ten
        ) {
            foreach (var marker in markers) {
                switch (marker) {
                    case TallyMarkerType.Single:
                        yield return single;
                        break;
                    case TallyMarkerType.Five:
                        yield return five ?? throw new ArgumentException();
                        break;
                    case TallyMarkerType.Ten:
                        yield return ten ?? throw new ArgumentException();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
