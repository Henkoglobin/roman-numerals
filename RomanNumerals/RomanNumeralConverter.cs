using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RomanNumerals {

    public class RomanNumeralConverter : INumeralConverter {
        private static readonly IReadOnlyList<(int value, char character, bool validInSubtraction)> TallyMarkers = new[] {
                // Arranged in ascending order, for convenience.
                (value: 1, character: 'I', validInSubtraction: true),
                (value: 5, character: 'V', validInSubtraction: false),
                (value: 10, character: 'X', validInSubtraction: true),
                (value: 50, character: 'L', validInSubtraction: false),
                (value: 100, character: 'C', validInSubtraction: true),
                (value: 500, character: 'D', validInSubtraction: false),
                (value: 1000, character: 'M', validInSubtraction: false),
            }
            .OrderByDescending(marker => marker.value)
            .ToList();

        public string ToNumeral(int value) {
            if (value <= 0) {
                throw new ArgumentException($"{nameof(value)} must greater than zero.");
            }

            // The available tally markers would actually suffice to represent numbers
            // up to 3999, but the specification specifies a range up to 3000.
            if (value > 3000) {
                throw new ArgumentException($"{nameof(value)} must not be greater than 3000");
            }

            var resultBuilder = new StringBuilder();
            var currentMarkerIndex = 0;
            while (value > 0) {
                var (currentMarkerValue, currentMarkerCharacter, _) = TallyMarkers[currentMarkerIndex];
                if (value >= currentMarkerValue) {
                    resultBuilder.Append(currentMarkerCharacter);
                    value -= currentMarkerValue;
                } else {
                    currentMarkerIndex++;

                    // Check if there's another value we can subtract from the current one to get further
                    var other = TallyMarkers
                        .Skip(currentMarkerIndex)
                        .Where(m => m.validInSubtraction)
                        .FirstOrDefault(x => value >= currentMarkerValue - x.value);
                    if (other != default) {
                        resultBuilder.Append(other.character)
                            .Append(currentMarkerCharacter);
                        value -= (currentMarkerValue - other.value);
                    }
                }
            }

            return resultBuilder.ToString();
        }
    }

    public class RomanNumeralConverter2 : INumeralConverter {
        private enum TallyMarkerType {
            Single, Five, Ten
        }

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

        private char[] MakeTally(
            int value, int power,
            char single, char? five,
            char? ten
        ) => FormatTallyMarkersImpl(
            ToTallyMarkers((value / IntPow(power)) % 10),
            single, five, ten
        ).ToArray();

        private static int IntPow(int power)
            => (int)Math.Pow(10, power);

        private IEnumerable<TallyMarkerType> ToTallyMarkers(int singleDigit) {
            if (singleDigit < 0 || singleDigit > 9) {
                throw new ArgumentException($"Required: 0 <= {nameof(singleDigit)} <= 9");
            }

            if (singleDigit == 9) {
                return new[] { TallyMarkerType.Single, TallyMarkerType.Ten };
            } else if (singleDigit >= 5) {
                return new[] { TallyMarkerType.Five }.Concat(ToTallyMarkers(singleDigit - 5));
            } else if (singleDigit == 4) {
                return new[] { TallyMarkerType.Single, TallyMarkerType.Five };
            } else {
                return Enumerable.Repeat(TallyMarkerType.Single, singleDigit);
            }
        }

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
