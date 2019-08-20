using FluentAssertions;
using System;
using Xunit;

namespace RomanNumerals.Tests {
    public class RomanNumeralConverterTest {
        private readonly INumeralConverter converter = new RomanNumeralConverter();

        [Theory]
        [InlineData(0, "roman numerals cannot represent zero")]
        [InlineData(-1, "negative values are not allowed")]
        [InlineData(3001, "values greater than 3000 are not allowed")]
        public void ToNumeral_FailsForInvalidInput(int value, string reason) {
            converter.Invoking(c => c.ToNumeral(value))
                .Should().Throw<ArgumentException>(reason);
        }

        [Theory]
        [InlineData(1, "I")]
        [InlineData(2, "II")]
        [InlineData(3, "III")]
        public void ToNumeral_UsesAdditiveNotationForSmallValues(int value, string expected) {
            converter.ToNumeral(value)
                .Should().Be(expected, "small values (< 3) are represented as repetitions of I");
        }

        [Theory]
        [InlineData(5, "V")]
        [InlineData(10, "X")]
        [InlineData(50, "L")]
        [InlineData(100, "C")]
        [InlineData(500, "D")]
        [InlineData(1000, "M")]
        public void ToNumeral_UsesLargerTallyMarkersForLargerValues(int value, string expected) {
            converter.ToNumeral(value)
                .Should().Be(expected, "this number can be represented by a single (non-I) digit");
        }

        [Theory]
        [InlineData(20, "XX")]
        [InlineData(30, "XXX")]
        [InlineData(200, "CC")]
        [InlineData(300, "CCC")]
        [InlineData(2000, "MM")]
        [InlineData(3000, "MMM")]
        public void ToNumeral_UsesMultipleLargerTallyMarkersForLargerValues(int value, string expected) {
            converter.ToNumeral(value)
                .Should().Be(expected, "this number requires multiple large tally marker");
        }

        [Theory]
        [InlineData(6, "VI")]
        [InlineData(7, "VII")]
        [InlineData(8, "VIII")]
        [InlineData(11, "XI")]
        [InlineData(12, "XII")]
        [InlineData(15, "XV")]
        [InlineData(16, "XVI")]
        [InlineData(66, "LXVI")]
        [InlineData(166, "CLXVI")]
        [InlineData(666, "DCLXVI")]
        [InlineData(1666, "MDCLXVI")]
        public void ToNumeral_UsesDifferentTallyMarkers(int value, string expected) {
            converter.ToNumeral(value)
                .Should().Be(expected, "this number requires use of differently-sized tally markers (additive)");
        }

        [Theory]
        [InlineData(4, "IV")]
        [InlineData(9, "IX")]
        [InlineData(14, "XIV")]
        [InlineData(99, "XCIX")]
        [InlineData(94, "XCIV")]
        [InlineData(1992, "MCMXCII")]
        public void ToNumeral_UsesSubtractiveNotation(int value, string expected) {
            converter.ToNumeral(value)
                .Should().Be(expected, "this number requires subtractive notation");
        }
    }
}
